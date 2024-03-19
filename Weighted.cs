using System.Collections.Generic;

[System.Serializable]
public class Weighted<ElementType>
{
    public ElementType element;
    public float weight;

    public Weighted(ElementType element, float weight)
    {
        this.element = element;
        this.weight = weight;
    }

    /// <summary>Returns index of weighted Element with same element. -1 if not found.</summary>
    public static int IndexOfElement(List<Weighted<ElementType>> Weighteds, ElementType element)
    {
        for( int i = 0; i < Weighteds.Count; i++)
        {
            if (Weighteds[i].element.Equals(element)) return i;
        }
        
        return -1;
    }
}
