using UnityEngine;
using System.Collections;

namespace DataManagement.TableClass
{
  [System.Serializable]
  public class AbsMultiLanguageTable : AbstractTable 
  {
    public string TextEN;
    public string TextJP;
    public string TextCNS;
    public string TextCNT;

    public AbsMultiLanguageTable(){}
  }
}
