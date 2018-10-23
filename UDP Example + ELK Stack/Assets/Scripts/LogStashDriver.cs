using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LogStashDriver : MonoBehaviour
{
    private DataAnalytics da;
    readonly string firstTestMessage = "THIS IS A TEST. HOPEFULLY WE SEE THIS ON THE LOGSTASH FEED.";
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

        DataAnalyticsStructure.SetPostRequest(true);        // Mimic the send logs button from the User

        test();
    }

    private void Update()
    {
        Debug.Log("Connection Status: " + DataAnalyticsStructure.GetConnectedState());
    }

    private void test()
    {
        DataAnalyticsStructure.AddLog(firstTestMessage);
        DataAnalyticsStructure.AddLogList(multipleLogEntryExample);
        DataAnalyticsStructure.AddLog("I THINK IT WORKED?");

        DataAnalyticsStructure.PostLogs();
    }
}