using UnityEngine;
using System.Collections;
using Common;

namespace PJAudio
{
  public class AudioManager : SingletonObject<AudioManager> {

    // Use this for initialization
    void Start () {

    }

    // Update is called once per frame
    void Update () {

    }

    public void Stop()
    {
      this.GetComponentInChildren<BGMPlayer> ().Stop ();
    }
  }
}

