/*
 *  Author: Ian
 *
 *  Project: Leaf Rand
 *
 *  Date: 8/5/22  
 *  
 *  Various Noise and Random methods built on the back of Squirrel3 Noise.
 *  An instanced Random Library with a persistent random position.
 *  Allows for a series of deterministic random calls.
 */

using UnityEngine;
using System.Collections.Generic;
using System;

public class LeafRandInstanced
{
    private uint pos = 0;
    private uint seed = 0;


    public uint GetSeed() => seed;
    public void SetSeed(uint newSeed, uint newPos = 0) { seed = newSeed; pos = newPos; }
    public void SetPos(uint newPos) => pos = newPos;


    ///<returns>Random roll against P(probability).<br></br>Advances the  position.</returns>
    public bool Chance(float successProbability) => LeafRand.Chance(successProbability, pos, seed, SetPos);
    /// <summary>Returns a random uint.
    /// <br></br>Randomizes the  seed.</summary>
    public uint Random() => LeafRand.Random(pos, seed, SetPos);

    public float Range(float min, float max) => LeafRand.Range(min, max, pos, seed, SetPos);

    /// <summary>Returns a random float [range.x, range.y]. (Inclusive)</summary>
    public float Range(Vector2 range) => LeafRand.Range(range, pos, seed, SetPos);
    /// <summary>Returns a random integer [min, max]. (Inclusive)</summary>
    public int Range(int min, int max) => LeafRand.Range(min, max, pos, seed, SetPos);

    /// <summary>Returns a random integer [range.x, range.y]. (Inclusive)</summary>
    public int Range(Vector2Int range) => LeafRand.Range(range, pos, seed, SetPos);


    /// <returns>Returns a uniformly random item from items.</returns>
    public T Random<T>(List<T> items) => LeafRand.Random(items, pos, seed, SetPos);
    ///<summary>If no weights are passed in assumes uniform distribution. Otherweise lengths of Lists must be equal.</summary>
    ///<returns>Random item from item list based on weights.</returns>
    public T Weighted<T>(List<T> items, List<float> weights) => LeafRand.Weighted(items, weights, pos, seed, SetPos);

    /// <summary>Returns a random item from the list of WeightedElements based on each Weighted's weight.</summary>
    public T Weighted<T>(List<Weighted<T>> weightedElements) => LeafRand.Weighted(weightedElements, pos, seed, SetPos);
    /// <summary>Returns a random item from the array of WeightedElements based on each Weighted's weight.</summary>
    public T Weighted<T>(Weighted<T>[] weightedElements) => LeafRand.Weighted(weightedElements, pos, seed, SetPos);
    /// <summary>Returns a random item from the List of IWeighted based on each IWeighted's weight.</summary>
    public IWeighted Weighted(List<IWeighted> weightedElements) => LeafRand.Weighted(weightedElements, pos, seed, SetPos);
    /// <summary>Returns a random item from the Array of IWeighted based on each IWeighted's weight.</summary>
    public IWeighted Weighted(IWeighted[] weightedElements) => LeafRand.Weighted(weightedElements, pos, seed, SetPos);
}