using UnityEngine;
using System.Collections;
using UnityEngine.Events;
using DataManagement.SaveData;

namespace Common.UI
{
  public abstract class AbsMultiLanguageView : MonoBehaviour 
  {
    [ReadOnly]
    public SystemLanguage UserLanguage;

    protected virtual void Awake()
    {
      this.UserLanguage = ConfigDataManager.Instance.UserLanguage;
    }

    protected virtual void OnEnable()
    {
      ConfigDataManager.Instance.UserLanguageChangedEvents += LanguageChanged;
    }

    protected virtual void Start()
    {
      LanguageChanged ();
    }

    protected virtual void OnDisable()
    {
      ConfigDataManager.Instance.UserLanguageChangedEvents -= LanguageChanged;
    }

    protected void LanguageChanged()
    {
      this.UserLanguage = ConfigDataManager.Instance.UserLanguage;
      OnLanguageChanged ();
    }

    protected abstract void OnLanguageChanged();
  }
}
