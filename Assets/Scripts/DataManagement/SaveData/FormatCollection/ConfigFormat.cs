using UnityEngine;
using System.Collections;

namespace DataManagement.SaveData.FormatCollection
{
  [System.Serializable]
  public class ConfigJsonFormat
  {
    public SystemLanguage UserLanguage;
    public float BGMVolume;
    public float SEVolume;

    public override string ToString ()
    {
      return JsonUtility.ToJson (this, true);
    }
  }
}
