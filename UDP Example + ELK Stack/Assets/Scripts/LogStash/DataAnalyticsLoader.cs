using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class DataAnalyticsLoader
{
    #region bool PingConnection
    /*
        This function will return a boolean value depending on the success of a ping to the server.
    */
    public static bool PingConnection()
    {
        Debug.Log("DAS-pingConnection() - Testing Connection");
        Ping newPing = new Ping("127.0.0.1");
        int maxTries = 5;

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
    public static Queue<string> AddLog(Queue<string> LogList, string log)
    {
        if (LogList != null && log != null)
            LogList.Enqueue(log);

        return LogList;
    }
    #endregion

    #region void AddLogList
    /*
        This function allows for a list of logs to be added onto the logList.
    */
    public static Queue<string> AddLogList(Queue<string> LogList, List<string> list)
    {
        if (LogList != null && list != null)
        {
            foreach (string temp in list)
            {
                LogList.Enqueue(temp);
            }
        }

        return LogList;
    }
    #endregion

    #region void PostAllLogs
    /*
        This function will send all logs within the logList while emptying the queue in the process.
        These logs are sent to the Logstash Client running on the server end.
    */
    public static Queue<string> PostAllLogs(UDPThreaded Connection, Queue<string> LogList)
    {
        if (DataAnalyticsStructure.GetPostRequestState() && DataAnalyticsStructure.GetConnectedState() && LogList != null && LogList.Count >= 1)
        {
            while (LogList.Count > 0 && Connection != null)
            {
                Connection.Send(LogList.Dequeue());
            }
        }

        return LogList;
    }
    #endregion
}
