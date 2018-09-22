using UnityEngine;
using System.Collections;
using GoogleMobileAds.Api;
using Common;
using System.Collections.Generic;
using System;

namespace PJAdvertisememt 
{
  enum BANNER_STATE
  {
    NONE = 0,
    LOADED,
    SHOWN
  }

  public class AdmobManager : Singleton<AdmobManager> 
  {

    public void BannerRequestInit (AdPosition adPos) 
    {
      if (Debug.isDebugBuild)
        RequestBannerTest (adPos);
      else
        RequestBanner(adPos);
    }

    public void InterstitialRequestInit () 
    {
      if (Debug.isDebugBuild)
        RequestInterstitialTest ();
      else
        RequestInterstitial();
    }

    public void RewardVideoRequestInit () 
    {
      if (Debug.isDebugBuild)
        RequestRewardVideoTest ();
      else
        RequestRewardVideo();
    }

    public IEnumerator ShowBannerCoroutine(AdPosition adPos)
    {
      if(this.bannerState != BANNER_STATE.NONE)
        yield break;

      this.BannerRequestInit (adPos);

      yield return new WaitUntil (() => 
        {
          return this.bannerState == BANNER_STATE.LOADED;
        });

      this.bannerView.Show ();

      this.bannerState = BANNER_STATE.SHOWN;
      yield break;
    }

    public void HideBanner()
    {
      if (this.bannerView != null)
        this.bannerView.Hide ();
    }

    public IEnumerator ShowInterstitialCoroutine(System.Action<string> callBack,float _timeoutSeconds = 5.0f)
    {
      this.InterstitialRequestInit ();
      float _startTime = Time.realtimeSinceStartup;
      bool _isLoaded = false;
      bool _isTimeout = false;
      yield return new WaitUntil (() => 
        {
          _isLoaded = this.interstitial.IsLoaded();
          _isTimeout = (Time.realtimeSinceStartup - _startTime) > _timeoutSeconds;
          return _isLoaded || _isTimeout;
        });

      if (_isLoaded) 
      {
        callBack ("Ad loaded");
        this.interstitial.Show ();
      } 
      else 
      {
        callBack ("Ad loading time out");
      } 
      yield break;
    }

    public IEnumerator RequestRewardVideoCoroutine(System.Action<string> onLoaded,  System.Action<string> onTimeout, float _timeoutSeconds = 5.0f)
    {
      this.RewardVideoRequestInit ();

      float _startTime = Time.realtimeSinceStartup;
      bool _isLoaded = false;
      bool _isTimeout = false;

      yield return new WaitUntil (() => 
        {
          _isLoaded = this.rewardBasedVideo.IsLoaded();
          _isTimeout = (Time.realtimeSinceStartup - _startTime) > _timeoutSeconds;
          return _isLoaded || _isTimeout;
        });

      if (_isLoaded)
        onLoaded ("rewardBasedVideo _isLoaded");
      else 
        onTimeout ("Reward video loading time out");

      yield break;
    }

    public bool ShowRewardVideo(System.Action<Reward> rewardCallback)
    {
      if (this.rewardBasedVideo == null || !this.rewardBasedVideo.IsLoaded ())
        return false;

      if (this.onRewardEventList == null)
        this.onRewardEventList = new List<EventHandler<Reward>> ();

      foreach (var item in this.onRewardEventList) 
      {
        this.rewardBasedVideo.OnAdRewarded -= item;
      }
      this.onRewardEventList.Clear ();

      System.EventHandler<Reward> _rewardCallback = (object sender, Reward e) => 
      {
        rewardCallback(e);
      };

      this.rewardBasedVideo.OnAdRewarded += _rewardCallback;
      this.onRewardEventList.Add (_rewardCallback);

      this.rewardBasedVideo.Show ();
      return true;
    }

