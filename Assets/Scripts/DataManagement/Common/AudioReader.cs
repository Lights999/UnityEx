using UnityEngine;
using System.Collections;
using Common;
using System;

namespace DataManagement.Common
{
  public class AudioReader : Singleton<AudioReader> 
  {
    public static string fileExtention = ".mp3";

    public AudioClip LoadAudio(string fileFullPath)
    {
      if (!fileFullPath.EndsWith (fileExtention))
        throw new Exception("Extension is not "+fileExtention);

      string _filePath = fileFullPath.Remove (fileFullPath.Length - fileExtention.Length);

      AudioClip _audioClip = Resources.Load (_filePath) as AudioClip;
      if(_audioClip == null)
        throw new NullReferenceException(string.Format("{0} is not exits!",_filePath));

      return _audioClip;
    }

    public ResourceRequest LoadAsyncAudio(string fileFullPath)
    {
      if (!fileFullPath.EndsWith (fileExtention))
        throw new Exception("Extension is not "+fileExtention);

      string _filePath = fileFullPath.Remove (fileFullPath.Length - fileExtention.Length);

      return Resources.LoadAsync (_filePath);
    }
  }
}

