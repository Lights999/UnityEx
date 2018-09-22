using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Common;

namespace DataManagement
{

  /// <summary>
  /// Global data for debug.
  /// </summary>
  [System.Serializable]
  public class GlobalData<T> : System.IEquatable<GlobalData<T>>
  {
    public string Space;
    public string Name;
    public T Value;

    public GlobalData(string space, string name, T value)
    {
      this.Space = space;
      this.Name = name;
      this.Value = value;
    }

    #region IEquatable implementation

    public bool Equals (GlobalData<T> other)
    {
      if (this.Space != other.Space)
        return false;
      if (this.Name != other.Name)
        return false;
      if (!this.Value.Equals(other.Value))
        return false;

      return true;
    }

    #endregion
  }

  /// <summary>
  /// A cross multi-scenes global data manager.
  /// </summary>
  public class GlobalDataManager : SingletonObject<GlobalDataManager> 
  {
    #region UNITY_METHOD
    protected override void Awake()
    {
      spaceTable = spaceTable ?? new Hashtable ();
      this.CrossScene = true;
      base.Awake ();
    }
    #endregion

    public bool SetValue<T>(string name, T newValue, string memorySpace = "Global")
    {
      
      //System.Type _methodType = typeof(T);
      System.Type _methodUnderlyingType = System.Nullable.GetUnderlyingType (typeof(T));
//      System.Type _valueType = newValue.GetType ();
      if (_methodUnderlyingType != null) 
      {
        Debug.LogErrorFormat ("Not support Nullable type(type?)");
        return false;
      }

//      if (_methodType != _valueType) 
//      {
//        Debug.LogErrorFormat ("SetValue< {0} > != {1} newValue", typeof(T), newValue.GetType ());
//        return false;
//      }

      spaceTable = spaceTable ?? new Hashtable ();

      if (!spaceTable.ContainsKey (memorySpace)) 
      {
        spaceTable.Add (memorySpace, new Hashtable ());
      }

      Hashtable _dataTable = spaceTable [memorySpace] as Hashtable;

      if (_dataTable.ContainsKey (name)) 
      {
        Debug.LogFormat ("Name of [{0}] has existing in Memory Space of [{1}]" +
          "\n This will overwrite the origin value!", name, memorySpace);
        _dataTable [name] = newValue;

        if(this.EnableDeepDebug)
          D_SetValue();
        return true;
      }

      _dataTable.Add (name, newValue);

      if(this.EnableDeepDebug)
        D_SetValue();
      return true;
    }

    public T GetValue<T>(string name, string memorySpace = "Global") 
    {
      spaceTable = spaceTable ?? new Hashtable ();

      if (!spaceTable.ContainsKey (memorySpace)) 
      {
        string _error = string.Format ("Memory Space of [{0}] is not existing!", memorySpace);
        Debug.LogWarning (_error);
        T _value = default(T); 
        SetValue<T> (name, _value, memorySpace);
        return _value;
      }

      Hashtable _dataTable = spaceTable [memorySpace] as Hashtable;

      if (!_dataTable.ContainsKey (name)) 
      {
        string _error = string.Format ("Name of [{0}] is not existing in Memory Space of [{1}]!", name, memorySpace);
        Debug.LogWarning (_error);
        T _value = default(T); 
        SetValue<T> (name, _value, memorySpace);
        return _value;
      }

      return (T)_dataTable [name];
    }

    public T GetValueSafety<T>(string name, string memorySpace = "Global") 
    {
      spaceTable = spaceTable ?? new Hashtable ();

      if (!spaceTable.ContainsKey (memorySpace)) 
      {
        string _error = string.Format ("Memory Space of [{0}] is not existing!", memorySpace);
        throw new System.NullReferenceException(_error);
      }

      Hashtable _dataTable = spaceTable [memorySpace] as Hashtable;

      if (!_dataTable.ContainsKey (name)) 
      {
        string _error = string.Format ("Name of [{0}] is not existing in Memory Space of [{1}]!", name, memorySpace);
        throw new System.NullReferenceException(_error);
      }

      return (T)_dataTable [name];
    }

    public T? GetNullableValue<T>(string name, string memorySpace = "Global") where T : struct
    {
      spaceTable = spaceTable ?? new Hashtable ();

      if (!spaceTable.ContainsKey (memorySpace)) 
      {
        Debug.LogWarningFormat ("Memory Space of [{0}] is not existing!", memorySpace);
        return null;
      }

      Hashtable _dataTable = spaceTable [memorySpace] as Hashtable;

      if (!_dataTable.ContainsKey (name)) 
      {
        Debug.LogWarningFormat ("Name of [{0}] is not existing in Memory Space of [{1}]!", name, memorySpace);
        return null;
      }

      return (T?)_dataTable [name];
    }

    public bool RemoveValue(string name, string memorySpace = "Global")
    {
      if (spaceTable == null) 
      {
        Debug.LogWarning("spaceTable == null");
        return false;
      }

      if (!spaceTable.ContainsKey (memorySpace)) 
      {
        Debug.LogWarningFormat ("Memory Space of [{0}] is not existing!", memorySpace);
        return false;
      }

      Hashtable _dataTable = spaceTable [memorySpace] as Hashtable;

      if (!_dataTable.ContainsKey (name)) 
      {
        Debug.LogWarningFormat ("Name of [{0}] is not existing in Memory Space of [{1}]!", name, memorySpace);
        return false;
      }

      _dataTable.Remove (name);

      if (_dataTable.Count == 0) 
      {
        spaceTable.Remove (memorySpace);
      }

      if(this.EnableDeepDebug)
        D_SetValue();

      return true;
    }

    public void Clear()
    {
      if (this.spaceTable == null)
        return;
      
      this.spaceTable.Clear ();
    }

    #region DEBUG_METHOD
    void D_SetValue()
    {
      #if UNITY_EDITOR
      debug_view = debug_view ?? new List<string> ();

      debug_view.Clear ();
      foreach (string space in spaceTable.Keys) {
        Hashtable _datas = spaceTable [space] as Hashtable;
        foreach (string name in _datas.Keys) {
          object _obj = _datas[name];
          if(_obj == null)
            debug_view.Add (string.Format ("[{0}] - [{1}] - [{2}]", space, name, "null"));
          else
            debug_view.Add (string.Format ("[{0}] - [{1}] - [{2}]", space, name, _obj.ToString()));
        }
      }
      #endif
    }
    #endregion

    [ReadOnly]
    Hashtable spaceTable;

    [SerializeField][ReadOnly]
    List<string> debug_view;
  }
}