    public bool RequestShowRewardVideo(System.Action<Reward> rewardCallback,  System.Action<string> timeoutCallback, float _timeoutSeconds = 5.0f)
    {
      this.RewardVideoRequestInit ();

      if (this.onRewardEventList == null)
        this.onRewardEventList = new List<EventHandler<Reward>> ();

      foreach (var item in this.onRewardEventList) 
      {
        this.rewardBasedVideo.OnAdRewarded -= item;
      }
      this.onRewardEventList.Clear ();

      System.EventHandler<Reward> _rewardCallback = (object sender, Reward e) => 
      {
        rewardCallback(e);
      };

      this.rewardBasedVideo.OnAdRewarded += _rewardCallback;
      this.onRewardEventList.Add (_rewardCallback);

      float _startTime = Time.realtimeSinceStartup;
      bool _isLoaded = false;
      bool _isTimeout = false;

      _isLoaded = this.rewardBasedVideo.IsLoaded();
      _isTimeout = (Time.realtimeSinceStartup - _startTime) > _timeoutSeconds;

      while (true) 
      {
        _isLoaded = this.rewardBasedVideo.IsLoaded();
        _isTimeout = (Time.realtimeSinceStartup - _startTime) > _timeoutSeconds;

        if (_isLoaded || _isTimeout)
          break;
      }

      if (_isLoaded) {
        Debug.Log ("rewardBasedVideo _isLoaded");
        this.rewardBasedVideo.Show ();
        return true;
      } 
      else 
      {
        timeoutCallback ("Reward video loading time out");
        return false;
      }
    }

    #region PRIVATE_METHOD
    void RequestBannerTest(AdPosition adPos) 
    {
      #if UNITY_ANDROID
      string adUnitId = BANNER_ID_ANDROID;
      #elif UNITY_IPHONE
      string adUnitId = BANNER_ID_IOS;
      #else
      string adUnitId = "unexpected_platform";
      #endif

      // Initialize an InterstitialAd.
      this.bannerView = new BannerView (adUnitId, AdSize.Banner, adPos);
      // Create an empty ad request.
      this.bannerRequest = new AdRequest.Builder ()
        .AddTestDevice (AdRequest.TestDeviceSimulator)
        .AddTestDevice ("13A77A28519F232228753CF051D11C51")
        .Build ();
      // Load the interstitial with the request.
      this.bannerView.LoadAd (this.bannerRequest);
      this.bannerView.OnAdLoaded += this.BannerAdLoaded;
    }

    void RequestInterstitialTest() 
    {
      #if UNITY_ANDROID
      string adUnitId = INTERSTITIAL_ID_ANDROID;
      #elif UNITY_IPHONE
      string adUnitId = INTERSTITIAL_ID_IOS;
      #else
      string adUnitId = "unexpected_platform";
      #endif

      // Initialize an InterstitialAd.
      if(this.interstitial == null)
        this.interstitial = new InterstitialAd (adUnitId);

      // Create an empty ad request.
      this.interstitialRequest = new AdRequest.Builder ()
        .AddTestDevice (AdRequest.TestDeviceSimulator)
        .AddTestDevice ("13A77A28519F232228753CF051D11C51")
        .Build ();

      // Load the interstitial with the request.
      this.interstitial.LoadAd (this.interstitialRequest);
    }

    void RequestRewardVideoTest()
    {
      #if UNITY_ANDROID
      this.rewardVideoID = AD_REWARD_ID_ANDROID;
      #elif UNITY_IPHONE
      this.rewardVideoID = AD_REWARD_ID_IOS;
      #else
      this.rewardVideoID = "unexpected_platform";
      #endif

      if (this.rewardBasedVideo == null) {
        this.rewardBasedVideo = RewardBasedVideoAd.Instance;
        this.rewardBasedVideo.OnAdRewarded += (object sender, Reward e) => {
          Debug.Log("OnAdRewarded");
        };
      }

      this.rewardVideoRequest = new AdRequest.Builder()
        .AddTestDevice (AdRequest.TestDeviceSimulator)
        .AddTestDevice ("13A77A28519F232228753CF051D11C51")
        .Build ();

      // Load with the request.
      this.rewardBasedVideo.LoadAd(this.rewardVideoRequest, this.rewardVideoID);
    }

