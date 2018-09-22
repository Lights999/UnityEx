using UnityEngine;
using System.Collections;
using System.Linq;
using ConstCollections.PJEnums;
using DataManagement.Common;

namespace DataManagement.TableClass
{
  [System.Serializable]
  public class StringsTable : AbsMultiLanguageTable 
  {
    [CsvColumnAttribute(1)]
    public STRINGS_LABEL Label;
  }
}
