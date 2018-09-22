using UnityEngine;
using System.Collections;
using UnityEngine.Events;
using UnityEngine.UI;
using ConstCollections.PJEnums;
using DataManagement.TableClass;

namespace Common.UI
{
  public class TextView : AbsMultiLanguageView
  {
    public STRINGS_LABEL Label;

    protected override void OnEnable()
    {
      base.OnEnable ();
      InitComponent();
    }

    protected override void OnLanguageChanged()
    {
//      if(base.Args == null)
        this.text.text = string.Format (StringsTableReader.Instance.GetString (Label));
//      else
//        this.text.text = string.Format (StringsTableReader.Instance.GetString (Label), (base.Args as object[]));
    }

    protected void InitComponent()
    {
      this.text = GetComponent<Text> ();
    }

    protected Text text;
  }
}


