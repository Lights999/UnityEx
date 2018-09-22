using UnityEngine;
using System.Collections;
using Common;
using UnityEngine.SceneManagement;
using UnityEngine.Events;
using PJAudio;
using ConstCollections.PJEnums;
using UnityEngine.Rendering;

namespace Splash
{
  public class SplashManager : SingletonObject<SplashManager> {
    public string MainSceneName = "main";
    public UnityEvent OnUnitySplashFinishedEvents;

    IEnumerator Start()
    {
      // Add listener
      if(OnUnitySplashFinishedEvents.GetPersistentEventCount() == 0)
      {
        #if UNITY_EDITOR
        UnityEditor.Events.UnityEventTools.AddPersistentListener(OnUnitySplashFinishedEvents, FindObjectOfType<Splash.UI.SplashView>().ShowCompany);
        #else
        OnUnitySplashFinishedEvents.AddListener (FindObjectOfType<Splash.UI.SplashView>().ShowCompany);
        #endif
      }

      FindObjectOfType<BGMPlayer> ().Stop ();

      #if DEBUG || DEVELOPMENT_BUILD
      float _startTime = Time.realtimeSinceStartup;
      // WaitWhile isShowingSplashScreen
      yield return new WaitWhile(() => 
        {
          Debug.Log("SplashScreen.isFinished == " + SplashScreen.isFinished);
          float _time = Time.realtimeSinceStartup - _startTime;
          if(SplashScreen.isFinished)
            FindObjectOfType<DataManagement.GlobalDataManager>().SetValue<float>("SplashTime", _time);
          Debug.Log("time = " + _time);
          return !SplashScreen.isFinished && _time < 4.5F;
        });
      #else
      yield return new WaitWhile(() => 
        {
          Debug.Log("isShowingSplashScreen == " + Application.isShowingSplashScreen);
          return Application.isShowingSplashScreen ;
        });
      #endif

      // Unity SplashScreen finished 
      OnUnitySplashFinishedEvents.Invoke ();
      yield break;
    }

    public void StartMainScene()
    {
      FindObjectOfType<SystemManager> ().PushScene (SceneManager.GetActiveScene ().name);
      SceneManager.LoadScene (MainSceneName);
    }

    public void EnableScreenButton(GameObject viewObject)
    {
      viewObject.SetActive (true);
    }

    public void PlayBGM()
    {
      //FindObjectOfType<BGMPlayer> ().LoadPlayFadeIn (GAME_MODE.VS, SPEED_MODE.NORMAL, 0);
    }
  }
}
