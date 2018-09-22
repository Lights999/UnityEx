using UnityEngine;
using System.Collections;
using Common.UI;
using UnityEngine.UI;
using Common;
using DataManagement.TableClass.TableReaderBase;
using DataManagement.TableClass;

namespace Common.Controller.Test
{
  public abstract class AbsTextControllerOld : AbsMultiLanguageView 
  {
    protected abstract string Format { get;}

    protected override void OnEnable()
    {
      base.OnEnable ();
      this.text = GetComponent<Text> ();
    }

    protected override void Start()
    {
      UpdateText ();
    }

    public virtual void UpdateArgs(params object[] args)
    {
      this.args = (args != null && args.Length > 0) ? args: this.args;
    }

    public virtual void UpdateText(params object[] args)
    {
      UpdateArgs (args);

      if(this.args == null)
        this.text.text = this.Format;
      else
        this.text.text = string.Format (this.Format, this.args);
    }

    protected override void OnLanguageChanged()
    {
      this.UpdateText ();
    }

    protected object[] args;

    protected Text text;
  }
}
