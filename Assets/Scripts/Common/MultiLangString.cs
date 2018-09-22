using UnityEngine;
using System.Collections;
using DataManagement.SaveData;
using DataManagement.TableClass;
using DataManagement.TableClass.TableReaderBase;
using System;
using System.Text;

namespace Common
{
  // Covariance and Contravariance
  // https://msdn.microsoft.com/zh-cn/library/ee207183(v=vs.110).aspx
  public interface IMultiLangString<out T> : IDisposable where T : AbsMultiLanguageTable, new()
  {
    int PrefixSpaceNumber{ get; set;}

    bool AttachedLangUpdateEvent{ get; set;}

    void LanguageChanged ();

    void AttachLangUpdate ();

    void DisattachLangUpdate ();

    void BindArgs (params object[] args);

    void UpdateArgs (params object[] args);

    void Notify ();
  }

  [System.Serializable]
  public class MultiLangString<T>: IMultiLangString<AbsMultiLanguageTable>/*, IDisposable*/ where T : AbsMultiLanguageTable, new()
  {
    // Ref : http://stackoverflow.com/questions/8624137/not-marked-as-serializable-error-when-serializing-a-class
    [field: System.NonSerialized]
    public event System.Action<string> ValuesChangedEvent;

    public MultiLangString(ushort id, AbsMulitiLanguageTableReaderBase<T> readerInstance, params object[] args)
    {
      this.id = id;
      this.readerInstance = readerInstance;
      SetFormat(this.readerInstance.GetString (id));
      this.args = (args != null && args.Length > 0) ? args: this.args;
      this.ValuesChangedEvent = delegate(string obj) {};
      
      ConfigDataManager.Instance.UserLanguageChangedEvents += LanguageChanged;
    }

    #region IMultiLangString implementation

    public int PrefixSpaceNumber
    { 
      get
      { 
        return this.prefixSpaceNumber;
      }
      set
      { 
        this.prefixSpaceNumber = value;
        SetFormat (this.format);
      }
    }

    public bool AttachedLangUpdateEvent
    {
      get
      { 
        return this.attachedLangUpdateEvent;
      }
      set
      {
        this.attachedLangUpdateEvent = true;
      }
    }

    public void LanguageChanged()
    {
      SetFormat(this.readerInstance.GetString (this.id));
      Notify ();
    }


    public void AttachLangUpdate()
    {
      ConfigDataManager.Instance.UserLanguageChangedEvents += LanguageChanged;
      this.AttachedLangUpdateEvent = true;
    }

    public void DisattachLangUpdate()
    {
      if (this.AttachedLangUpdateEvent == false)
        return;

      ConfigDataManager.Instance.UserLanguageChangedEvents -= LanguageChanged;
      this.AttachedLangUpdateEvent = false;
    }

    public void BindArgs(params object[] args)
    {
      this.args = (args != null && args.Length > 0) ? args: this.args;
    }

    public void UpdateArgs(params object[] args)
    {
      BindArgs (args);
      Notify ();
    }

    public void Notify()
    {
      this.ValuesChangedEvent.Invoke (this.ToString ());
    }

    #endregion

    public override string ToString ()
    {
      if (this.args == null)
        return this.finalFormat.ToString();

      return string.Format (this.finalFormat.ToString(), this.args);
    }

    #region IDisposable implementation

    // Public implementation of Dispose pattern callable by consumers.
    public void Dispose()
    { 
      Dispose(true);
      GC.SuppressFinalize(this);           
    }

    // Protected implementation of Dispose pattern.
    protected virtual void Dispose(bool disposing)
    {
      if (disposed)
        return; 

      if (disposing) {
        // Free any other managed objects here.
        //
      }

      // Free any unmanaged objects here.
      this.DisattachLangUpdate();
      disposed = true;
    }

    ~ MultiLangString()
    {
      Dispose(false);
    }

    #endregion

    void SetFormat(string format)
    {
      this.format = format;

      if(this.finalFormat != null)
        this.finalFormat.Length = 0;
      else
        this.finalFormat = new StringBuilder ();
      
      for (int i = 0; i < this.prefixSpaceNumber; i++) 
      {
        this.finalFormat.Append (" ");
      }
        
      this.finalFormat.Append (this.format);
    }

    [SerializeField]
    int prefixSpaceNumber;
    [SerializeField]
    ushort id;
    [SerializeField]
    string format;
    [SerializeField]
    StringBuilder finalFormat;
    [SerializeField]
    object[] args;
    [SerializeField]
    AbsMulitiLanguageTableReaderBase<T> readerInstance;
    [SerializeField]
    bool attachedLangUpdateEvent;

    // Flag: Has Dispose already been called?
    bool disposed = false;
  }
}
