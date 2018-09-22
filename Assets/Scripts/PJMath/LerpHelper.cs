using UnityEngine;
using System.Collections;

namespace PJMath
{
  public class LerpHelper 
  {
    /// <summary>
    /// Lerps the coroutine.
    /// </summary>
    /// <returns>The coroutine.</returns>
    /// <param name="limitSeconds">Limit seconds.</param>
    /// <param name="onUpdate">On update.</param>
    public static IEnumerator LerpCoroutine(float limitSeconds, System.Action<float> onUpdate)
    {
      float _time = 0;

      while(_time < limitSeconds)
      {
        _time += Time.deltaTime;
        onUpdate.Invoke(_time / limitSeconds);
        yield return null;
      }
      yield break;
    }
  }
}
