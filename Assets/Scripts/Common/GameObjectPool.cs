using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Common
{
  public class GameObjectPool : SingletonObject<GameObjectPool> 
  {
    public GameObject FindGameObjectFromCache(GameObject prefab, Vector3? pos = null, Quaternion? rot = null, GameObject parent = null)
    {
      if (pos == null)
        pos = Vector3.zero;

      if (rot == null)
        rot = Quaternion.identity;

      GameObject _targetObj = null;

      foreach (GameObject item in objectCache) 
      {
        if(item.name == prefab.name ||
          item.name == prefab.name + "(Clone)")
        {
          _targetObj = item;
          break;
        }
      }

      if (_targetObj == null) {
        _targetObj = Instantiate (prefab) as GameObject;
      } 
      else 
      {
        objectCache.Remove(_targetObj);
      }


      if (parent != null) {
        _targetObj.transform.SetParent (parent.transform);
      } 

      _targetObj.transform.position = pos.Value;
      _targetObj.transform.rotation = rot.Value;

      _targetObj.SetActive(true);
      return _targetObj;

    }

    public void CollectGameObject(GameObject myObj)
    {
      myObj.SetActive (false);
      objectCache.Add (myObj);
    }

    List<GameObject> objectCache = new List<GameObject>();
  }
}
