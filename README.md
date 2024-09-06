### Motivation
- Convenient Random methods (Chance, Range(Taking a vector), Direction, Element, WeightedElement)
- No need to reimplement these methods, a Weighted interface, or generic type every time I need it
- Consistency between static and instanced random class (Unity uses UnityEngine.Random for static calls and System.Random for instanced)
- Consistent Random calls across different environments (Portability)
