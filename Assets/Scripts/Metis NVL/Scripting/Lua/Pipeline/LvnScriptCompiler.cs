using System.Collections;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.Text;

/// <summary>
/// Compiles source code in the Metis .lvn format into Lua code.
/// </summary>
public static class LvnScriptCompiler
{
    static string[] compiledLines;
    static Mode[] compiledModes;
    
    /// <summary>
    /// Entry point into the compiler. Consumes .lvn source code and returns it as a string of Lua code.
    /// </summary>
    /// <param name="sourceCode"></param>
    /// <returns></returns>
    public static string Compile(string sourceCode)
    {
        string[] src = sourceCode.Split(new string[] { "\r\n", "\n" }, System.StringSplitOptions.None);
        string filename = "placeholder";

        ParseLines(filename, src, 1, 1);

        // Get Lua source code
        string luaSourceCode = ParserUtil.ConcatLines(compiledLines);
        return luaSourceCode;
    }

    static void ParseLines(string filename, string[] lines, int startLineNum, int textLineNum)
    {
        compiledLines = new string[lines.Length];
        compiledModes = new Mode[lines.Length];

        Mode mode = Mode.TEXT;
        for (int n = 0; n < lines.Length; n++)
        {
            string line = lines[n].Trim();

            // End single-line modes
            if (IsSingleLine(mode)) mode = Mode.TEXT;

            // Look for comment starts           
            if (mode == Mode.TEXT && line.StartsWith("#")) mode = Mode.COMMENT;
            if (mode == Mode.TEXT && line.StartsWith("@")) mode = Mode.CODE;

            // Process line
            if (mode == Mode.TEXT)
            {
                compiledModes[n] = mode;
                if (line.Length > 0)
                {
                    line = ParseTextLine(filename, line, textLineNum);
                    textLineNum++;
                }
            }
            else if (mode == Mode.CODE || mode == Mode.MULTILINE_CODE)
            {
                if (line.StartsWith("@@"))
                {
                    compiledModes[n] = Mode.MULTILINE_CODE;

                    line = line.Substring(2);
                    mode = (mode == Mode.MULTILINE_CODE ? Mode.TEXT : Mode.MULTILINE_CODE);
                }
                else
                {
                    compiledModes[n] = mode;
                    if (mode == Mode.CODE && line.StartsWith("@"))
                    {
                        line = line.Substring(1);
                    }
                }

                if (mode == Mode.CODE || mode == Mode.MULTILINE_CODE)
                {
                    line = ParseCodeLine(line);
                }
                else
                {
                    line = "";
                }
            }
            else if (mode == Mode.COMMENT || mode == Mode.MULTILINE_COMMENT)
            {
                if (line.StartsWith("##"))
                {
                    compiledModes[n] = Mode.MULTILINE_COMMENT;

                    mode = (mode == Mode.MULTILINE_COMMENT ? Mode.TEXT : Mode.MULTILINE_COMMENT);
                }
                else
                {
                    compiledModes[n] = mode;
                }

                // Ignore commented lines
                line = "";
            }

            compiledLines[n] = line;
        }
    }

    static string ParseTextLine(string filename, string line, int textLineNum)
    {
        if (line.Length == 0)
        {
            return line; // Ignore empty lines
        }

        List<string> result = new List<string>(8);
        result.Add(BeginParagraphCommand(filename, textLineNum));

        StringBuilder sb = new StringBuilder(line.Length);

        for (int n = 0; n < line.Length; n++)
        {
            char c = line[n];
            if (c == '\\')
            {
                n++;
                c = line[n];
                sb.Append(ParserUtil.Unescape(c));
            }
            else if (c == '[' || c == '$')
            { //Read [lua code] or $stringify or ${stringify}
                int start = n;
                int end = start;

                char startChar = c;
                char endChar = ' ';
                if (startChar == '[')
                {
                    endChar = ']';
                }
                else if (startChar == '$' && start + 1 < line.Length && line[start + 1] == '{')
                {
                    start++;
                    endChar = '}';
                }

                if (sb.Length > 0)
                { //Flush buffered chars
                    string ln = AppendTextCommand(sb.ToString());
                    if (ln.Length > 0) result.Add(ln);
                    sb.Remove(0, sb.Length);
                }

                bool inQuotes = false;
                int brackets = (startChar == '[' ? 1 : 0);
                for (int x = n + 1; x < line.Length; x++)
                {
                    int d = line[x];
                    if (d == '\\')
                    {
                        x++;
                    }
                    else if (d == '\"')
                    {
                        inQuotes = !inQuotes;
                    }
                    else if (!inQuotes)
                    {
                        if (d == '[') brackets++;
                        else if (d == ']') brackets--;

                        if (brackets <= 0 && d == endChar)
                        {
                            end = x;
                            break;
                        }
                    }
                }

                if (end > start + 1)
                {
                    string str = line.Substring(start + 1, end);
                    if (startChar == '$')
                    {
                        result.Add(ParseStringifier(str));
                    }
                    else
                    {
                        result.Add(ParseCodeLine(str));
                    }
                }
                n = end;
            }
            else
            {
                sb.Append(c);
            }
        }

        if (sb.Length > 0)
        { //Flush buffered chars
            string ln = AppendTextCommand(sb.ToString());
            if (ln.Length > 0) result.Add(ln);
            sb.Remove(0, sb.Length);
        }
        result.Add(EndParagraphCommand());

        //Merge out lines into a String
        foreach (string outLine in result)
        {
            if (sb.Length > 0) sb.Append("; ");
            sb.Append(outLine);
        }
        return sb.ToString();
    }

    static string ParseStringifier(string str)
    {
        return string.Empty;
    }

    static string ParseCodeLine(string line)
    {
        return line.Trim();
    }

    static string BeginParagraphCommand(string filename, int textLineNum)
    {
        return string.Empty;
    }

    static string AppendTextCommand(string line)
    {
        if (line.Length == 0) return "";
        line = ParserUtil.CollapseWhitespace(ParserUtil.Escape(line), false);
        if (line.Length == 0) return "";
        return CreateLibraryCall("nvl", "say", '"' + line + '"');
    }

    static string CreateLibraryCall(string library, string methodName, string parameters)
    {
        return string.Format("{0}.{1}({2})", library, methodName, parameters);
    }

    static string EndParagraphCommand()
    {
        return string.Empty;
    }

    enum Mode
    {
        TEXT,
        CODE,
        MULTILINE_CODE,
        COMMENT,
        MULTILINE_COMMENT
    }

    static bool IsSingleLine(Mode mode)
    {
        return mode == Mode.CODE || mode == Mode.COMMENT;
    }
}
