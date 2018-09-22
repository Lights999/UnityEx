using UnityEngine;
using System.Collections;
using ConstCollections.PJPaths;
using Common;
using DataManagement.Common;
//using DataManagement.GameData.FormatCollection;
using DataManagement.SaveData.FormatCollection;
#if UNITY_IOS
using System.Runtime.InteropServices;
#endif

namespace DataManagement.SaveData
{
  public class ConfigDataManager : Singleton<ConfigDataManager> 
  {
    public static readonly string STRING_EMPTY = "EMPTY";
    public static readonly float VOLUME_MAX = 1.0F;

    public event System.Action UserLanguageChangedEvents = delegate {};
    public event System.Action<float> BGMVolumeChangedEvents = delegate(float obj) {};
    public event System.Action<float> SEVolumeChangedEvents = delegate(float obj) {};

    public bool DataExsit
    {
      get{ return this.dataExsit;}
    }

    public SystemLanguage UserLanguage
    {
      get 
      {
        if (!this.dataExsit)
          InitConfigData ();
        
        return this.configData.UserLanguage;
      }
      set 
      {
        if (!this.dataExsit)
          InitConfigData ();
        
        SystemLanguage _lang = value;

        // Unsupported Language
        if(!LanguageMark.Languages.ContainsKey (_lang))
        {
          Debug.LogError(value + " is not supported language");
          _lang = SystemLanguage.English;
        }

        // Check current language
        if (this.configData.UserLanguage == _lang)
          return;

        this.configData.UserLanguage = _lang;

        PlayerPrefs.SetString(KEY_CONFIG, JsonUtility.ToJson(this.configData));

        UserLanguageChangedEvents.Invoke ();
      }
    }

    public float BGMVolume
    {
      get 
      {
        if (!this.dataExsit)
          InitConfigData ();
        
        return this.configData.BGMVolume;
      }
      set 
      {
        if (!this.dataExsit)
          InitConfigData ();
        
        this.configData.BGMVolume = value;
        PlayerPrefs.SetString(KEY_CONFIG, JsonUtility.ToJson(this.configData));
        BGMVolumeChangedEvents (value);
      }
    }

    public float SEVolume
    {
      get 
      {
        if (!this.dataExsit)
          InitConfigData ();
        
        return this.configData.SEVolume;
      }
      set 
      {
        if (!this.dataExsit)
          InitConfigData ();
        
        this.configData.SEVolume = value;
        PlayerPrefs.SetString(KEY_CONFIG, JsonUtility.ToJson(this.configData));
        SEVolumeChangedEvents (value);
      }
    }
      
    public ConfigDataManager()
    {
      InitConfigData ();
    }

    public void InitConfigData()
    {
      this.configData = new ConfigJsonFormat();

      // Check data file
      string _configJson = PlayerPrefs.GetString(KEY_CONFIG, STRING_EMPTY);
      if (_configJson == STRING_EMPTY) 
      {
        this.configData = CreateDefaultConfigData();
        PlayerPrefs.SetString (KEY_CONFIG, JsonUtility.ToJson (this.configData));
        this.dataExsit = true;
        return;
      } 

      this.configData = JsonUtility.FromJson<ConfigJsonFormat> (_configJson);
      this.dataExsit = true;
    }

    public void Save() 
    {
      // This method will be called by Unity automatically when application exiting
      PlayerPrefs.Save ();
    }

    public void Clear()
    {
      this.configData = new ConfigJsonFormat();
      this.dataExsit = false;
      PlayerPrefs.DeleteKey(KEY_CONFIG);
    }

    #region PRIVATE_METHOD

    ConfigJsonFormat CreateDefaultConfigData()
    {
      ConfigJsonFormat _data = new ConfigJsonFormat ();
      _data.BGMVolume = VOLUME_MAX;
      _data.SEVolume = VOLUME_MAX;
      _data.UserLanguage = CreateLang ();
      return _data;
    }

    SystemLanguage CreateLang()
    {
      SystemLanguage _lang = SystemLanguage.Unknown;

      Debug.LogError("platform = " + Application.platform.ToString());

      #if UNITY_IOS
      string name = CurIOSLang();  

      // Check ChineseSimplified / ChineseTraditional
      if (name.StartsWith("zh-Hans") || name == "zh") 
      {  
      // ChineseSimplified
      _lang = SystemLanguage.ChineseSimplified;
      return _lang;
      }  
      else if(name.StartsWith("zh-Hant") || name == "zh-HK" || name == "zh-TW")
      {
      // ChineseTraditional
      _lang = SystemLanguage.ChineseTraditional;
      return _lang;
      }
      #endif
      //Sometime it will get "Chinese" 
      //but the button's language label have only English,Japanese,ChineseSimplified and ChineseTraditional
      //change Chinese into ChineseSimplified so the button will be selected
      _lang = Application.systemLanguage;
      if (_lang == SystemLanguage.Chinese) 
      {
        _lang = SystemLanguage.ChineseSimplified;
      }

      // Unsupported Language
      if(!LanguageMark.Languages.ContainsKey (_lang))
      {
        _lang = SystemLanguage.English;
      }

      //PlayerPrefs.SetInt (KEY_USER_LANGUAGE, (int)_lang);
      return _lang;
    }

    #if UNITY_IOS
    /** 
    general
    Simplified              zh, zh-Hans, zh-Hans-CN 
    Traditional             zh-Hant, zh-Hant-TW, zh-Hant-HK, zh-HK, zh-TW 

    ios 7 
    Simplified              zh-Hans 
    Traditional             zh-Hant 

    ios 8.1 
    Simplified                zh-Hans             ChineseSimplified 
    Traditional(HongKong)     zh-HK               ChineseTraditional 
    Traditional(TaiWan)       zh-Hant             ChineseTraditional 

    ios 9.1 
    Simplified                zh-Hans-CN          Chinese 
    Traditional(HongKong)     zh-HK               ChineseTraditional 
    Traditional(TaiWan)       zh-TW               Chinese 
    **/   
    [DllImport("__Internal")]  
    static extern string CurIOSLang(); 
    #endif

    #endregion


    #region PRIVATE_MEMBER
    static readonly string KEY_CONFIG = "KEY_CONFIG";

    /*
    static readonly string KEY_PLAYED_TIMES_BEFORE_AD = "KEY_PLAYED_TIMES_BEFORE_AD";
    static readonly string KEY_PLAYED_TIMES_BEFORE_AD_LIMIT = "KEY_PLAYED_TIMES_BEFORE_AD_LIMIT";
    static readonly string KEY_USER_LANGUAGE = "KEY_USER_LANGUAGE";
    static readonly string KEY_BGM_VOLUME = "KEY_BGM_VOLUME";
    static readonly string KEY_SE_VOLUME = "KEY_SE_VOLUME";
    */

    ConfigJsonFormat configData;
    bool dataExsit;
    #endregion

  }

}
