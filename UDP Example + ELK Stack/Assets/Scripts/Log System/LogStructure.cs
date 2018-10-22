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
    public static string filePath = fileDirectory + "\\" + "ErrorLog -- " + DateTime.UtcNow.Date + ".txt";

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
    public static void Log(string log)
    {
        LogList.Add(log);
        LogLoader.Log(filePath, log);

        // Should see if We have Connection. If so, send away. Else, stash it.
        // Should also see if the log dump has entities. If so, fetch and send.
        // Empty all files out of the Log Dump after sending.
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

    }
    #endregion

    #region OnApplicationQuit()
    private void OnApplicationQuit()
    {
        
    }
    #endregion
}
