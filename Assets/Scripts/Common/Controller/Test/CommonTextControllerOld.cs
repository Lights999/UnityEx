using UnityEngine;
using System.Collections;
using ConstCollections.PJEnums;
using UnityEngine.UI;
using DataManagement.TableClass;

namespace Common.Controller.Test
{
  public class CommonTextControllerOld : AbsTextControllerOld {

    public string LabelName;

    protected override void Awake()
    {
      base.Awake ();
      object _target = System.Enum.Parse(typeof(STRINGS_LABEL), this.LabelName, true);

      if (_target == null)
        throw new System.NullReferenceException (string.Format("{0} was not found!", this.LabelName));
      else
        this.label = (STRINGS_LABEL)_target;
    }

    #region implemented abstract members of AbsTextController

    protected override string Format {
      get {
        return StringsTableReader.Instance.GetString (this.label);
      }
    }

    #endregion

    [SerializeField, ReadOnly]
    STRINGS_LABEL label;
  }
}
