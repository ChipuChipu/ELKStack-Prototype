using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataAnalyticsStructure : Singleton<DataAnalyticsStructure>
{
    #region Singleton Initialization
    [RuntimeInitializeOnLoadMethodAttribute(RuntimeInitializeLoadType.AfterSceneLoad)]
    public static void InitializeStructure()
    {
        InitializeSingleton();
    }
    #endregion

    #region General Initialization
    // Log Creation Variables
    private Queue<string> _logList;
    static private Queue<string> LogList
    {
        get { return Instance._logList; }
        set
        {
            Instance._logList = value;
        }
    }
    private static bool hasConnection;
    private static bool postRequest;

    // UDP Variables
    private UDPThreaded _connection;
    static private UDPThreaded Connection
    {
        get { return Instance._connection; }
        set
        {
            Instance._connection = value;
        }
    }
    private static string sendIp = "127.0.0.1";
    private static int sendPort = 9200;
    private static int receivePort = 10000;
    #endregion

    #region Awake()
    private void Awake()
    {
        _logList = new Queue<string>();
        _connection = new UDPThreaded();
        _connection.StartConnection(sendIp, sendPort, receivePort);

        hasConnection = false;
        postRequest = false;
    }
    #endregion

    #region Setters and Getters
    #region GetLogListCount
    public int GetLogListCount()
    {
        try
        {
            return _logList.Count;
        }

        catch
        {
            Debug.Log("LogList is null");
            return -1;
        }
    }
    #endregion

    #region GetIP
    public static string GetIP() { return sendIp; }
    #endregion

    #region GetSendPort
    public static int GetSendPort() { return sendPort; }
    #endregion

    #region GetReceivePort
    public static int GetReceivePort() { return receivePort; }
    #endregion

    #region GetConnectedState
    public static bool GetConnectedState() { return hasConnection; }
    #endregion

    #region GetPostRequestState
    public static bool GetPostRequestState() { return postRequest; }
    #endregion

    #region SetPostRequest
    public static void SetPostRequest(bool state) { postRequest = state; }
    #endregion
    #endregion

    #region Core Functions
    public static void AddLog(string log)
    {
        hasConnection = DataAnalyticsLoader.PingConnection();
        LogList = DataAnalyticsLoader.AddLog(LogList, log);
    }

    public static void AddLogList(List<string> list)
    {
        hasConnection = DataAnalyticsLoader.PingConnection();
        LogList = DataAnalyticsLoader.AddLogList(LogList, list);
    }

    public static void PostLogs()
    {
        hasConnection = DataAnalyticsLoader.PingConnection();
        LogList = DataAnalyticsLoader.PostAllLogs(Connection, LogList);
    }
    #endregion

    #region OnDestroy
    void OnDestroy()
    {
        Connection.Stop();
    }
    #endregion
}
