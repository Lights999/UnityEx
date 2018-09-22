using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using ConstCollections.PJEnums.Skill;

namespace ConstCollections.PJConstOthers
{
  public struct SlotID
  {
    public static short NONE = -1;
    public static short ID_0 = 0;
    public static short ID_1 = 1;
    public static short ID_2 = 2;
  }

  public struct SkillOthers
  {
    public static readonly int LEVEL_NONE = 0;
    public static readonly int LEVEL_MIN = 1;
    public static readonly int LEVEL_MAX = 10;
    public static readonly int SKILL_POINT_UP = 1;
  }

  public struct EquipmentOthers
  {
    public static readonly int TEN_THOUSAND = 10000;
    public static readonly int EQUIPMENT_REINFORCE_MAX_LEVEL = 10;
    public static readonly int EQUIPMENT_REINFORCE_ATTRIBUTE_BASE_LIST_MAX = 4; 
  }

}

namespace ConstCollections.PJConstOthers.Battle
{
  public struct FIGHT
  {
    public const int HIT_COUNT_DEFAULT = 1;
    public const int HIT_SUCCESS_PROBABILITY_MIN = 0;
    public const int HIT_SUCCESS_PROBABILITY_MAX = 100;
    public const int HIT_SUCCESS_CP_0 = 80;

    public const int BLAST_SUCCESS_AC_MINUS_PEN_MIN = 0;
    public const int BLAST_SUCCESS_AC_MINUS_PEN_MAX = 100;
    public const float BLAST_SUCCESS_CRI_CP_0 = 0.01F;
    public const int BLAST_SUCCESS_CP_0 = 100;
    public const float BLAST_POWER_NONE = 1.0F;
    public const float BLAST_POWER_NORMAL = 2.0F;

    public const int CALCULATE_DAMAGE_TO_OTHER_AC_MINUS_PEN_MIN = 0;
    public const int CALCULATE_DAMAGE_TO_OTHER_AC_MINUS_PEN_MAX = 100;
    public const float CALCULATE_DAMAGE_TO_OTHER_CP_0 = 1.0F;
    public const float CALCULATE_DAMAGE_TO_OTHER_CP_1 = 100.0F;
    public const float CALCULATE_DAMAGE_TO_OTHER_MUL_RANDOM_MIN = 0.9F;
    public const float CALCULATE_DAMAGE_TO_OTHER_MUL_RANDOM_MAX = 1.1F;
    public const float CALCULATE_DAMAGE_TO_OTHER_ATK_MIN_COE = 0.2F;

  }

  public struct REWARD_STRICTION
  {
    public const int DELTA_LEVEL_MAX = 5;
    public const float CP_0 = 1.0F;
    public const float CP_1 = 0.1F;
    public const int AMOUNT_MIN = 1;
  }
}
