using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using ConstCollections.PJEnums.Battle;
using ConstCollections.PJEnums.Character;
using Common;
using ConstCollections.PJEnums.Equipment;

namespace DataManagement.SaveData.FormatCollection
{
  // User.json
  [System.Serializable]
  public class UserSaveDataFormat
  {

    public UserSaveDataBasicFormat UserSaveDataBasic;

    public UserSaveDataFormat()
    {
      this.UserSaveDataBasic = new UserSaveDataBasicFormat ();
    }

    public override string ToString ()
    {
      return JsonUtility.ToJson (this, true);
    }
  }

  [System.Serializable]
  public class UserSaveDataBasicFormat
  {

    public UserSaveDataBasicFormat()
    {
    }

    public override string ToString ()
    {
      return JsonUtility.ToJson (this, true);
    }
  }
}
