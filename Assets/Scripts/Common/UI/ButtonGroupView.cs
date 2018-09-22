using UnityEngine;
using System.Collections;

namespace Common.UI
{
  public class ButtonGroupView : MonoBehaviour 
  {
    public string GroupName;
    public ButtonView[] ButtonViewList;

    void Start()
    {
      foreach (var item in ButtonViewList) 
      {
        if (item.GroupName == null || item.GroupName == "")
          item.GroupName = this.GroupName;
      }
    }

    public void SelectExclusive(ButtonView activeView)
    {
      foreach (var item in ButtonViewList) 
      {
        if (item.GroupName != this.GroupName)
          continue;
        
        if (item == activeView) 
        {
          item.ActivedImage.enabled = true;
        } 
        else 
        {
          item.ActivedImage.enabled = false;
        }
      }
    }
  }
}

