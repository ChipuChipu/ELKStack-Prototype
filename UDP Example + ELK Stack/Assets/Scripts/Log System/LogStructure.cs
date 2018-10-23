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

    }
    #endregion

    #region Core Methods
    public static void GetLocalLogs()
    {
        LogList = LogLoader.GetLocalLogs(LogList);
    }

    public static void Log(string log)
    {
        // If there is no Connection, add it to the buffer
        if (!DataAnalytics.GetConnectedState())
        {
            LogList.Add(log);
            LogLoader.WriteLogLocal(filePath, log);
        }
        // If there is a Connection, immediately send the log
        else
            DataAnalytics.AddLog(log);
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
        // ON app stop, I have elemetns in and a connected state
        if (LogList.Count > 0 && DataAnalytics.GetConnectedState())
        {
            foreach(string temp in LogList)
            {
                DataAnalytics.AddLog(temp);
            }
        }
    }
    #endregion

    #region OnApplicationQuit()
    private void OnApplicationQuit()
    {
        
    }
    #endregion
}
