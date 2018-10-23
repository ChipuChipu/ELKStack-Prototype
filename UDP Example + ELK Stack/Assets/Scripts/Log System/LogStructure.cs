using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;

/*
    THINGS-TO-DO:
    (1) Flesh out LogLoader with a way to save a list as a file
    (2) Provide Checks to see when a valid connect exists, we send a log.
    (3) Provide additional checks when a valid connect exists, if there are any logs saved on local. We fetch, send, and delete.
*/

/*
*  Destructive Behavior: It is possible for the log system to delete the current (Today's Log File)
*
*  The Expected Behavior:
*      (1) Current or Today's File is deleted (Logs are digitally stored on temporary memory -> LogList)
*      (2) New Logs added the same day are added in a newly created "current" log file.
*/

public class LogStructure : Singleton<LogStructure>
{
    #region Singleton Initialization
    [RuntimeInitializeOnLoadMethodAttribute(RuntimeInitializeLoadType.AfterSceneLoad)]
    public static void InitializeStructure()
    {
        InitializeSingleton();
    }
    #endregion

    #region Initialization
    public static string fileDirectory = "C:\\Users\\Joshu\\Documents\\GitHub\\ELKStack-Prototype\\Log Dump";
    public static string filePath = fileDirectory + "\\" + "ErrorLog -- " + DateTime.UtcNow.Date + ".log";

    private List<string> _LogList;
    static List<string> LogList
    {
        get { return Instance._LogList; }
        set
        {
            Instance._LogList = value;
        }
    }

    private void Awake()
    {
        _LogList = new List<string>();

        // Should a check be added here to see if the log dump has an files inside?
        GetLocalLogs();
    }
    #endregion

    #region Core Methods
    public static void GetLocalLogs()
    {
        LogList = LogLoader.GetLocalLogs(LogList);
    }

    public static void Log(string log)
    {
        // If there is no Connection, add it to the List
        if (!DataAnalytics.GetConnectedState())
            LogList.Add(log);

        // If there is a Connection, immediately send the log
        else
            LogLoader.SendLog(LogList, log);
    }
    #endregion

    #region Helper Functions
    public static string[] GetAllFilePaths()
    {
        Directory.CreateDirectory(fileDirectory);
        return Directory.GetFiles(fileDirectory, "*.*");
    }
    #endregion

    #region Stop()
    private void Stop()
    {
        // if LogList is populated and we have a connection
        if (LogList.Count > 0 && DataAnalytics.GetConnectedState())
        {
            // For each element in LogList, send a request to DataAnalytics for Log Sending
            foreach (string temp in LogList)
            {
                DataAnalytics.AddLog(temp);
            }
        }

        // Else if LogList is populated and we do not have a connection
        else if (LogList.Count > 0 && DataAnalyticsStructure.GetConnectedState())
        {
            // For each element in LogList, send a request to LogLoader for Local Saving
            foreach (string temp in LogList)
            {
                LogLoader.WriteLogLocal(filePath, temp);
            }

            // Clear LogList
            LogList.Clear();
        }
    }
    #endregion

    #region OnApplicationQuit()
    private void OnApplicationQuit()
    {
        // if LogList is populated and we have a connection
        if (LogList.Count > 0 && DataAnalytics.GetConnectedState())
        {
            // For each element in LogList, send a request to DataAnalytics for Log Sending
            foreach (string temp in LogList)
            {
                DataAnalytics.AddLog(temp);
            }
        }

        // Else if LogList is populated and we do not have a connection
        else if (LogList.Count > 0 && DataAnalyticsStructure.GetConnectedState())
        {
            // For each element in LogList, send a request to LogLoader for Local Saving
            foreach (string temp in LogList)
            {
                LogLoader.WriteLogLocal(filePath, temp);
            }

            // Clear LogList
            LogList.Clear();
        }
    }
    #endregion
}
