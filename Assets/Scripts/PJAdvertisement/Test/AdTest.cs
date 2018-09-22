using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using PJAdvertisememt;
using DataManagement;

namespace Advertisiment {

  public class AdTest : MonoBehaviour {

    public Text GamePlayedTimes;
    public Text PlayedTimesLimit;
    public Text TextLog;

    // Use this for initialization
    void Start () 
    {
      //playedtime = 0;
      //AdManager.Instance.Init ();
    }

    // Update is called once per frame
    void Update () 
    {
      /*
      GamePlayedTimes.text = "PLayed Times : " + UserData.Instance.PlayedTimesBeforeAd.ToString ();
      PlayedTimesLimit.text = "PLayed Times Limit :" + UserData.Instance.PlayedTimesBeforeAdLimit.ToString();
*/
    }

    //    public void ShowAd ()
    //    {
    //      StopAllCoroutines ();
    //      StartCoroutine (AdManager.Instance.ShowInterstitialCoroutine ());
    //    }

    public void SetLog(string text)
    {
      TextLog.text = text;
    }

    public void ShowBanner()
    {
      StopAllCoroutines ();
      //StartCoroutine (AdManager.Instance.ShowBannerCoroutine ());
    }

    public void ShowInterstitial()
    {
      StopAllCoroutines ();
      //StartCoroutine (AdManager.Instance.ShowInterstitialCoroutine ());
    }

    private AdManager adManager;

    private AdmobManager admobManager;

    [SerializeField]
    private Text timeText;
    //private int playedtime;

  }
}
