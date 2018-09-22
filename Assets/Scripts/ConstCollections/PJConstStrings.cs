using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using ConstCollections.PJEnums;
using ConstCollections.PJEnums.Character;
using ConstCollections.PJEnums.Equipment;


namespace ConstCollections.PJConstStrings
{
  public struct PJGlobal
  {
    #region PUBLIC_STATIC_MEMBER

    public static readonly string PrevSceneName = "PrevSceneName"; 

    public static readonly Dictionary<SCENE_LIST, string> SceneNameList = InitSceneNameList();

    #endregion

    #region PPRIVATE_STATIC_METHOD

    static Dictionary<SCENE_LIST, string> InitSceneNameList()  
    {
      Dictionary<SCENE_LIST, string> _nameList = new Dictionary<SCENE_LIST, string> ();
      _nameList [SCENE_LIST.TITLE] = "title";
      _nameList [SCENE_LIST.CREATE_HERO] = "create_character";
      _nameList [SCENE_LIST.BATTLE] = "battle_main";
      return _nameList;
    }

    #endregion
  }

  public struct HangUpString
  {
    public const string DELTA_TIME_SECONDS = "DetalTimeMiliseconds";
    public const string REWARD = "HangUpReward";
    public const string MEMORY_SPACE = "HangUpString";
  }

  public struct NextJumpString
  {
    public static readonly string NEXT_HERO_SLOTID = "NextHeroSlotId";
    public static readonly string MEMORY_SPACE = "NextJumpString";

  }

  public struct SkillString
  {
    public static readonly string MEMORY_SPACE = "SkillString";
    public static readonly string SKILL_ADVANCE_CACHE = "SkillAdvanceCache";
    public static readonly string HERO_SLOT_ID = "HeroSlotID";
  }

  public struct EquipmentString
  {
    public static readonly string MEMORY_SPACE = "EquipmentString";
    public static readonly string HERO_SLOT_ID = "HeroSlotID";
    public static readonly string EQUIPMENT_BUILD_DATA = "EquipmentBuildData";
    public static readonly string EQUIPMENT_EXCHANGE_DATA = "EquipmentExchangeData";
    public static readonly string EQUIPMENT_DESTROY_ACQUIRE = "EquipmentDestroyAcquire";
    public static readonly string EQUIPMENT_DESTROY_CONFIRM = "EquipmentDestroyConfirm";
    public static readonly string POP_WINDOW_NEXT_TO = "PopWindowNextTo";
    public static readonly string EQUIPMENT_TYPE_STRING = "EquipmentType"; 
    public static readonly string EQUIPMENT_REINFORCE_DATA = "EquipmentReinforceData";  

    public static readonly Dictionary<EQUIPMENT_TYPE, STRINGS_LABEL> EquipmentTypeStringDic = InitEquipmentTypeString();

    public static readonly Dictionary<ATTRIBUTE_TYPE, STRINGS_LABEL> EquipmentAttributeStringDic = InitEquipmentAttributeString();

    public static readonly Dictionary<int, STRINGS_LABEL> EquipmentReinforceLevelStringDic = InitEquipmentReinforceLevelString();

    #region PRIVATE_STATIC_METHOD
    static Dictionary<ATTRIBUTE_TYPE, STRINGS_LABEL> InitEquipmentAttributeString()  
    {
      var _names = new Dictionary<ATTRIBUTE_TYPE, STRINGS_LABEL> ();
      _names [ATTRIBUTE_TYPE.HPMax] = STRINGS_LABEL.HERO_INFORMATION_LIFE;
      _names [ATTRIBUTE_TYPE.RES] = STRINGS_LABEL.HERO_INFORMATION_RESISTANCE;
      _names [ATTRIBUTE_TYPE.ATK] = STRINGS_LABEL.HERO_INFORMATION_ATK;
      _names [ATTRIBUTE_TYPE.STR] = STRINGS_LABEL.STRENGTH;
      _names [ATTRIBUTE_TYPE.MAG] = STRINGS_LABEL.HERO_INFORMATION_MAG;
      _names [ATTRIBUTE_TYPE.VIT] = STRINGS_LABEL.PHYSIQUE;
      _names [ATTRIBUTE_TYPE.DEF] = STRINGS_LABEL.HERO_INFORMATION_DEF;
      _names [ATTRIBUTE_TYPE.INT] = STRINGS_LABEL.SAGACITY;
      _names [ATTRIBUTE_TYPE.AC] = STRINGS_LABEL.HERO_INFORMATION_AC;
      _names [ATTRIBUTE_TYPE.DEX] = STRINGS_LABEL.DEXTERITY;
      _names [ATTRIBUTE_TYPE.CRI] = STRINGS_LABEL.HERO_INFORMATION_CRI;
      _names [ATTRIBUTE_TYPE.PEN] = STRINGS_LABEL.HERO_INFORMATION_PEN;
      _names [ATTRIBUTE_TYPE.HIT] = STRINGS_LABEL.HERO_INFORMATION_HIT;
      _names [ATTRIBUTE_TYPE.AVD] = STRINGS_LABEL.HERO_INFORMATION_AVOID;
      return _names;
    }

