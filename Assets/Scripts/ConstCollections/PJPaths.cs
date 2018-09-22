using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using ConstCollections.PJEnums;
using ConstCollections.PJEnums.Character;

namespace ConstCollections.PJPaths
{
  public struct SEPath
  {
    #region PUBLIC_STATIC_MEMBER
    /*
     * Sample
    public static readonly string StartToPlay = "Audios/SE/GameStart/se_button_start.mp3";
    public static readonly string ChangeSong = "Audios/SE/GameStart/se_button_change.mp3";
    public static readonly string ChangeSpeed = "Audios/SE/GameStart/se_button_change.mp3";
    public static readonly string FailureGreen = "Audios/SE/GamePlay/WinLose/se_green_lose.mp3";
    public static readonly string FailureRed = "Audios/SE/GamePlay/WinLose/se_red_lose.mp3";
    */
    #endregion

  }

  public struct HeroInfoBG
  {
    
    public static readonly Dictionary<int, string> HeroInfoBGPathDic = InitHeroInfoBGPathDic();

    #region PPRIVATE_STATIC_METHOD
    static Dictionary<int, string> InitHeroInfoBGPathDic()  
    {
      Dictionary<int, string> _pathDic = new Dictionary<int, string> ();
      _pathDic [0] = "Textures/Test/HeroInfo/heroInfoBG_0";
      _pathDic [1] = "Textures/Test/HeroInfo/heroInfoBG_1";
      _pathDic [2] = "Textures/Test/HeroInfo/heroInfoBG_2";
      return _pathDic;
    }

    #endregion
  }

  public struct LanguageMark
  {
    public static readonly Dictionary<SystemLanguage, string> Languages = InitLanguages();

    #region PPRIVATE_STATIC_METHOD
    static Dictionary<SystemLanguage, string> InitLanguages()  
    {
      Dictionary<SystemLanguage, string> _langs = new Dictionary<SystemLanguage, string> ();
      _langs [SystemLanguage.English] = "en";
      _langs [SystemLanguage.Japanese] = "jp";
      _langs [SystemLanguage.Chinese] = "cns";
      _langs [SystemLanguage.ChineseSimplified] = "cns";
      _langs [SystemLanguage.ChineseTraditional] = "cnt";
      return _langs;
    }

    #endregion
  }

  public struct MultiLanguageImagePrefixPath
  {
    public static readonly string Root = "Textures/";
    public static readonly string TitleSpritePrefixPath = "Splash/logo_title";

    /*
     * Sample
    public static readonly string[] SongPrefixPaths = 
    {
      "GameStart/Message/message_music_0",
      "GameStart/Message/message_music_1",
      "GameStart/Message/message_music_2",
      "GameStart/Message/message_music_3"
    };
    */
  }

  public struct HeroNameCombination
  {
    public static readonly Dictionary<HERO_NAME_PART, string> TablePathDic = InitTablePathDic();

    #region PPRIVATE_STATIC_METHOD
    static Dictionary<HERO_NAME_PART, string> InitTablePathDic()  
    {
      Dictionary<HERO_NAME_PART, string> _pathDic = new Dictionary<HERO_NAME_PART, string> ();
      _pathDic [HERO_NAME_PART.PART_0] = "Data/Hero/hero_name_0.csv";
      _pathDic [HERO_NAME_PART.PART_1] = "Data/Hero/hero_name_1.csv";
      _pathDic [HERO_NAME_PART.PART_2] = "Data/Hero/hero_name_2.csv";
      return _pathDic;
    }

    #endregion
  }
}