    void RequestBanner(AdPosition adPos) 
    {
      #if UNITY_ANDROID
      string adUnitId = BANNER_ID_ANDROID;
      #elif UNITY_IPHONE
      string adUnitId = BANNER_ID_IOS;
      #else
      string adUnitId = "unexpected_platform";
      #endif

      // Initialize an InterstitialAd.
      this.bannerView = new BannerView (adUnitId, AdSize.Banner, adPos);
      // Create an empty ad request.
      this.bannerRequest = new AdRequest.Builder ().Build ();
      // Load the interstitial with the request.
      this.bannerView.LoadAd (this.bannerRequest);
      this.bannerView.OnAdLoaded += this.BannerAdLoaded;
    }

    void RequestInterstitial() 
    {
      #if UNITY_ANDROID
      string adUnitId = INTERSTITIAL_ID_ANDROID;
      #elif UNITY_IPHONE
      string adUnitId = INTERSTITIAL_ID_IOS;
      #else
      string adUnitId = "unexpected_platform";
      #endif

      // Initialize an InterstitialAd.
      if(this.interstitial == null)
        this.interstitial = new InterstitialAd (adUnitId);

      // Create an empty ad request.
      this.interstitialRequest = new AdRequest.Builder ().Build ();
      // Load the interstitial with the request.
      this.interstitial.LoadAd (this.interstitialRequest);
    }

    void BannerAdLoaded(object sender, EventArgs args)
    {
      this.bannerState = BANNER_STATE.LOADED;
      Debug.Log ("BannerAdLoaded");
    }

    void RequestRewardVideo()
    {
      #if UNITY_ANDROID
      this.rewardVideoID = AD_REWARD_ID_ANDROID;
      #elif UNITY_IPHONE
      this.rewardVideoID = AD_REWARD_ID_IOS;
      #else
      this.rewardVideoID = "unexpected_platform";
      #endif

      if (this.rewardBasedVideo == null) {
        this.rewardBasedVideo = RewardBasedVideoAd.Instance;
        this.rewardBasedVideo.OnAdRewarded += (object sender, Reward e) => {
          Debug.Log("OnAdRewarded");
        };
      }

      this.rewardVideoRequest = new AdRequest.Builder().Build ();

      // Load with the request.
      this.rewardBasedVideo.LoadAd(this.rewardVideoRequest, this.rewardVideoID);
    }

    #endregion

    // FIXME: yang-zhang ID
    #if UNITY_ANDROID
    static readonly string BANNER_ID_ANDROID = "ca-app-pub-7901627817663442/7366416414";
    static readonly string INTERSTITIAL_ID_ANDROID = "ca-app-pub-7901627817663442/4897427216";
    static readonly string AD_REWARD_ID_ANDROID = "ca-app-pub-7901627817663442/5889683211";
    #elif UNITY_IPHONE
    static readonly string BANNER_ID_IOS = "ca-app-pub-7901627817663442/7506017213";
    static readonly string INTERSTITIAL_ID_IOS = "ca-app-pub-7901627817663442/4107607612";
    static readonly string AD_REWARD_ID_IOS = "ca-app-pub-7901627817663442/1319882817";
    #endif

    BannerView bannerView;
    InterstitialAd  interstitial;
    RewardBasedVideoAd rewardBasedVideo;

    AdRequest bannerRequest;
    AdRequest interstitialRequest;
    AdRequest rewardVideoRequest;

    bool bannerIsLoaded;
    BANNER_STATE bannerState = BANNER_STATE.NONE;

    string rewardVideoID;
    List<EventHandler<Reward>> onRewardEventList;
  }
}
