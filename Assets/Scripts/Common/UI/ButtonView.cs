using UnityEngine;
using System.Collections;
using PJAudio;
using UnityEngine.UI;
using UnityEngine.Events;

namespace Common.UI
{
  [RequireComponent(typeof(Button))]
  public class ButtonView : MonoBehaviour 
  {
    public bool AutoRemoveListener;
    public string GroupName;
    public Image ActivedImage;
    public bool EnableActivedImage;

    protected virtual void Awake()
    {
      this.AutoRemoveListener = true;
    }

    protected virtual void OnDisable()
    {
      if (this.AutoRemoveListener) 
      {
        this.RemoveAllOnClick ();
      }
    }

    // Use this for initialization
    protected virtual void Start () 
    {
      if (this.EnableActivedImage && this.ActivedImage == null) 
      {
        Image[] _images = this.transform.parent.GetComponentsInChildren<Image> ();
        Image _buttonImages = this.GetComponent<Image> ();
        foreach (var item in _images) 
        {
          if (item != _buttonImages)
            this.ActivedImage = item;
        }
      }

    }

    public void PlaySE(AudioClip clip)
    {
      if (!this.buttonScript.IsInteractable ())
        return;
      
      this.sePlayerScript.Play (clip);
    }

//    public void PlaySEs(AudioClip[] clip)
//    {
//    }

    public void AddOnClick(UnityAction action)
    {
      /* Backup
      #if UNITY_EDITOR
      UnityEditor.Events.UnityEventTools.AddPersistentListener(this.buttonScript.onClick, action);
      #else
      this.buttonScript.onClick.AddListener (action);
      #endif 
      */
      this.buttonScript.onClick.AddListenerWithEditor (action);
    }

    public void RemoveOnClick(UnityAction action)
    {
      this.buttonScript.onClick.RemoveListenerWithEditor (action);
    }

    public void RemoveAllOnClick()
    {
      this.buttonScript.onClick.RemoveALlListenerWithEditor();
    }

    public void ToggleActiveGameObject(GameObject gameObject)
    {
      gameObject.SetActive (!gameObject.activeSelf);
    }

    protected SEPlayer sePlayerScript
    {
      get{ 
        if(this.sePlayer == null)
          this.sePlayer = FindObjectOfType<SEPlayer> ();
        return this.sePlayer;
      }
    }

    protected SEPlayer sePlayer;

    protected Button buttonScript{
      get{ 
        if (this.button == null)
          this.button = GetComponent<Button> ();

        return this.button;
      }
    }

    Button button;
  }
}
