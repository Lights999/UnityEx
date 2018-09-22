using UnityEngine;
using System.Collections;
using DataManagement.TableClass;
using ConstCollections.PJEnums;
using UnityEngine.UI;
using Common;
using DataManagement.TableClass.TableReaderBase;
using System.Collections.Generic;

namespace Common.Controller
{
  public class CommonTextController : AbsMultiLangTextController<StringsTable>
  {
    public string LabelName;

    #region implemented abstract members of AbsMultiLangText

    protected override ushort ID 
    {
      get {
        object _target = System.Enum.Parse(typeof(STRINGS_LABEL), this.LabelName, true);

        if (_target == null)
          throw new System.NullReferenceException (string.Format("{0} was not found!", this.LabelName));
        else
          this.label = (STRINGS_LABEL)_target;

        ushort _ID = StringsTableReader.Instance.FindID (this.label);
        return _ID;
      }
    }

    protected override AbsMulitiLanguageTableReaderBase<StringsTable> readerInstance 
    {
      get {
        return StringsTableReader.Instance;
      }
    }

    #endregion

    [SerializeField, ReadOnly]
    STRINGS_LABEL label;
  }
}
