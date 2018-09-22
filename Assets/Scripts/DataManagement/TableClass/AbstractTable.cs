using UnityEngine;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;
using ConstCollections.PJEnums;
using DataManagement.Common;

namespace DataManagement.TableClass
{
  [System.Serializable]
  public abstract class AbstractTable
  {
    [CsvColumnAttribute(0)]
    public ushort ID;

    public AbstractTable(){}

    public override string ToString ()
    {
      return JsonUtility.ToJson (this, true);
    }

    /*
    public override string ToString ()
    {
      
      var memberTypes = new MemberTypes[] { MemberTypes.Field, MemberTypes.Property };

      // インスタンスメンバーを対象とする
      BindingFlags flag = BindingFlags.Instance | BindingFlags.Public; //| BindingFlags.NonPublic;

      List<MemberInfo> _memberInfos =  this.GetType ().GetMembers (flag).Where (member => (memberTypes.Contains (member.MemberType))).ToList ();
      string _strBuffer = "";
      foreach (var item in _memberInfos) {

        if (item.MemberType == MemberTypes.Field) {
          // field
          FieldInfo fieldInfo = this.GetType ().GetField (item.Name, flag);
          _strBuffer += (item.Name + " = ");
          _strBuffer += fieldInfo.GetValue (this).ToString () + toStringSpliter;
        } 
        else 
        {
          // property
          PropertyInfo propertyInfo = this.GetType ().GetProperty(item.Name, flag);
          _strBuffer += (item.Name + " = ");
          _strBuffer += propertyInfo.GetValue(this, null).ToString () + toStringSpliter;
        }

      }

      if (_strBuffer.EndsWith (toStringSpliter))
        _strBuffer = _strBuffer.Remove (_strBuffer.Length - toStringSpliter.Length);

      return _strBuffer;
    }
*/

    public void PrintOut()
    {
      Debug.Log (this.ToString ());
    }

//    static string toStringSpliter = ", ";
  }
}


