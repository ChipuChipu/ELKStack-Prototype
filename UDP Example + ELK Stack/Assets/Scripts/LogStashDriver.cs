using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LogStashDriver : MonoBehaviour
{
    private DataAnalytics da;
    string firstTestMessage = "THIS IS A TEST. HOPEFULLY WE SEE THIS ON THE LOGSTASH FEED.";
    List<string> multipleLogEntryExample = new List<string>();

    void Start()
    { 
        multipleLogEntryExample.Add("This");
        multipleLogEntryExample.Add("serves");
        multipleLogEntryExample.Add("As");
        multipleLogEntryExample.Add("An");
        multipleLogEntryExample.Add("Example");
        multipleLogEntryExample.Add("Of");
        multipleLogEntryExample.Add("Batched");
        multipleLogEntryExample.Add("Log");
        multipleLogEntryExample.Add("Sending");

        da = new DataAnalytics();   
        da.setPostRequest(true);        // Mimic the send logs button from the User
        StartConnectionChecks();
    }

    private void Update()
    {
        if (da.getConnectedState())
        {
            Debug.Log("We are Connected");
            da.AddLog(firstTestMessage);
            da.addLogList(multipleLogEntryExample);
            da.AddLog("I THINK IT WORKED?");

            da.PostAllLogs();
        }
        else
            Debug.Log("We are not Connected");
    }

    #region void StartConnectChecks
    public void StartConnectionChecks()
    {
        StartCoroutine(da.testConnection());
        Debug.Log("Coroutine started...");
    }
    #endregion
}
