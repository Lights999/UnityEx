using UnityEngine;
using System.Collections;
using System;
using DataManagement;
using Common;
using DataManagement.SaveData;

namespace PJAudio
{
  public class BGMPlayer :  SingletonObject<BGMPlayer>
  {
    [ReadOnly]
    public float VolumeMax = 1.0F;
    [ReadOnly]
    public float VolumeMin = 0.0F;
    [Range(0.2F, 10.0F)]
    public float FadeInSeconds = 2.0F;
    [Range(0.2F, 10.0F)]
    public float FadeOutSeconds = 2.0F;
    public float FadeTimeMin = 0.2F;
    public float FadeTimeMax = 10.0F;

    public bool IsPlaying 
    {
      set{ }
      get 
      {
        return this.audioSource.isPlaying;
      }
    }

    public bool IsStopped 
    {
      set{ }
      get 
      {
        return !this.IsPlaying;
      }
    }

    public bool IsFinished
    {
      set{ }
      get 
      {
        if (this.audioSource.loop)
          return false;
        
        return !this.IsPlaying && Mathf.Approximately(this.audioSource.time, this.audioSource.clip.length);
      }
    }

    public float CurrentTimeSeconds 
    {
      set{ }
      get 
      {
        return this.audioSource.time;
      }
    }

    public bool IsLoopMode
    {
      set{ }
      get
      {
        return this.audioSource.loop;
      }
    }

    public float Length
    {
      set{ }
      get
      {
        return this.audioSource.clip.length;
      }
    }

    public float Volume
    {
      set{this.audioSource.volume = value;}
      get{return this.audioSource.volume;}
    }

    #region UNITY_FUNCTION
    // Use this for initialization(self)
    protected override void Awake()
    {
      audioSource = GetComponent<AudioSource> ();
      base.Awake ();
    }

    void OnEnable()
    {
      ConfigDataManager.Instance.BGMVolumeChangedEvents += OnVolumeChanged;
    }

    // Use this for initialization(work with others)
    void Start () 
    {
      this.VolumeMax = ConfigDataManager.Instance.BGMVolume;
    }

    void OnDisable()
    {
      ConfigDataManager.Instance.BGMVolumeChangedEvents -= OnVolumeChanged;
    }
    #endregion


    /// <summary>
    /// Plaies the initially.
    /// </summary>
    /// <param name="delaySeconds">Delay seconds.</param>
    public void PlayInitially(float? delaySeconds = null)
    {
      if(audioSource == null)
        audioSource = GetComponent<AudioSource> ();

      StopAllCoroutines();

      audioSource.Stop ();
      StartCoroutine (FadeInCoroutine ());
    }

    /// <summary>
    /// Stop this bgm.
    /// </summary>
    public void Stop()
    {
      StopAllCoroutines ();
      audioSource.Stop ();
    }

    /// <summary>
    /// Pause this instance.
    /// </summary>
    public void Pause()
    {
      audioSource.Pause ();
    }

    /// <summary>
    /// Resume this instance.
    /// </summary>
    public void Resume()
    {
      audioSource.UnPause ();
    }

    /// <summary>
    /// Fades out coroutine.
    /// </summary>
    /// <returns>The out coroutine.</returns>
    /// <param name="fadeOutSeconds">Fade out seconds.</param>
    public IEnumerator FadeOutCoroutine(float? fadeOutSeconds = null)
    {
      // Do not need to fade
      if (fadeOutSeconds == null || fadeOutSeconds.Value < this.FadeTimeMin) 
      {
        audioSource.Stop ();
        yield break;
      }

      // Need to fade
      yield return FadeVolume (fadeOutSeconds.Value, audioSource.volume, 0.0F);

      audioSource.Stop ();
      yield break;
    }

    /// <summary>
    /// Play with fade in coroutine.
    /// </summary>
    /// <returns>The fade in coroutine.</returns>
    /// <param name="fadeInSeconds">Fade in seconds.</param>
    /// <param name="delaySeconds">Delay seconds.</param>
    public IEnumerator FadeInCoroutine(float? fadeInSeconds = null, float? delaySeconds = null)
    {
      // Play postponed
      if (delaySeconds != null) 
      {
        yield return new WaitForSeconds (delaySeconds.Value);
      }

      // Play without fade time
      if (fadeInSeconds == null || fadeInSeconds.Value < this.FadeTimeMin) 
      {
        audioSource.volume = this.VolumeMax;
        audioSource.time = 0.0F;
        audioSource.Play ();
        yield break;
      }

      // Play with fade time
      audioSource.volume = 0.0F;
      audioSource.time = 0.0F;
      audioSource.Play ();

      yield return FadeVolume (fadeInSeconds.Value, audioSource.volume, this.VolumeMax);
      yield break;
    }

    public IEnumerator FadeOutInCoroutine(float? fadeOutSeconds = null, float? fadeInSeconds = null, float? delaySeconds = null)
    {
      yield return FadeOutCoroutine (fadeOutSeconds);
      this.audioSource.clip = this.nextAudioClip;
      this.nextAudioClip = null;
      yield return FadeInCoroutine (fadeInSeconds, delaySeconds);

    }

    #region PRIVATE_METHOD

    /// <summary>
    /// Fades the volume.
    /// </summary>
    /// <returns>The volume.</returns>
    /// <param name="fadeSeconds">Fade seconds.</param>
    /// <param name="volumeFrom">Volume from.</param>
    /// <param name="volumeTo">Volume to.</param>
    IEnumerator FadeVolume(float fadeSeconds, float? volumeFrom = null, float? volumeTo = null)
    {
      if (fadeSeconds < this.FadeTimeMin) 
      {
        yield break;
      }

      float? _volumeFrom = volumeFrom ?? this.VolumeMin;
      float? _volumeTo = volumeTo ?? this.VolumeMax;
      yield return LerpCoroutine (fadeSeconds, _normalizedTimeSeconds => 
        {
          audioSource.volume = Mathf.Lerp(_volumeFrom.Value, _volumeTo.Value, _normalizedTimeSeconds);
        });
          
      yield break;
    }

    // TODO: zhang-yang 
    // Set it to Common
    /// <summary>
    /// Lerps the coroutine.
    /// </summary>
    /// <returns>The coroutine.</returns>
    /// <param name="limitSeconds">Limit seconds.</param>
    /// <param name="onUpdate">On update.</param>
    IEnumerator LerpCoroutine(float limitSeconds, System.Action<float> onUpdate)
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

    void OnVolumeChanged(float value)
    {
      this.VolumeMax = value;
      this.Volume = value;
    }
    #endregion
      
    AudioSource audioSource;
    AudioClip nextAudioClip;
  }
}

