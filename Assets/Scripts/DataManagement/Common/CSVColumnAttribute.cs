using UnityEngine;
using System;

namespace DataManagement.Common
{
  /// <summary>
  /// Mapping coloumns of CSV
  /// </summary>
  public class CsvColumnAttribute : Attribute
  {
    public CsvColumnAttribute(int columnIndex)
      : this(columnIndex, null)
    {
    }

    public CsvColumnAttribute(int columnIndex, object defaultValue)
    {
      this.ColumnIndex = columnIndex;
      this.DefaultValue = defaultValue;
    }
    public int ColumnIndex { get; set; }
    public object DefaultValue { get; set; }
  }
}
