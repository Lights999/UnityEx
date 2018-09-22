using UnityEngine;
using System.Collections;
using System.Linq;
using System.Collections.Generic;

namespace PJMath
{
  public class ProbabilityHelper 
  {
    public static int CalculateIndex(int[] probabilityArray)
    {
      int _rangeMax = 0;

      foreach (var item in probabilityArray) 
      {
        _rangeMax += item;
      }

      int _result = Random.Range (0, _rangeMax);
      int _rangeStart = 0;
      int _rangeEnd = 0;

      for (int i = 0; i < probabilityArray.Length; i++) 
      {
        _rangeStart = _rangeEnd;
        _rangeEnd += probabilityArray [i];
        if (_result >= _rangeStart && _result <= _rangeEnd - 1) 
        {
          return i;
        }
      }

      return -1;
    }

    public static bool TrySuccess(int probability)
    {
      int _value = Random.Range (0, 100);
      return _value >= 0 && _value < probability;
    }
  }
}
