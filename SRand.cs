/*
 * Auth: Ian
 * 
 * Proj: Rand
 * 
 * Desc: Static wrapper that encases a single instance of the Rand class
 * 
 * Date: 8/26/24
 */

using System;
using System.Collections.Generic;
using UnityEngine;

public static class SRand
{
    static uint GenerateStartupSeed()
    {
        // Get the current time in milliseconds
        uint timeSeed = (uint)DateTime.Now.Millisecond;

        // Combine with a more random value like the process ID and a small random number
        uint processSeed = (uint)System.Diagnostics.Process.GetCurrentProcess().Id;
        uint randomValue = (uint)new System.Random().Next(1, 1000);

        // Combine the seeds using bitwise operations to mix them
        return timeSeed ^ (processSeed << 16) ^ randomValue;
    }
    static readonly Rand i = new(GenerateStartupSeed());

    public static uint GetSeed() => i.GetSeed();
    public static void SetSeed(uint newSeed, uint newPos = 0) => i.SetSeed(newSeed, newPos);
    public static void SetPos(uint newPos) => i.SetPos(newPos);
    public static bool Chance(float successProbability) => i.Chance(successProbability);
    public static uint Random() => i.Random();
    public static float Range(float min, float max) => i.Range(min, max);
    public static float Range(Vector2 range) => i.Range(range);
    public static int Range(int min, int max) => i.Range(min, max);
    public static int Range(Vector2Int range) => i.Range(range);
    public static Vector2 Direction(Vector2 thetaRange) => i.Direction(thetaRange);
    public static Vector3 Direction(Vector2 thetaRange, Vector2 phiRange) => i.Direction(thetaRange, phiRange);
    public static T Element<T>(List<T> list) => i.Element(list);
    public static T Element<T>(T[] array) => i.Element(array);
    public static T Weighted<T>(Func<int, T> GetElement, Func<int, float> GetWeight, Func<int> GetCount) => i.Weighted(GetElement, GetWeight, GetCount);
    public static T Weighted<T>(List<Weighted<T>> weightedElements) => i.Weighted(weightedElements);
    public static T Weighted<T>(Weighted<T>[] weightedElements) => i.Weighted(weightedElements);
    public static T Weighted<T>(List<T> weightedElements) where T : IWeighted => i.Weighted(weightedElements);
    public static T Weighted<T>(T[] weightedElements) where T : IWeighted => i.Weighted(weightedElements);
}
