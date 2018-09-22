using UnityEngine;
using System.Collections;
using DataManagement.SaveData;
using System.Linq;
using System.Text;

namespace DataManagement.TableClass.TableReaderBase
{
  [System.Serializable]
  public abstract class AbsMulitiLanguageTableReaderBase<T> : AbstractTableReader<T> where T : AbsMultiLanguageTable, new()
  {
    public virtual string GetString(ushort ID, SystemLanguage? lang = null)
    {
      AbsMultiLanguageTable _row =  base.FindDefaultUnique (ID);
      SystemLanguage _lang = lang ?? ConfigDataManager.Instance.UserLanguage;
      return GetString (_row, _lang);
    }
      
    public static string GetString(AbsMultiLanguageTable row, SystemLanguage lang)
    {
      if (row == null)
        return null;

      StringBuilder _str = new StringBuilder();
      switch (lang) 
      {
      case SystemLanguage.English:
        _str.Append(row.TextEN);
        break;
      case SystemLanguage.Japanese:
        _str.Append(row.TextJP);
        break;
      case SystemLanguage.ChineseSimplified:
        _str.Append(row.TextCNS);
        break;
      case SystemLanguage.ChineseTraditional:
        _str.Append(row.TextCNT);
        break;
      case SystemLanguage.Chinese:
        _str.Append(row.TextCNS);
        break;
      default:
        _str.Append(row.TextEN);
        break;
      }

      return StringFormat (_str);
    }

    public static string StringFormat(StringBuilder fromCSV)
    {
      if (fromCSV == null || fromCSV.Length == 0)
        return null;

      return fromCSV.
        Replace("<br>", "\n").
        Replace("\\n", "\n").
        Replace("<cm>", ",").
        Replace("<dq>", "\"").ToString();
    }

//    public static string StringFormat(string fromCSV)
//    {
//      if (fromCSV == null || fromCSV.Count() == 0)
//        return null;
//
//      return fromCSV.
//        Replace("<br>", "\n").
//        Replace("\\n", "\n").
//        Replace("<cm>", ",").
//        Replace("<dq>", "\"");
//    }
  }
}

