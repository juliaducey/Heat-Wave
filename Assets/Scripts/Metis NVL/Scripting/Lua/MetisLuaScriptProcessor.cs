#if UNITY_EDITOR
using UnityEngine;
using System.Collections;
using UnityEditor;
using System.IO;

/// <summary>
/// Listens for imports and changes to files with a *.lvn extension and compiles them into a *.lua file
/// </summary>
public class MetisLuaScriptProcessor : AssetPostprocessor
{
    static readonly string METIS_SCRIPT_EXTENSION = "lvn";
    static readonly string METIS_SCRIPT_DIRECTORY = "Assets/Scripts/Metis NVL/Compiled";

    static void OnPostprocessAllAssets(
        string[] importedAssets,
        string[] deletedAssets,
        string[] movedAssets,
        string[] movedFromAssetPaths)
    {
        foreach (string str in importedAssets)
        {
            //Debug.Log("Reimported Asset: " + str);
            string[] splitStr = str.Split('/', '.');

            string folder = splitStr[splitStr.Length - 3];
            string fileName = splitStr[splitStr.Length - 2];
            string extension = splitStr[splitStr.Length - 1];
            //Debug.Log("File name: " + fileName);
            //Debug.Log("File type: " + extension);

            if (extension == METIS_SCRIPT_EXTENSION)
            {
                // Copy it to the compiled script directory
                string newDirectory = METIS_SCRIPT_DIRECTORY + "/" + folder;
                string compiledFilename = newDirectory + "/" + fileName + ".txt";

                System.IO.Directory.CreateDirectory(newDirectory);
                AssetDatabase.CopyAsset(str, compiledFilename);

                // Now compile it
                string lvnSourceCode = string.Empty;
                using (StreamReader streamReader = new StreamReader(compiledFilename))
                {
                    lvnSourceCode = streamReader.ReadToEnd();
                }
                string luaSourceCode = LvnScriptCompiler.Compile(lvnSourceCode);

                // Replace contents of copied file with compiled code
                //System.IO.File.WriteAllText(compiledFilename, luaSourceCode);
                using (StreamWriter streamWriter = new StreamWriter(compiledFilename))
                {
                    streamWriter.Write(luaSourceCode);
                }
            }
        }

        foreach (string str in deletedAssets)
        {
            //Debug.Log("Deleted Asset: " + str);
        }

        for (int i = 0; i < movedAssets.Length; i++)
        {
            //Debug.Log("Moved Asset: " + movedAssets[i] + " from: " + movedFromAssetPaths[i]);
        }
    }
}
#endif