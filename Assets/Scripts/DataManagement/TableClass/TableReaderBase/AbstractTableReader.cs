using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using ConstCollections.PJEnums;
using Common;
using System.Reflection;

namespace DataManagement.TableClass.TableReaderBase
{
  using Common;

  [System.Serializable]
  public abstract class AbstractTableReader<T> where T : AbstractTable, new() 
  {
    public static string ColumnIDName = "ID";

    public virtual string TablePath {
      get {
        return null;
      }
    }

    public virtual List<T> DefaultCachedList {
      get {
        return this.GetDefaultCachedList();
      }
    }

    #region PUBLIC_METHOD
    public virtual List<T> GetCachedList(string tablePath)
    {
      if (this.cachedList == null || this.cachedPath != tablePath ) {
        using(var _reader  = FileIO.Instance.CSVReader<T>(tablePath))
        {
          this.cachedList = _reader.ToList();
          this.cachedPath = tablePath;
        }

      }
      return this.cachedList;//.CloneEx();
    }

    public virtual List<T> GetList(string tablePath)
    {
      using(var _reader  = FileIO.Instance.CSVReader<T>(tablePath))
      {
        return _reader.ToList();
      }
    }

    public virtual CSVReader<T> GetRawTable(string tablePath)
    {
      using(var _reader  = FileIO.Instance.CSVReader<T>(tablePath))
      {
        return _reader;
      }
    }

    public virtual T FindDefaultUnique(string tablePath, ushort ID)
    {
      using(var _reader = FileIO.Instance.CSVReader<T>(tablePath))
      {
        List<T> _rows = _reader.Where(_row => {
          // field
          FieldInfo fieldInfo = _row.GetType ().GetField (ColumnIDName, BindingFlags.Instance | BindingFlags.Public);
          if(fieldInfo == null)
            return false;

          return ((ushort)fieldInfo.GetValue (_row)) == ID;
        }).ToList ();

        if (_rows.Count == 0)
          throw new System.NullReferenceException ();

        if (_rows.Count != 1)
          throw new System.Exception ("Find Unique but got duplicated!");

        return _rows[0];
      }
    }

    public virtual T FindFirstByID(string tablePath, ushort ID)
    {
      using(var _reader = FileIO.Instance.CSVReader<T>(tablePath))
      {
        List<T> _rows = _reader.Where(_row => {
          // field
          FieldInfo fieldInfo = _row.GetType ().GetField (ColumnIDName, BindingFlags.Instance | BindingFlags.Public);
          if(fieldInfo == null)
            return false;

          return ((ushort)fieldInfo.GetValue (_row)) == ID;
        }).ToList ();

        return _rows[0];
      }
    }

    public virtual List<T> FindByID(string tablePath, ushort ID)
    {
      using(var _reader = FileIO.Instance.CSVReader<T>(tablePath))
      {
        List<T> _rows = _reader.Where(_row => {
          // field
          FieldInfo fieldInfo = _row.GetType ().GetField ("ID", BindingFlags.Instance | BindingFlags.Public);
          if(fieldInfo == null)
            return false;

          return ((ushort)fieldInfo.GetValue (_row)) == ID;
        }).ToList ();


        return _rows;
      }
    }

    public virtual T FindDefaultUnique(ushort ID)
    {
      var _rows = this.DefaultCachedList.FindAll (row => {
        return row.ID == ID;
      });

      if (_rows.Count == 0)
        throw new System.NullReferenceException ();

      if (_rows.Count != 1)
        throw new System.Exception ("Find Unique but got duplicated!");

      return _rows[0];

      //      return FindDefaultUnique (this.TablePath, ID);
    }

    public virtual T FindDefaultFirst(ushort ID)
    {
      var _row = this.DefaultCachedList.Find (row => {
        return row.ID == ID;
      });

      return _row;

      //      return FindFirstByID (this.TablePath, ID);
    }

    public virtual List<T> FindFromDefault(ushort ID)
    {
      return FindByID (this.TablePath, ID);
    }

    public virtual void ClearCache()
    {
      this.cachedList = null;
      this.cachedPath = null;
    }
    #endregion

    #region NONE_PUBLIC_METHOD
    protected virtual List<T> GetDefaultCachedList()
    {
      return GetCachedList (this.TablePath);
    }

    protected virtual List<T> GetDefaultList()
    {
      return GetList (this.TablePath);
    }

    protected virtual CSVReader<T> GetDefaultRawTable()
    {
      return GetRawTable (this.TablePath);
    }
    #endregion

    string cachedPath;
    List<T> cachedList;
  }
}
