using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using Common;
using System.Collections;

namespace DataManagement
{
  using Common;

  public class AudioDataManager : SingletonObject<AudioDataManager> 
  {
    [ReadOnly]
    public string DataPath = "Audios/";
    public int SECountMax = 10;

    #region UNITY_FUNCTION
    void Start()
    {
      this.seCache = new SECache (SECountMax);
    }
    #endregion

    public AudioClip LoadCachedSE(string filePath)
    {
      this.seCache = this.seCache ?? new SECache(SECountMax);
      return this.seCache.GetCachedClip (filePath) ?? this.seCache.Add(filePath).SEClip;
    }

    public AudioClip LoadCachedSE(AudioClip audioclip)
    {
      this.seCache = this.seCache ?? new SECache(SECountMax);
      return this.seCache.GetCachedClip (audioclip) ?? this.seCache.Add(audioclip).SEClip;
    }

    [SerializeField][ReadOnly]
    SECache seCache;
  }


  [System.Serializable]
  public class SECache
  {
    public SE[] SEClipsCache;
    public int NextIndex;

    public SECache(){}
    public SECache(int capacity)
    {
      SEClipsCache = new SE[capacity];
      NextIndex = 0;
    }

    public AudioClip GetCachedClip(string fileFullPath)
    {
      IEnumerable<SE> _cachedSE = this.SEClipsCache.Where (clip => 
        {
          if(clip == null)
            return false;

          return clip.fileFullPath == fileFullPath;
        });
      
      if (_cachedSE == null || _cachedSE.Count() == 0)
        return null;
      
      return _cachedSE.First().SEClip;
    }

    public AudioClip GetCachedClip(AudioClip audioclip)
    {
      IEnumerable<SE> _cachedSE = this.SEClipsCache.Where (clip => 
        {
          if(clip == null || clip.SEClip == null)
            return false;

          return clip.SEClip.GetHashCode() == audioclip.GetHashCode();
        });

      if (_cachedSE == null || _cachedSE.Count() == 0)
        return null;

      return _cachedSE.First().SEClip;
    }

    public bool IsCached(string fileFullPath)
    {
      return this.SEClipsCache.Any (clip => clip.fileFullPath == fileFullPath);
    }

    public bool IsCached(AudioClip audioclip)
    {
      return this.SEClipsCache.Any (clip => 
        {
          if(clip == null || clip.SEClip == null)
            return false;

          return clip.SEClip.GetHashCode() == audioclip.GetHashCode();
        });
    }

    public SE Add(string fileFullPath)
    {
      SE _se = null;
      _se = new SE(fileFullPath);
//      _se.SEClip.LoadAudioData ();

//      if ( this.SEClipsCache [NextIndex]!= null && this.SEClipsCache [NextIndex].SEClip != null)
//        this.SEClipsCache [NextIndex].SEClip.UnloadAudioData ();
      this.SEClipsCache [NextIndex] = _se;

      NextIndex = (NextIndex + 1) % SEClipsCache.Length;
      return _se;
    }

    public SE Add(AudioClip audioclip)
    {
      SE _se = null;
      _se = new SE(audioclip);
      this.SEClipsCache [NextIndex] = _se;

      NextIndex = (NextIndex + 1) % SEClipsCache.Length;
      return _se;
    }
  }

  [System.Serializable]
  public class SE
  {
    public string fileFullPath;
    public AudioClip SEClip;

    public SE(string fileFullPath)
    {
      this.fileFullPath = fileFullPath;
      this.SEClip = FileIO.Instance.LoadAudio (fileFullPath);
    }

    public SE(AudioClip audioclip)
    {
      this.fileFullPath = null;
      this.SEClip = audioclip;
    }
  }
}
