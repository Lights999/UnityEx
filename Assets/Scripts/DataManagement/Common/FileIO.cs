using UnityEngine;
using System.Collections.Generic;
using System.Xml;
using System.IO;
//using EnumCollections;
using Common;
using System.Text;
using DataManagement.TableClass;
using System.Collections;
//using StaticCollections;

namespace DataManagement.Common
{
  public class FileIO : Singleton<FileIO>
  {
    public bool EnableDeepDebug = false;

    public CSVReader<T> CSVReader<T>(string fileFullPath, AttributeIndexType attributeIndexType = AttributeIndexType.MIXED, bool skipFirstLine = true, Encoding encoding = null) where T: AbstractTable,new()
    {
      return new CSVReader<T> (fileFullPath, attributeIndexType, skipFirstLine, encoding);
    }

    public CSVReader<T> CSVReader<T>(TextAsset csv, AttributeIndexType attributeIndexType = AttributeIndexType.MIXED, bool skipFirstLine = true, Encoding encoding = null)  where T: AbstractTable,new()
    {
      return new CSVReader<T> (csv, attributeIndexType, skipFirstLine, encoding);
    }

    public AudioClip LoadAudio(string fileFullPath)
    {
      return AudioReader.Instance.LoadAudio(fileFullPath);
    }

    public ResourceRequest LoadAudioAsync(string fileFullPath)
    {
      return AudioReader.Instance.LoadAsyncAudio(fileFullPath);
    }

    public Sprite LoadMultiLanguageImage(string prefixPath, SystemLanguage lang = SystemLanguage.English)
    {
      return ImageReader.Instance.LoadMultiLanguage (prefixPath, lang);
    }
  }
}
