using System;
using System.Collections.Generic;
using System.Linq; 
using System.Text; 
using System.Reflection; 
using System.ComponentModel; 
using System.IO; 
using UnityEngine;
using DataManagement.TableClass;

namespace DataManagement.Common
{
  public class CSVReader<T> : IEnumerable<T>, IDisposable
    where T : AbstractTable, new()
  {
    public static string fileExtention = ".csv";

    //public event EventHandler<ConvertFailedEventArgs> ConvertFailed;
    public CSVReader(string fileFullPath, AttributeIndexType attributeIndexType, bool skipFirstLine = true,  Encoding encoding = null)
    {
      if (fileFullPath == null)
        throw new System.NullReferenceException ("fileFullPath == null");
      
      // Check Extension
      if (!fileFullPath.EndsWith(fileExtention, StringComparison.CurrentCultureIgnoreCase))
      {
        throw new Exception(fileFullPath + " 's Extension is not " + fileExtention);
      }
        
      this.filePath = fileFullPath.Remove(fileFullPath.Length - fileExtention.Length);;
      this.skipFirstLine = skipFirstLine;

      /*
      this.encoding = encoding;
      // Check Encoding
      if (this.encoding == null)
      {
        this.encoding = System.Text.Encoding.GetEncoding("utf-8");
      }
     */

      // Tを解析する 
      LoadType(attributeIndexType);
      TextAsset csv = Resources.Load(this.filePath) as TextAsset;

      if (csv == null)
        throw new System.NullReferenceException (string.Format("{0} is not exists!", this.filePath));
      //this.reader = new StreamReader(this.filePath, this.encoding);
      CreateReader (csv);
    }

    public CSVReader(TextAsset csv, AttributeIndexType attributeIndexType, bool skipFirstLine = true, Encoding encoding = null)
    {
      if (csv == null)
        throw new System.NullReferenceException ("csv == null");
      
      // Tを解析する 
      LoadType(attributeIndexType);
      this.filePath = "Got CSV Directly";
      this.skipFirstLine = skipFirstLine;
      CreateReader(csv);
    }

    public void Dispose()
    {
      using (reader)
      {
      }
      reader = null;
    }

    public IEnumerator<T> GetEnumerator()
    {
      string line;
      while ((line = reader.ReadLine()) != null)
      {
        // T のインスタンスを作成
        var data = new T();

        // 行をセパレータで分解
        string[] fields = line.Split(',');

        // セル数分だけループを回す
        foreach (int columnIndex in Enumerable.Range(0, fields.Length))
        {
          // 列番号に対応するsetメソッドがない場合は処理しない
          if (!setters.ContainsKey(columnIndex)) continue;

          // setメソッドでdataに値を入れる
          setters[columnIndex](data, fields[columnIndex]);
        }
        yield return data;
      }
    }

    /// <summary>
    /// Tの情報をロードします。
    /// setterには列番号をキーとしたsetメソッドが格納されます。
    /// </summary>
    void LoadType(AttributeIndexType attributeIndexType)
    {
      Type type = typeof(T);

      // Field, Property のみを対象とする
      var memberTypes = new MemberTypes[] { MemberTypes.Field, MemberTypes.Property };

      // インスタンスメンバーを対象とする
      BindingFlags flag = BindingFlags.Instance | BindingFlags.Public; //| BindingFlags.NonPublic;

      switch (attributeIndexType) 
      {
      case AttributeIndexType.MIXED:
        this.SetValueMixed (type, flag, memberTypes);
        break;
      case AttributeIndexType.WITH_INDEX:
        this.SetValueWithAttributeIndex (type, flag, memberTypes);
        break;
      case AttributeIndexType.WITHOUT_INDEX:
        this.SetValueWithoutAttributeIndex (type, flag, memberTypes);
        break;
      default:
        break;
      }
    }

    void SetValueMixed(Type type, BindingFlags flag, MemberTypes[] memberTypes)
    {
      int _indexLast = SetValueWithAttributeIndex (type, flag, memberTypes);
      SetValueWithoutAttributeIndex (type, flag, memberTypes, _indexLast + 1);

      //TODO: yang-zhang
      /*
      var _members = type.GetMembers (flag).Where ((member) => memberTypes.Contains (member.MemberType)).ToList();
      List<MemberInfo> _membersWithIndex = _members.FindAll(member => GetCsvColumnAttribute(member) != null);
      List<MemberInfo> _membersWithoutIndex = _members.FindAll(member => GetCsvColumnAttribute(member) == null);;

      List<int> _indexList = new List<int> ();
      for (int i = 0; i < _members.Count; i++) {
        _indexList.Add (i);
      }

      foreach (MemberInfo member in _membersWithIndex)
      {
        CsvColumnAttribute csvColumn = GetCsvColumnAttribute(member);

        if (csvColumn == null) continue;

        int columnIndex = csvColumn.ColumnIndex;
        object defaultValue = csvColumn.DefaultValue;

        if (!_indexList.Contains (columnIndex))
          continue;

        if (member.MemberType == MemberTypes.Field)
        {
          // field
          FieldInfo fieldInfo = type.GetField(member.Name, flag);
          setters[columnIndex] = (target, value) =>
            fieldInfo.SetValue(target, GetConvertedValue(fieldInfo, value, defaultValue));
          _indexList.Remove (columnIndex);
        }
        else
        {
          // property
          PropertyInfo propertyInfo = type.GetProperty(member.Name, flag);
          setters[columnIndex] = (target, value) =>
            propertyInfo.SetValue(target, GetConvertedValue(propertyInfo, value, defaultValue), null);
          _indexList.Remove (columnIndex);
        }
      }


      foreach (MemberInfo member in _membersWithoutIndex) 
      {
        int _index = _indexList[0];

        if (member.MemberType == MemberTypes.Field)
        {
          // field
          FieldInfo fieldInfo = type.GetField(member.Name, flag);
          setters[_index] = (target, value) =>
            fieldInfo.SetValue(target, GetConvertedValue(fieldInfo, value, null));
          _indexList.RemoveAt (0);
        }
        else
        {
          // property
          PropertyInfo propertyInfo = type.GetProperty(member.Name, flag);
          setters[_index] = (target, value) =>
            propertyInfo.SetValue(target, GetConvertedValue(propertyInfo, value, null), null);
          _indexList.RemoveAt (0);
        }
      }
      */
    }

    int SetValueWithoutAttributeIndex(Type type, BindingFlags flag, MemberTypes[] memberTypes, int indexStart = 0)
    {
      int _index = indexStart;
      foreach (MemberInfo member in type.GetMembers(flag).Where((member) => memberTypes.Contains(member.MemberType))) 
      {
        CsvColumnAttribute csvColumn = GetCsvColumnAttribute(member);

        if (csvColumn != null) continue;

        if (member.MemberType == MemberTypes.Field)
        {
          // field
          FieldInfo fieldInfo = type.GetField(member.Name, flag);
          setters[_index] = (target, value) =>
            fieldInfo.SetValue(target, GetConvertedValue(fieldInfo, value, null));
        }
        else
        {
          // property
          PropertyInfo propertyInfo = type.GetProperty(member.Name, flag);
          setters[_index] = (target, value) =>
            propertyInfo.SetValue(target, GetConvertedValue(propertyInfo, value, null), null);
        }

        _index++;
      }

      return _index;
    }

    int SetValueWithAttributeIndex(Type type, BindingFlags flag, MemberTypes[] memberTypes)
    {
      //int columnIndex = 0;
      List<int> _colIndexList = new List<int> ();
      foreach (MemberInfo member in type.GetMembers(flag).Where((member) => memberTypes.Contains(member.MemberType)))
      {
        CsvColumnAttribute csvColumn = GetCsvColumnAttribute(member);

        if (csvColumn == null) continue;

        int columnIndex = csvColumn.ColumnIndex;
        object defaultValue = csvColumn.DefaultValue;

        _colIndexList.Add (columnIndex);

        if (member.MemberType == MemberTypes.Field)
        {
          // field
          FieldInfo fieldInfo = type.GetField(member.Name, flag);
          setters[columnIndex] = (target, value) =>
            fieldInfo.SetValue(target, GetConvertedValue(fieldInfo, value, defaultValue));
        }
        else
        {
          // property
          PropertyInfo propertyInfo = type.GetProperty(member.Name, flag);
          setters[columnIndex] = (target, value) =>
            propertyInfo.SetValue(target, GetConvertedValue(propertyInfo, value, defaultValue), null);
        }
      }

      if (_colIndexList.Count == 0)
        return -1;

      return _colIndexList.Max ();
    }

    /// <summary>
    /// Creates the reader.
    /// </summary>
    /// <param name="csv">Csv.</param>
    void CreateReader(TextAsset csv)
    {
      this.reader = new StringReader (csv.text);
      // ヘッダーを飛ばす場合は1行読む
      if (this.skipFirstLine)
      {
        this.reader.ReadLine();
      }
    }

    /// <summary>
    /// 対象のMemberInfoからCsvColumnAttributeを取得する
    /// </summary>
    /// <param name="member">確認対象のMemberInfo</param>
    /// <returns>CsvColumnAttributeのインスタンス、設定されていなければnull</returns>
    CsvColumnAttribute GetCsvColumnAttribute(MemberInfo member)
    {
      return  (CsvColumnAttribute) member.GetCustomAttributes(typeof(CsvColumnAttribute), true).FirstOrDefault();
      //return member.GetCustomAttributes<CsvColumnAttribute>().FirstOrDefault();
    }

    /// <summary>
    /// valueを対象のTypeへ変換する。できない場合はdefaultを返す
    /// </summary>
    /// <param name="type">変換後の型</param>
    /// <param name="value">変換元の値</param>
    /// <param name="default">規定値</param>
    /// <returns></returns>
    object GetConvertedValue(MemberInfo info, object value, object @default)
    {
      Type type = null;
      if (info is FieldInfo)
      {
        type = (info as FieldInfo).FieldType;
      }
      else if (info is PropertyInfo)
      {
        type = (info as PropertyInfo).PropertyType;
      }

      // コンバーターは同じTypeを使用することがあるため、キャッシュしておく
      if (!converters.ContainsKey(type))
      {
        converters[type] = TypeDescriptor.GetConverter(type);
      }

      TypeConverter converter = converters[type];

      ////変換できない場合に例外を受け取りたい場合
      //return converter.ConvertFrom(value);

      //失敗した場合に CsvColumnAttribute の規定値プロパティを返す場合
      try
      {

        // Convert to object of type
        return converter.ConvertFrom(value);
      }
      catch (Exception e)
      {
        Debug.LogError (e);

        // 変換できなかった場合は規定値を返す
        return @default;
      }

      //// 変換できない場合に、イベントを発生させ使用者に判断させる場合
      //try
      //{
      //    return converter.ConvertFrom(value);
      //}
      //catch (Exception ex)
      //{
      //    // イベント引数の作成
      //    var e = new ConvertFailedEventArgs(info, value, @default, ex);

      //    // イベントに関連付けられたメソッドがない場合は例外を投げる
      //    if (ConvertFailed == null)
      //    {
      //        throw;
      //    }

      //    // 使用する際に判断させる
      //    ConvertFailed(this, e);

      //    // 正しい値を返す
      //    return e.CorrectValue;
      //}
    }

    System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
    {
      return this.GetEnumerator();
    }
      
    StringReader reader;
    string filePath;
    bool skipFirstLine;
    //    Encoding encoding;

    /// <summary>
    /// Type毎のデータコンバーター
    /// </summary>
    Dictionary<Type, TypeConverter> converters = new Dictionary<Type, TypeConverter>();

    /// <summary>
    /// 列番号をキーとしてフィールド or プロパティへのsetメソッドが格納されます。
    /// </summary>
    Dictionary<int, Action<object, string>> setters = new Dictionary<int, Action<object, string>>();

    //public List<T> cachedList;
  }

  /// <summary>
  /// 変換失敗時のイベント引数クラス
  /// </summary>
  public class ConvertFailedEventArgs : EventArgs
  {
    public ConvertFailedEventArgs(MemberInfo info, object value, object defaultValue, Exception ex)
    {
      this.MemberInfo = info;
      this.FailedValue = value;
      this.CorrectValue = defaultValue;
      this.Exception = ex;
    }

    /// <summary>
    /// 変換に失敗したメンバーの情報
    /// </summary>
    public MemberInfo MemberInfo { get; private set; }

    /// <summary>
    /// 失敗時の値
    /// </summary>
    public object FailedValue { get; private set; }

    /// <summary>
    /// 正しい値をイベントで受け取る側が設定してください。規定値はCsvColumnAttribute.DefaultValueです。
    /// </summary>
    public object CorrectValue { get; set; }

    /// <summary>
    /// 発生した例外
    /// </summary>
    public Exception Exception { get; private set; }
  }

  public enum AttributeIndexType
  {
    NONE,
    WITH_INDEX,
    WITHOUT_INDEX,
    MIXED
  }
}
