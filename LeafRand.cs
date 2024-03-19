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

public class LeafRand
{
    /// <summary>
    /// Global Leaf Rand Instance
    /// </summary>
    public  LeafRand I = new LeafRand(); 
    private uint pos = 0;
    private uint seed = 0;


    public uint GetSeed() => seed;
    public void SetSeed(uint newSeed, uint newPos = 0) { seed = newSeed; pos = newPos; }
    public void SetPos(uint newPos) => pos = newPos;


    #region Random
    ///<summary>
    ///Random roll against P(probability).
    ///<br></br>
    ///Advances the random position.
    ///</summary>
    public bool Chance(float successProbability)
    {
        //Edge case if chance is 0 & 0 is rolled should return false
        if (successProbability == 0) return false;

        return (float)Random() / uint.MaxValue <= successProbability;
    }
    #region Random Number
    /// <summary>
    /// Returns a random uint using the seed and pos.
    /// <br></br> 
    /// Advances the random position.
    /// </summary>
    public  uint Random()
    {
        pos = Noise(pos, seed);

        return pos;
    }
    /// <summary>
    /// Returns a random float [min, max]. (Inclusive)
    /// <br></br>
    /// Advances the random position.
    /// </summary>
    public  float Range(float min, float max) => Remap(Random(), min, max);
    /// <summary>
    /// Returns a random float [range.x, range.y]. (Inclusive)
    /// <br></br>
    /// Advances the random position.
    /// </summary>
    public float Range(Vector2 range) => Range(range.x, range.y);
    /// <summary>
    /// Returns a random integer [min, max]. (Inclusive)
    /// <br></br>
    /// Advances the random position.
    /// </summary>
    public int Range(int min, int max) => Remap(Random(), min, max);
    /// <summary>
    /// Returns a random integer [range.x, range.y]. (Inclusive)
    /// <br></br>
    /// Advances the random position.
    /// </summary>
    public int Range(Vector2Int range) => Range(range.x, range.y);


