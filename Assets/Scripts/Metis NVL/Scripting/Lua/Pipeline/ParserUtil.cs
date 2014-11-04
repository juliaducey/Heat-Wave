using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public static class ParserUtil
{
    private static char ZERO_WIDTH_SPACE = '\u8203';

    //Functions
    public static String ConcatLines(string[] lines)
    {
        StringBuilder sb = new StringBuilder();
        foreach (String line in lines)
        {
            sb.Append(line);
            sb.Append('\n');
        }
        return sb.ToString();
    }

    static readonly char[] escapeList = new char[]
    {
        'n', '\n', 'r', '\r', 't', '\t', 'f', '\f', '\"', '\"', '\'', '\'', '\\', '\\'
    };

    public static string Escape(string s)
    {
        if (s == null) return null;

        StringBuilder sb = new StringBuilder(s.Length);
        for (int n = 0; n < s.Length; n++)
        {
            char c = s[n];

            int t;
            for (t = 0; t < escapeList.Length; t += 2)
            {
                if (c == escapeList[t + 1])
                {
                    sb.Append('\\');
                    sb.Append(escapeList[t]);
                    break;
                }
            }
            if (t >= escapeList.Length)
            {
                sb.Append(c);
            }
        }
        return sb.ToString();
    }

    public static String Unescape(String s)
    {
        char[] chars = new List<char>(s).ToArray();

        int t = 0;
        for (int n = 0; n < chars.Length; n++)
        {
            if (chars[n] == '\\')
            {
                n++;
                chars[t] = Unescape(chars[n]);
            }
            else
            {
                chars[t] = chars[n];
            }
            t++;
        }
        return new String(chars, 0, t);
    }

    public static char Unescape(char c)
    {
        for (int n = 0; n < escapeList.Length; n += 2)
        {
            if (c == escapeList[n])
            {
                return escapeList[n + 1];
            }
        }
        return c;
    }

    private static bool IsCollapsibleSpace(char c)
    {
        return c == ' ' || c == '\t' || c == '\f' || c == ZERO_WIDTH_SPACE;
    }

    public static String CollapseWhitespace(String s, bool trim)
    {
        List<char> chars = new List<char>(s);

        int r = 0;
        int w = 0;
        while (r < chars.Count)
        {
            char c = chars[r++];

            if (IsCollapsibleSpace(c))
            {
                //Skip any future characters if they're whitespace
                while (r < chars.Count && IsCollapsibleSpace(chars[r]))
                {
                    r++;
                }

                if (w == 0 && trim)
                {
                    continue; //Starts with space
                }
                else if (r >= chars.Count && trim)
                {
                    continue; //Ends with space
                }
            }
            chars[w++] = c;
        }
        return new String(chars.ToArray(), 0, w);
    }

    public static String GetSrclocFilename(String srcloc)
    {
        return GetSrclocFilename(srcloc, "?");
    }

    public static String GetSrclocFilename(String srcloc, String defaultFilename)
    {
        if (srcloc == null)
        {
            return defaultFilename;
        }

        int index = srcloc.IndexOf(':');
        if (index >= 0)
        {
            srcloc = srcloc.Substring(0, index);
        }

        srcloc = srcloc.Trim();

        if (srcloc.Equals("?") || srcloc.Equals("???") || srcloc.Equals("undefined"))
        {
            return defaultFilename;
        }

        return srcloc;
    }

    public static int GetSrclocLine(String srcloc)
    {
        if (srcloc == null)
        {
            return -1;
        }

        int index = srcloc.IndexOf(':');

        if (index < 0)
        {
            return -1;
        }

        srcloc = srcloc.Substring(index + 1);
        return int.Parse(srcloc);
    }
}
