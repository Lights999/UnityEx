using UnityEngine;
using System.Collections;
using ConstCollections.PJEnums;
using DataManagement.Common;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;
using DataManagement.SaveData;

namespace DataManagement.TableClass.TableReaderBase
{
  [System.Serializable]
  public class StringsTableReaderBase : AbsMulitiLanguageTableReaderBase<StringsTable> {
    public static string ColumnLabelName = "Label";

    public override string TablePath {
      get {
        return "Data/strings.csv";
      }
    }

    public ushort FindID(STRINGS_LABEL label, SystemLanguage? lang = null)
    {
      var _row = this.DefaultCachedList.Find (row => {
        return row.Label == label;
      });

      return _row.ID;
      /*
       * Open CSV file every time is too slow
      using(var _reader = FileIO.Instance.CSVReader<StringsTable>(TablePath))
      {
        List<StringsTable> _rows = _reader.Where(_row => {
          // field
          FieldInfo fieldInfo = _row.GetType ().GetField (ColumnLabelName, BindingFlags.Instance | BindingFlags.Public);
          if(fieldInfo == null)
            return false;

          return ((STRINGS_LABEL)fieldInfo.GetValue (_row) == label);
        }).ToList ();

        if (_rows.Count == 0)
          throw new System.NullReferenceException ();

        if (_rows.Count > 1)
          throw new System.Exception ("Find Unique but got duplicated!");

        return _rows[0].ID;
      }
      */
    }

    public string GetString(STRINGS_LABEL label, SystemLanguage? lang = null)
    {
      if (label == STRINGS_LABEL.BLANK)
        return "";

      var _row = this.DefaultCachedList.Find (row => {
        return row.Label == label;
      });

      SystemLanguage _lang = lang ?? ConfigDataManager.Instance.UserLanguage;
      return GetString (_row, _lang);

      /*
       * Open CSV file every time is too slow
      using(var _reader = FileIO.Instance.CSVReader<StringsTable>(TablePath))
      {
        List<StringsTable> _rows = _reader.Where(_row => {
          // field
          FieldInfo fieldInfo = _row.GetType ().GetField (ColumnLabelName, BindingFlags.Instance | BindingFlags.Public);
          if(fieldInfo == null)
            return false;

          return ((STRINGS_LABEL)fieldInfo.GetValue (_row) == label);
        }).ToList ();

        if (_rows.Count == 0)
          throw new System.NullReferenceException ();

        if (_rows.Count > 1)
          throw new System.Exception ("Find Unique but got duplicated!");

        SystemLanguage _lang = lang ?? ConfigDataManager.Instance.UserLanguage;
        return GetString (_rows[0], _lang);
      }
      */
    }

    /*
    public static string GetString(MultiLanguageTable row, SystemLanguage lang)
    {
      if (row == null)
        return null;
      
      string _str;
      switch (lang) 
      {
      case SystemLanguage.English:
        _str = row.TextEN;
        break;
      case SystemLanguage.Japanese:
        _str = row.TextJP;
        break;
      case SystemLanguage.ChineseSimplified:
        _str = row.TextCNS;
        break;
      case SystemLanguage.ChineseTraditional:
        _str = row.TextCNT;
        break;
      case SystemLanguage.Chinese:
        _str = row.TextCNS;
        break;
      default:
        _str = row.TextEN;
        break;
      }

      return StringFormat (_str);
    }

    public static string StringFormat(string fromCSV)
    {
      if (fromCSV == null || fromCSV.Count() == 0)
        return null;
      
      return fromCSV.
        Replace("<br>", "\n").
        Replace("\\n", "\n").
        Replace("<cm>", ",").
        Replace("<dq>", "\"");
    }
    */
  }
}


