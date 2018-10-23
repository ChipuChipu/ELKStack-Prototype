using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
    Design Choices:
        (1) Use Coroutine to periodically check for stable connection via PING
            - Prevents the ability for the user to request postAllLogs (Connection check was initially at the postAllLogs function)
        (2) Coroutine checks run every 45 seconds.
        (3) Ping Checks will return a bool value when done.
            - Ping success depends on Ping.Time.
                - If Ping.Time == -1, a TimeOut event occured
*/

public class DataAnalytics : Singleton<DataAnalytics>
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
    private static readonly int pingIntervalCheckTime = 45;

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
    public static int GetSendPort() {return sendPort; }
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

    #region IEnumerator TestConnection
    public IEnumerator TestConnection()
    {
        Debug.Log("DA-testConnection");
        hasConnection = PingConnection();
        yield return new WaitForSeconds(pingIntervalCheckTime);
    }
    #endregion

    #region bool PingConnection
    /*
        This function will return a boolean value depending on the success of a ping to the server.
    */
    private bool PingConnection()
    {
        Debug.Log("DA-pingConnection() - Testing Connection");
        Ping newPing = new Ping("127.0.0.1");
        int maxTries = 10;

        while (!newPing.isDone || maxTries > 0)
        {
            maxTries--;
        }

        // A ping.Time of -1 is a response for Time Out
        if (newPing.time != -1)
            return true;
        else
            return false;
    }
    #endregion

    #region void AddLog
    /*
        This function will allow for a single log to be added onto the logList.
        This is an overloaded function that allows for an additional step to occur by the
        definitions set by the interface in DataAnalyticsInterface (Name Pending to Change)
    */
    public static void AddLog(string log)
    {
        if (LogList != null && log != null)
            LogList.Enqueue(log);
    }
    #endregion

    #region void AddLogList
    /*
        This function allows for a list of logs to be added onto the logList.
    */
    public static void AddLogList(List<string> list)
    {
        if (LogList != null && list != null)
        {
            foreach (string temp in list)
            {
                LogList.Enqueue(temp);
            }
        }
    }
    #endregion

    #region void PostAllLogs
    /*
        This function will send all logs within the logList while emptying the queue in the process.
        These logs are sent to the Logstash Client running on the server end.
    */
    public static void PostAllLogs()
    {
        if (postRequest && hasConnection && LogList != null && LogList.Count >= 1)
        {
            while (LogList.Count > 0 && Connection != null)
            {
                Connection.Send(LogList.Dequeue());
            }
        }
    }
    #endregion

    #region OnDestroy
    void OnDestroy()
    {
        _connection.Stop();
    }
    #endregion

}
