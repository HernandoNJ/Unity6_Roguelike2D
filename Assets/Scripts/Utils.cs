using UnityEngine;

public static class Utils
{
    // Random including min value, excluding max value
    public static int GetNewRandomInt(int min, int max) => Random.Range(min, max);
}