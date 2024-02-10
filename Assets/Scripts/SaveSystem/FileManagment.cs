using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.ConstrainedExecution;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;

public static class FileManagment
{
    private const string SaveFileExt = ".dat";
    // Specially Made for Player Data Saving
    public static bool WriteToSaveFile(string a_profileName, string a_fileContents)
    {
        var fullPath = Application.persistentDataPath + "/" + a_profileName + SaveFileExt;
        try
        {
            File.WriteAllText(fullPath, a_fileContents);
            return true;
        }
        catch (Exception e)
        {
            Debug.LogError($"Failed To write to {fullPath} with exception {e}");
        }

        return false;
    }

    public static bool LoadFromSaveFile(string a_profileName, out string result) 
    {
        var fullPath = Application.persistentDataPath + "/" + a_profileName + SaveFileExt;
        try
        {
            result = File.ReadAllText(fullPath);
            return true;
        }
        catch (Exception e)
        {
            //Debug.LogError($"Failed To read to {fullPath} with exception {e}");
            result = "";
            return false;
        }    
    }

    public static List<string> GetProfileFileNames()
    {
        var filesInDir = Directory.GetFiles(Application.persistentDataPath, "*" + SaveFileExt, SearchOption.TopDirectoryOnly);
        List<string> profiles = new List<string>();

        foreach (var fileInDir in filesInDir)
        {
            profiles.Add(Path.GetFileNameWithoutExtension(fileInDir));
        }

        return profiles;
    }

    /// <summary>
    /// Returns the contents of the profiles found in json
    /// </summary>
    /// <returns></returns>
    public static List<string> GetProfileContents()
    {
        var filesInDir = Directory.GetFiles(Application.persistentDataPath, "*" + SaveFileExt, SearchOption.TopDirectoryOnly);
        List<string> profilesContents = new List<string>();

        foreach (var fileInDir in filesInDir)
        {
            try
            {
                profilesContents.Add(File.ReadAllText(fileInDir));
            }
            catch (Exception e)
            {
                Debug.Log("Error with Getting All Profile Contents. Producing Error: " + e);
            }
        }

        return profilesContents;
    }
}
