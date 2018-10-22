using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;    // For StreamWriter

public static class LogLoader
{
    public static void FetchLogs()
    {
        if(DataAnalytics.GetConnectedState())
        {
            string[] logFileList = LogStructure.GetAllFilePaths();

            if (logFileList != null || logFileList.Length != 0)
            {

            }
        }
    }

    public static void Log(string filePath, string log)
    {
        using (StreamWriter sw = new StreamWriter(filePath))
        {
            sw.WriteLine(log);
            sw.Close();
        }
    }
}
