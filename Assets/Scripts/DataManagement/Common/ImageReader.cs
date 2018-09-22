using UnityEngine;
using System.Collections;
using Common;
using System;
using ConstCollections.PJPaths;

namespace DataManagement.Common
{
  public class ImageReader : Singleton<ImageReader> 
  {
    public Sprite LoadMultiLanguage(string prefixPath, SystemLanguage lang)
    {
      string _langMark = null;
      if (!LanguageMark.Languages.ContainsKey (lang))
        _langMark = LanguageMark.Languages [SystemLanguage.English];
      else
        _langMark = LanguageMark.Languages [lang];

      string _fullPath = MultiLanguageImagePrefixPath.Root + prefixPath + "_" + _langMark;
      Debug.Log (_fullPath);
      return Resources.Load<Sprite>(_fullPath); 
    }
  }
}
