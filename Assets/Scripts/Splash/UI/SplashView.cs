using UnityEngine;
using System.Collections;
using UnityEngine.Events;
using Common.UI;
using ConstCollections.PJPaths;
using DataManagement.Common;
using UnityEngine.UI;

namespace Splash.UI
{
  public class SplashView : AbsMultiLanguageView 
  {
    public string TitleSpritePrefixPath;
    public Sprite TitleSprite;
    public Sprite CompanySprite;
    public Image LogoImage;

    public UnityEvent ShowTitleEvents;
    public UnityEvent ShowIndicatorEvents;
    public UnityEvent TitleFadeOutEvents;

    protected override void Start()
    {
      if (this.LogoImage == null)
        this.LogoImage = this.transform.Find ("Logo").GetComponent<Image>();
      this.TitleSpritePrefixPath = MultiLanguageImagePrefixPath.TitleSpritePrefixPath;
      base.Start ();
    }

    protected override void OnLanguageChanged ()
    {
      this.TitleSprite = FileIO.Instance.LoadMultiLanguageImage (this.TitleSpritePrefixPath, base.UserLanguage);
    }

    public void ShowCompany()
    {
      GetComponent<Animator> ().SetTrigger ("ShowCompany");
    }

    public void SetTitleImage()
    {
      this.LogoImage.sprite = this.TitleSprite;
      this.LogoImage.SetNativeSize ();
    }

    public void SetCompanyImage()
    {
      this.LogoImage.sprite = this.CompanySprite;
      this.LogoImage.SetNativeSize ();
    }

    public void OnShowTitle()
    {
      ShowTitleEvents.Invoke ();
    }

    public void OnShowIndicator()
    {
      ShowIndicatorEvents.Invoke ();
    }

    public void OnTitleFadeOut()
    {
      TitleFadeOutEvents.Invoke ();
    }
  }
}

