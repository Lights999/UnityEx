using UnityEngine;
using System.Collections;
using DataManagement.TableClass.TableReaderBase;
using DataManagement.TableClass;
using UnityEngine.UI;

namespace Common.Controller
{
  [System.Serializable]
  public abstract class AbsMultiLangTextController<T> : MonoBehaviour where T : AbsMultiLanguageTable, new()
  {
    public MultiLangString<T> MultiLangString{
      get
      { 
        if (this.multiLangString == null) 
        {
          this.multiLangString = new MultiLangString<T> (this.ID, this.readerInstance);
        }

        return this.multiLangString;
      }
    }

    protected abstract ushort ID{ get;}
    protected abstract AbsMulitiLanguageTableReaderBase<T> readerInstance{ get;}

//    protected virtual void Awake()
//    {
//      this.MultiLangString = new MultiLangString<T> (ID, readerInstance);
//    }

    protected virtual void OnEnable()
    {
      this.text = GetComponent<Text> ();
      this.MultiLangString.ValuesChangedEvent += UpdateText;
    }

    protected virtual void Start()
    {
      this.MultiLangString.Notify ();
    }

    protected virtual void OnDisable()
    {
      this.MultiLangString.ValuesChangedEvent -= UpdateText;
    }

    public virtual void UpdateText(string str)
    {
      this.text.text = str;
    }

    protected Text text;
    MultiLangString<T> multiLangString;
  }
}

