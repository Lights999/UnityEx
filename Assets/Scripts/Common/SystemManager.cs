using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using PJAudio;

namespace Common
{
  public class SystemManager : SingletonObject<SystemManager> 
  {
    public bool EnableBack = true;
    public AudioClip BackDownSE;

    #region UNITY_FUNCTION
    protected override void Awake()
    {
      base.Awake ();
      this.objStack = new Stack<Object> ();
      this.popEventsDic = new Dictionary<int, UnityAction> ();
      this.sceneNameStack = new Stack<string> ();
      this.sePlayer = FindObjectOfType<SEPlayer> ();
    }

    void Update()
    {
      if (!this.EnableBack)
        return;
      
      if (Input.GetKeyDown (KeyCode.Escape)) 
      {
        this.OnBackDown ();
      }
    }
    #endregion

    public void OnBackDown()
    {
      if (this.objStack.Count > 0) 
      {
        this.PopObject ();
        PlaySE ();
        return;
      }

      this.PopScene ();
      PlaySE ();
    }

    public void PushScene(string sceneName)
    {
      this.sceneNameStack.Push(sceneName);
    }

    public void PopScene()
    {
      if (this.sceneNameStack.Count <= 0) 
      {
        Debug.LogWarning ("[QUIT]");
        Application.Quit ();
        return;
      }

      string _sceneName = this.sceneNameStack.Pop();
      UnityEngine.SceneManagement.SceneManager.LoadScene (_sceneName);
    }

    public void PushObject(Object instance, UnityAction onPop = null)
    {
      this.objStack.Push(instance);
      if(onPop != null)
        this.popEventsDic.Add (instance.GetInstanceID (), onPop);
    }

    public void PopObject(int count = 1)
    {
      if (count > this.objStack.Count)
        throw new System.Exception (string.Format("count({0}) > this.objStack.Count({1})", count, this.objStack.Count));

      if (this.objStack.Count == 0) 
      {
        Debug.Log("objStack Is Clear");
        return;
      }

      while (count > 0) 
      {
        Object _obj = this.objStack.Pop ();
        int _instanceID = _obj.GetInstanceID ();

        DestroyImmediate(_obj);
        if (this.popEventsDic.ContainsKey (_instanceID)) 
        {
          this.popEventsDic [_instanceID].Invoke ();
          this.popEventsDic.Remove (_instanceID);
        }

        count--;
      }

      if (this.objStack.Count == 0)
        Debug.Log("objStack Is Clear");
    }

    public void ClearObject()
    {
      this.objStack.Clear();
    }

    public void PlaySE()
    {
      if (this.BackDownSE == null)
        return;

      this.sePlayer.Play (this.BackDownSE);
    }

    [SerializeField]
    Stack<Object> objStack;
    Dictionary<int, UnityAction> popEventsDic;
    Stack<string> sceneNameStack;
    SEPlayer sePlayer;
  }
}
