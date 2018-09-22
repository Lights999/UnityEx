using UnityEngine;
using System.Collections;

namespace Common {
  /// <summary>
  /// http://wiki.unity3d.com/index.php/Singleton
  /// Be aware this will not prevent a non singleton constructor
  ///   such as `T myT = new T();`
  /// To prevent that, add `protected T () {}` to your singleton class.
  /// 
  /// As a note, this is made as MonoBehaviour because we need Coroutines.
  /// </summary>
  [DisallowMultipleComponent]
  public abstract class SingletonObject<T> : MonoBehaviour where T : MonoBehaviour
  {
    public bool EnableDeepDebug;
    public bool CrossScene;

    /// <summary>
    /// Gets the instance.
    /// </summary>
    /// <value>The instance.</value>
    public T Instance
    {
      get
      { 
        return SingletonObject<T>.GetInstance (this.CrossScene);
      }
    }

    public static T InstanceCrossSecene
    {
      get
      { 
        return SingletonObject<T>.GetInstance (true);
      }
    }
      
    #region UNITY_FUNCTION
    protected virtual void Awake()
    {
      if (instance == null)
      {
        SetInstanceOnAwake(this.CrossScene);
      }
      else if (instance != this)
      {
        Debug.LogWarningFormat("[Singleton] instance!= this [{0}]", this);
 //       DestroyImmediate(this.GetComponent(instance.GetType()));
        DestroyImmediateManual (this.gameObject);
      }
    }
    #endregion

    protected void SetInstanceOnAwake(bool crossScene = false)
    {
      instance = this as T;
      if (crossScene)
      {
        //TODO: zhang-yang
        // Check duplicated setting
        DontDestroyOnLoad(instance.transform.root.gameObject);
        Debug.Log(instance +"' was setuped with DontDestroyOnLoad.");
      }
    }

    protected static T GetInstance(bool crossScene = false)
    {
      if (crossScene && applicationIsQuitting) {
        Debug.LogWarning("[Singleton] Instance '"+ typeof(T) +
          "' already destroyed on application quit." +
          " Won't create again - returning null.");
        return null;
      }

      lock(locker) // for multithread's conflict
      {
        if (instance == null)
        {
          instance = (T)FindObjectOfType(typeof(T));

          if ( FindObjectsOfType(typeof(T)).Length > 1 )
          {
            Debug.LogError("[Singleton] Something went really wrong " +
              " - there should never be more than 1 singleton!" +
              " Reopening the scene might fix it.");
            return instance;
          }

          if (instance == null)
          {
            GameObject singleton = new GameObject();
            instance = singleton.AddComponent<T>();
            singleton.name = "(singleton) "+ typeof(T).ToString();
            Debug.Log("[Singleton] An instance of " + typeof(T) + 
              " is needed in the scene, so '" + singleton +
              "' was created.");

            if (crossScene == true)
            {
              DontDestroyOnLoad(singleton);
              Debug.Log(singleton +"' was setuped with DontDestroyOnLoad.");
            }

          } 
          else 
          {
            Debug.Log("[Singleton] Using instance already created: " +
              instance.gameObject.name);

            if (crossScene == true)
            {
              DontDestroyOnLoad(instance.gameObject);
              Debug.Log(instance.gameObject +"' was setuped with DontDestroyOnLoad.");
            }
          }
        }
      }
      return instance;
    }

    /// <summary>
    /// When Unity quits, it destroys objects in a random order.
    /// In principle, a Singleton is only destroyed when application quits.
    /// If any script calls Instance after it have been destroyed, 
    ///   it will create a buggy ghost object that will stay on the Editor scene
    ///   even after stopping playing the Application. Really bad!
    /// So, this was made to be sure we're not creating that buggy ghost object.
    /// </summary>
    protected virtual void OnDestroy () {
      if (destoriedBySystem) {
        applicationIsQuitting = true;
      }

      destoriedBySystem = true;
    }

    protected void DestroyImmediateManual(GameObject obj) 
    {
      destoriedBySystem = false;
      DestroyImmediate (obj);
    }

    protected static T instance;
    protected static object locker = new object();// for multithread's conflict
    protected static bool applicationIsQuitting = false;
    protected static bool destoriedBySystem = true;

  }
}
