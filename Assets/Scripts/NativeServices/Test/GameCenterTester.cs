using UnityEngine;
using System.Collections;
using NativeServices;
using UnityEngine.UI;

public class GameCenterTester : MonoBehaviour {
  public Text Log;

  public void Login()
  {
    PlatformAccount.Instance.Login ();
  }

  public void ShowLeaderboard()
  {
    PlatformAccount.Instance.ShowLeaderboard ();
  }

  public void Report10()
  {
    PlatformAccount.Instance.ReportScore (10);
  }

  public void GetMyHighScore()
  {
    PlatformAccount.Instance.LoadRemoteHighScore (score => {
      this.Log.text = "Score : "+ score;
    });
  }
}
