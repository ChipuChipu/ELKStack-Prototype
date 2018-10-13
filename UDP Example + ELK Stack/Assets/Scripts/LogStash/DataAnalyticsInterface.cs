using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface DataAnalyticsInterface
{
    void AddLog(string log);
    void PostAllLogs();
}