using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DataManagement;
using ConstCollections.PJPaths;
using Common;
using DataManagement.SaveData;

namespace PJAudio
{
  public class SEPlayer : SingletonObject<SEPlayer> 
  {
    public float Volume
    {
      set{this.audioSource.volume = value;}
      get{return this.audioSource.volume;}
    }

    #region UNITY_FUNCTION
    // Use this for initialization(self)
    protected override void Awake()
    {
      this.audioSource = GetComponent<AudioSource> ();
      this.audioSource.loop = false;
      base.Awake ();
    }

    void OnEnable()
    {
      ConfigDataManager.Instance.SEVolumeChangedEvents += OnVolumeChanged;
    }

    // Use this for initialization(work with others)
    void Start () 
    {
      this.audioSource.volume = ConfigDataManager.Instance.SEVolume;
    }

    void OnDisable()
    {
      ConfigDataManager.Instance.SEVolumeChangedEvents -= OnVolumeChanged;
    }
    #endregion

    public void Play(string filePath)
    {
      this.audioSource.PlayOneShot(FindObjectOfType<AudioDataManager> ().LoadCachedSE(filePath));
    }

    public void Play(AudioClip clip)
    {
      this.audioSource.PlayOneShot(clip);
    }

    public void PlayRandomly(string[] filePathArray)
    {
      if (filePathArray == null)
        throw new System.NullReferenceException();

      if (filePathArray.Length == 0)
        throw new System.IndexOutOfRangeException ();

      Play (filePathArray [Random.Range (0, filePathArray.Length)]);
    }

    void OnVolumeChanged(float value)
    {
      this.Volume = value;
    }
    AudioSource audioSource;
  }
}