    #endregion
    #region Random Element
    /// <returns>
    /// Returns a uniformly random item from items.
    /// <br></br>
    /// Advances the random position.
    /// </returns>
    public T Random<T>(List<T> items) => items[Range(0, items.Count - 1)];
    ///<summary>
    ///If no weights are passed in assumes uniform distribution. 
    ///Otherweise lengths of Lists must be equal.
    /// <br></br>
    /// Advances the random position.
    ///</summary>
    public T Weighted<T>(List<T> items, List<float> weights)
    {
        if (weights.Count == 0) return Random(items);

        //Gets Total Weight
        float totalWeight = 0;
        foreach (float w in weights)
            totalWeight += w;

        //Get rand
        float rand = Range(0f, totalWeight);
        float weightIndex = 0;
        for (int i = 0; i < items.Count; i++)
        {
            weightIndex += weights[i];
            if (weightIndex > rand)
                return items[i];
        }

        throw new System.Exception($"LeafNoise: rand weight larger than totalweight! {items[0]}");
    }
    /// <summary>
    /// Returns a random item from the list of WeightedElements based on each Weighted's weight.
    /// <br></br>
    /// Advances the random position.
    /// </summary>
    public T Weighted<T>(List<Weighted<T>> weightedElements)
    {
        List<float> weights = new();
        List<T> elements = new();

        foreach (Weighted<T> weightedElement in weightedElements)
        {
            weights.Add(weightedElement.weight);
            elements.Add(weightedElement.element);
        }

        return Weighted(elements, weights);
    }
    /// <summary>
    /// Returns a random item from the array of WeightedElements based on each Weighted's weight.
    /// <br></br>
    /// Advances the random position.
    /// </summary>
    public T Weighted<T>(Weighted<T>[] weightedElements)
    {
        List<float> weights = new();
        List<T> elements = new();

        foreach (Weighted<T> weightedElement in weightedElements)
        {
            weights.Add(weightedElement.weight);
            elements.Add(weightedElement.element);
        }

        return Weighted(elements, weights);
    }
    /// <summary>
    /// Returns a random item from the List of IWeighted based on each IWeighted's weight.
    /// <br></br>
    /// Advances the random position.
    /// </summary>
    public IWeighted Weighted(List<IWeighted> weightedElements)
    {
        List<float> weights = new();

        foreach (IWeighted weightedElement in weightedElements)
        {
            weights.Add(weightedElement.GetWeight());
        }

        return Weighted(weightedElements, weights);
    }
    /// <summary>
    /// Returns a random item from the Array of IWeighted based on each IWeighted's weight.
    /// <br></br>
    /// Advances the random position.
    /// </summary>
    public IWeighted Weighted(IWeighted[] weightedElements)
    {
        List<float> weights = new();
        List<IWeighted> weightedElementList = new();

        foreach (IWeighted weightedElement in weightedElements)
        {
            weightedElementList.Add(weightedElement);
            weights.Add(weightedElement.GetWeight());
        }

        return Weighted(weightedElementList, weights);
    }
    #endregion
    #endregion
    #region Noise
    #region Squirrel3 Functions
    /// <summary>Returns 1D Noise given a position and a seed</summary>
    public static uint Noise(int position, uint seed)
    {   //Squirrel3 1D Noise Function
        uint bigP1 = 0x68E31DA4;
        uint bigP2 = 0xB5297A4D;
        uint bigP3 = 0x1B56C4E9;

        uint mangled = (uint)position;
        mangled *= bigP1;
        mangled += seed;
        mangled ^= (mangled >> 8);
        mangled += bigP2;
        mangled ^= (mangled << 8);
        mangled *= bigP3;
        mangled ^= (mangled >> 8);
        return mangled;
    }
    /// <summary>Returns 2D Noise given a position and a seed</summary>
    public static uint Noise(int x, int y, uint seed) => Noise(x + y * 198491317, seed);
    /// <summary>Returns 3D Noise given a position and a seed</summary>
    public static uint Noise(int x, int y, int z, uint seed) => Noise(x + y * 198491317 + z * 6542989, seed);
    #endregion
    /// <summary>Returns a linear interpolation between 1D noise values.</summary>
    public static uint Noise(float position, uint seed)
    {
        uint floorNoise = Noise(Floor(position), seed);
        uint ceilNoise = Noise(Ceil(position), seed);

        return Lerp(floorNoise, ceilNoise, GetRawDecimal(position));
    }
    /// <summary>Returns a linear interpolation between 2D noise values.</summary>
    public static uint Noise(float x, float y, uint seed)
    {
        //Get Interpolations between x with floored y and x with ceiling y
        //uint x1toX2YFloorLerped = Noise(Noise(x + Mathf.Floor(y * 198491317), seed) + x % 1, seed);
        //uint x1toX2YCeilLerped  = Noise(Noise(x + Mathf.Ceil(y * 198491317), seed) + x % 1, seed);

        uint floorNoise = Noise(Floor(x), Floor(y), seed);
        uint ceilNoise = Noise(Ceil(x), Floor(y), seed);
        uint x1toX2YFloorLerped = Lerp(floorNoise, ceilNoise, GetRawDecimal(x));


        floorNoise = Noise(Floor(x), Ceil(y), seed);
        ceilNoise = Noise(Ceil(x), Ceil(y), seed);
        uint x1toX2YCeilLerped = Lerp(floorNoise, ceilNoise, GetRawDecimal(x));

        //Return Interpolation between those two values
        return Lerp(x1toX2YFloorLerped, x1toX2YCeilLerped, GetRawDecimal(y));
    }
    public static uint Noise(float x, float y, float z, uint seed)
    {
        uint floorNoise = Noise(Floor(x), Floor(y), Floor(z), seed);
        uint ceilNoise = Noise(Ceil(x), Floor(y), Floor(z), seed);
        uint valAtYFloor = Lerp(floorNoise, ceilNoise, GetRawDecimal(x));

        floorNoise = Noise(Floor(x), Ceil(y), Floor(z), seed);
        ceilNoise = Noise(Ceil(x), Ceil(y), Floor(z), seed);
        uint valAtYCeil = Lerp(floorNoise, ceilNoise, GetRawDecimal(x));

        uint valAtZFloor = Lerp(valAtYFloor, valAtYCeil, GetRawDecimal(y));


        floorNoise = Noise(Floor(x), Floor(y), Ceil(z), seed);
        ceilNoise = Noise(Ceil(x), Floor(y), Ceil(z), seed);
        valAtYFloor = Lerp(floorNoise, ceilNoise, GetRawDecimal(x));

        floorNoise = Noise(Floor(x), Ceil(y), Ceil(z), seed);
        ceilNoise = Noise(Ceil(x), Ceil(y), Ceil(z), seed);
        valAtYCeil = Lerp(floorNoise, ceilNoise, GetRawDecimal(x));

        uint valAtZCeil = Lerp(valAtYFloor, valAtYCeil, GetRawDecimal(y));


        return Lerp(valAtZFloor, valAtZCeil, GetRawDecimal(z));
    }
    #endregion
    #region Util
    /// <summary> 
    /// Remaps uint to a value in [range.x, range.y]. (Inclusive)
    /// </summary>
    public static float Remap(uint noise, float min, float max) => min + (float)noise / uint.MaxValue * (max - min);
    /// <summary> 
    /// Remaps uint to a value in [range.x, range.y]. (Inclusive)
    /// </summary>
    public static int Remap(uint noise, int min, int max)
    {
        if (noise == uint.MaxValue) return max;
        else return Floor(min + (float)noise / uint.MaxValue * (max + 1 - min));
    }

    /// <summary> 
    /// uint Lerp 
    /// </summary>
    static uint Lerp(uint from, uint to, float lerpPercent) => (uint)((1 - lerpPercent) * from + lerpPercent * to);
    /// <summary>
    /// Rounds up relative to the absolute value of num.
    /// <br></br>
    /// * -1.4 -> -2
    /// <br></br>
    /// * 1.4  ->  2
    /// </summary>
    static int Ceil(float num) => (num % 1 == 0) ? (int) num : (int) (num + Mathf.Sign(num));
    /// <summary>
    /// Rounds down relative to the absolute value of num.
    /// <br></br>
    /// * -1.4 -> -1
    /// <br></br>
    /// * 1.4  ->  1
    /// </summary>
    static int Floor(float num) => (int)num;
    static float GetRawDecimal(float num) => Mathf.Abs(num % 1);
    #endregion
}
