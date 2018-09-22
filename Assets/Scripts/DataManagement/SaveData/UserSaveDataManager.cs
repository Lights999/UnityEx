using UnityEngine;
using System.Collections;
using Common;
using DataManagement.SaveData.FormatCollection;

namespace DataManagement.SaveData
{
  public class UserSaveDataManager : Singleton<UserSaveDataManager> 
  {
    public static readonly string STRING_NONE = "NONE";

    public event System.Action<UserSaveDataBasicFormat> UserSaveDataBasicEvent = delegate(UserSaveDataBasicFormat obj) {};

    public bool DataExist
    {
      get
      { 
        return this.dataExist;
      }
    }

    public UserSaveDataFormat UserData
    {
      get
      { 
        return this.userData;
      }
      set
      {
        this.userData = value.CloneEx ();
        WriteToPlayerPrefs ();
      }
    }

    public UserSaveDataBasicFormat UserBasic
    {
      get
      { 
        return this.userData.UserSaveDataBasic;
      }
      set
      {
        if (!this.dataExist)
          InitUserData (true);
        
        this.userData.UserSaveDataBasic = value.CloneEx();
        this.UserSaveDataBasicEvent.Invoke (this.userData.UserSaveDataBasic);
        WriteToPlayerPrefs ();
      }
    }

    public UserSaveDataManager()
    {
      InitUserData ();
    }
      
    public void InitUserData(bool enableCreateEmpty = false)
    {
      this.userData = null;
      this.dataExist = false;

      // Check data file
      string _userJson = PlayerPrefs.GetString(KEY_USER, STRING_NONE);
      if (_userJson == STRING_NONE) 
      {
        if (enableCreateEmpty) 
        {
          this.userData = new UserSaveDataFormat ();
          WriteToPlayerPrefs ();
        } 
        else 
        {
          this.dataExist = false;
        }
        return;
      } 
        
      this.userData = JsonUtility.FromJson<UserSaveDataFormat> (_userJson);
      this.dataExist = true;
    }

    public void Save() 
    {
      // This method will be called by Unity automatically when application exiting
      PlayerPrefs.Save ();
    }

    public void Clear()
    {
      this.userData = null;//new UserSaveDataFormat ();
      PlayerPrefs.DeleteKey(KEY_USER);
      this.dataExist = false;
    }

    public void WriteToPlayerPrefs()
    {
      PlayerPrefs.SetString (KEY_USER, JsonUtility.ToJson (this.userData));
      this.dataExist = true;
    }

    #region PRIVATE_METHOD

    #endregion


    #region PRIVATE_MEMBER
    static readonly string KEY_USER = "KEY_USER";
    bool dataExist;
    UserSaveDataFormat userData;
    #endregion
  }
}
