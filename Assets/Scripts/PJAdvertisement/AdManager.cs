using UnityEngine;
using System.Collections;
//using UnityEngine.Advertisements;
using DataManagement;
using UnityEngine.UI;
using Common;
using GoogleMobileAds.Api;

namespace PJAdvertisememt 
{
  public class AdManager : SingletonObject<AdManager> 
  {
    [ReadOnly]
    public float TIME_OUT_INTERSTITIAL = 1.0f;
    [ReadOnly]
    public float TIME_OUT_REWARD_VIDEO = 5.0f;

    /*
    private static readonly int INIT_PLAYED_TIMES_BEFORE_AD_LIMIT = 10;
    private static readonly int PLAYED_TIMES_BEFORE_AD_LIMIT_MIN = 4;
    private static readonly int PLAYED_TIMES_BEFORE_AD_LIMIT_MAX = 6;
    */

    protected override void Awake()
    {
      this.admobManager = AdmobManager.Instance;
      base.Awake ();
    }

    void Start()
    {
      this.ShowBanner ();
    }

    public void ShowBanner()
    {
      StartCoroutine (ShowBannerCoroutine ());
    }

    public void ShowInterstitial(System.Action<string> callBack)
    {
      StartCoroutine (ShowInterstitialCoroutine (callBack));
    }

    /// <summary>
    /// Requests the reward video.
    /// </summary>
    /// <returns>The reward video.</returns>
    /// <param name="onLoaded">On loaded.</param>
    /// <param name="onTimeout">On timeout.</param>
    public IEnumerator RequestRewardVideoCoroutine(System.Action<string> onLoaded,  System.Action<string> onTimeout)
    {
      if(this.admobManager == null)
        this.admobManager = AdmobManager.Instance;

      yield return this.admobManager.RequestRewardVideoCoroutine (onLoaded, onTimeout, this.TIME_OUT_REWARD_VIDEO);
    }

    /// <summary>
    /// Shows the reward video.
    /// </summary>
    /// <param name="rewardCallback">Reward callback.</param>
    public void ShowRewardVideo(System.Action<Reward> rewardCallback)
    {
      if(this.admobManager == null)
        this.admobManager = AdmobManager.Instance;

      this.admobManager.ShowRewardVideo (rewardCallback);
    }

    /// <summary>
    /// Shows the banner coroutine.
    /// </summary>
    /// <returns>The banner coroutine.</returns>
    public IEnumerator ShowBannerCoroutine()
    {
      if(this.admobManager == null)
        this.admobManager = AdmobManager.Instance;

      yield return this.admobManager.ShowBannerCoroutine (AdPosition.Bottom);
    }

    /// <summary>
    /// Shows the interstitial coroutine.
    /// </summary>
    /// <returns>The interstitial coroutine.</returns>
    public IEnumerator ShowInterstitialCoroutine(System.Action<string> callBack)
    {
      if(this.admobManager == null)
        this.admobManager = AdmobManager.Instance;

      yield return this.admobManager.ShowInterstitialCoroutine (callBack,this.TIME_OUT_INTERSTITIAL);
    }

    /// <summary>
    /// Hides the banner.
    /// </summary>
    public void HideBanner()
    {
      if(this.admobManager == null)
        this.admobManager = AdmobManager.Instance;

      this.admobManager.HideBanner ();
    }

    /*
    public IEnumerator ShowAdCoroutine() 
    {
      if (CheckAdCanBeShown()) 
      {
        UserData.Instance.PlayedTimesBeforeAd = 0;
        UserData.Instance.PlayedTimesBeforeAdLimit = Random.Range (PLAYED_TIMES_BEFORE_AD_LIMIT_MIN,PLAYED_TIMES_BEFORE_AD_LIMIT_MAX + 1);
        UserData.Instance.Save ();
        yield return AdmobManager.Instance.ShowAdCoroutine (TIME_OUT);
      }
      yield break;
    }
  
    public void PlayedTimesCount() 
    {
      int _playedTimes = UserData.Instance.PlayedTimesBeforeAd;
      if (_playedTimes == UserData.DID_NOT_PLAY) 
      {
        _playedTimes = 0;
        UserData.Instance.PlayedTimesBeforeAdLimit = INIT_PLAYED_TIMES_BEFORE_AD_LIMIT + Random.Range (PLAYED_TIMES_BEFORE_AD_LIMIT_MIN,PLAYED_TIMES_BEFORE_AD_LIMIT_MAX + 1);
      }
      _playedTimes++;
      UserData.Instance.PlayedTimesBeforeAd = _playedTimes;
      Debug.Log ("PlayerTimesBeforeAd : " + UserData.Instance.PlayedTimesBeforeAd + " PlayedTimesBeforeAdLimit : " + UserData.Instance.PlayedTimesBeforeAdLimit);
      UserData.Instance.Save ();
    }
    public bool CheckAdCanBeShown()
    {
      return UserData.Instance.PlayedTimesBeforeAd >= UserData.Instance.PlayedTimesBeforeAdLimit;
    }
*/
    private AdmobManager admobManager;
  }
}
