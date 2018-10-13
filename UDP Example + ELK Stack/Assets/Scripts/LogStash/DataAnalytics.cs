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

public class DataAnalytics : Singleton<DataAnalytics>, DataAnalyticsInterface
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
    Queue<string> logList;
    bool hasConnection;
    bool postRequest;

    // UDP Variables
    UDPThreaded connection;
    string sendIp = "127.0.0.1";
    int sendPort = 8088;
    int receivePort = 10000;
    #endregion

    #region Constructor
    DataAnalytics()
    {
        logList = new Queue<string>();
        connection = new UDPThreaded();
        connection.StartConnection(sendIp, sendPort, receivePort);

        hasConnection = false;
        postRequest = false;
    }
    #endregion

    #region Start
    void Start()
    {
        StartCoroutine(testConnection());
    }
    #endregion

    #region IEnumerator testConnection
    IEnumerator testConnection()
    {
        hasConnection = pingConnection();
        yield return new WaitForSeconds(45);
    }
    #endregion

    #region bool pingConnection
    /*
        This function will return a boolean value depending on the success of a ping to the server.
    */
    bool pingConnection()
    {
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

    #region void addLog
    /*
        This function will allow for a single log to be added onto the logList.
        This is an overloaded function that allows for an additional step to occur by the
        definitions set by the interface in DataAnalyticsInterface (Name Pending to Change)
    */
    public void AddLog(string log)
    {
        if (logList != null && log != null)
            logList.Enqueue(log);
    }
    #endregion

    #region void addLogList
    /*
        This function allows for a list of logs to be added onto the logList.
    */
    public void addLogList(List<string> list)
    {
        if (logList != null && list != null)
        {
            foreach (string temp in list)
            {
                logList.Enqueue(temp);
            }
        }
    }
    #endregion

    #region void PostAllLogs
    /*
        This function will send all logs within the logList while emptying the queue in the process.
        These logs are sent to the Logstash Client running on the server end.
    */
    public void PostAllLogs()
    {
        if (postRequest && hasConnection)
        {
            while (logList.Count > 0 && connection != null)
            {
                connection.Send(logList.Dequeue());
            }
        }
    }
    #endregion
}
