using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class LogCounter
{
    private static int logCount = 0;

    public static int GetNextLogNumber()
    {
        logCount++;
        return logCount;
    }
}
