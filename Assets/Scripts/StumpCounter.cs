using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class StumpCounter
{
    private static int stumpCount = 0;

    public static int GetNextStumpNumber()
    {
        stumpCount++;
        return stumpCount;
    }
}
