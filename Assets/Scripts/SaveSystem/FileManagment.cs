using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public static class FileManagment
{
    // Specially Made for Player Data Saving
    public static bool WriteToSaveFile(string a_profileName, string a_fileContents)
    {
        var fullPath = Application.persistentDataPath + "/" + a_profileName + "_save.dat";
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
        var fullPath = Application.persistentDataPath + "/" + a_profileName + "_save.dat";
        try
        {
            result = File.ReadAllText(fullPath);
            return true;
        }
        catch (Exception e)
        {
            Debug.LogError($"Failed To read to {fullPath} with exception {e}");
            result = "";
            return false;
        }    
    }
}