    static Dictionary<EQUIPMENT_TYPE, STRINGS_LABEL> InitEquipmentTypeString()  
    {
      var _names = new Dictionary<EQUIPMENT_TYPE, STRINGS_LABEL> ();
      _names [EQUIPMENT_TYPE.WEAPON] = STRINGS_LABEL.EQUIPMENT_TYPE_WEAPON;
      _names [EQUIPMENT_TYPE.ARMOR] = STRINGS_LABEL.EQUIPMENT_TYPE_ARMOR;
      _names [EQUIPMENT_TYPE.DECORATIONS] = STRINGS_LABEL.EQUIPMENT_TYPE_DECORATIONS;
      return _names;
    }

    static Dictionary<int, STRINGS_LABEL> InitEquipmentReinforceLevelString()  
    {
      var _names = new Dictionary<int, STRINGS_LABEL> ();
      _names [0] = STRINGS_LABEL.EQUIPMENT_REINFORCEMENT_LEVEL1_BUTTON;
      _names [1] = STRINGS_LABEL.EQUIPMENT_REINFORCEMENT_LEVEL2_BUTTON;
      _names [2] = STRINGS_LABEL.EQUIPMENT_REINFORCEMENT_LEVEL3_BUTTON;
      _names [3] = STRINGS_LABEL.EQUIPMENT_REINFORCEMENT_MIRACLE_BUTTON;
      return _names;
    }
    #endregion
  }

  public struct BattleString
  {
    public struct Character
    {
      public struct Animation
      {
        public static readonly string IDLE = "Idle";
        public static readonly string ATTACK = "Attack";
        public static readonly string GET_DAMAGE = "GetDamage";
        public static readonly string DEAD = "Dead";

        public static readonly Dictionary<ANIMATION_TRIGGERS, string> NextAnimationDic = InitNextAnimation();

        public static readonly Dictionary<ANIMATION_TRIGGERS, string> TriggerDic = InitTrigger();

        public static readonly Dictionary<string, ANIMATION_STATES> AnimationStateDic = InitAnimationState();

        #region PPRIVATE_STATIC_METHOD
        static Dictionary<ANIMATION_TRIGGERS, string> InitNextAnimation()  
        {
          var _names = new Dictionary<ANIMATION_TRIGGERS, string> ();
          _names [ANIMATION_TRIGGERS.GOTO_IDLE] = IDLE;
          _names [ANIMATION_TRIGGERS.GOTO_ATTACK] = ATTACK;
          _names [ANIMATION_TRIGGERS.GOTO_GET_DAMAGE] = GET_DAMAGE;
          _names [ANIMATION_TRIGGERS.GOTO_DEAD] = DEAD;
          return _names;
        }

        static Dictionary<ANIMATION_TRIGGERS, string> InitTrigger()  
        {
          var _triggers = new Dictionary<ANIMATION_TRIGGERS, string> ();
          _triggers [ANIMATION_TRIGGERS.GOTO_IDLE] = IDLE;
          _triggers [ANIMATION_TRIGGERS.GOTO_ATTACK] = ATTACK;
          _triggers [ANIMATION_TRIGGERS.GOTO_GET_DAMAGE] = GET_DAMAGE;
          _triggers [ANIMATION_TRIGGERS.GOTO_DEAD] = DEAD;
          return _triggers;
        }

        static Dictionary<string, ANIMATION_STATES> InitAnimationState()  
        {
          var _states = new Dictionary<string, ANIMATION_STATES> ();
          _states [IDLE] = ANIMATION_STATES.IDLE;
          _states [ATTACK] = ANIMATION_STATES.ATTACK;
          _states [GET_DAMAGE] = ANIMATION_STATES.GET_DAMAGE;
          _states [DEAD] = ANIMATION_STATES.DEAD;
          return _states;
        }
        #endregion
      }
    }

  }
}
