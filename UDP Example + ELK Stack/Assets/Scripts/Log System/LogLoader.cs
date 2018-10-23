using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;    // For StreamWriter

public static class LogLoader
{

    #region GetLocalLogs()
    // This function tries to find all log files that were previously saved in the past and add onto the current loglist.
    public static List<string> GetLocalLogs(List<string> LogList)
    {
        List<string> localList = LogList;
        string[] logFileList = LogStructure.GetAllFilePaths();

        // Check if the Directory is empty
        if (logFileList.Length != 0)
        {
            // For each Log File Found
            foreach (string temp in logFileList)
            {
                string[] lines = File.ReadAllLines(temp);

                // For each Log Entry in a Log FileS
                foreach (string line in lines)
                {
                    localList.Add(line);
                }
                // Placeholder Logic for Deleting Scanned Log Files
                // Deleting Log Files are handled in the log sending process
            }
        }
        return localList;
    }
    #endregion

    #region WriteLogLocal(string, string)
    public static void WriteLogLocal(string filePath, string log)
    {
        using (StreamWriter sw = new StreamWriter(filePath))
        {
            sw.WriteLine(log);
            sw.Close();
        }
    }
    #endregion

    #region SendLog
    // In the event where connection is false, we save the log onto the list and send it back.
    public static List<string> SendLog(List<string> LogList, string log)
    {
        if (DataAnalytics.GetConnectedState())
            DataAnalytics.AddLog(log);

        else
            LogList.Add(log);

        return LogList;
    }
    #endregion


}
