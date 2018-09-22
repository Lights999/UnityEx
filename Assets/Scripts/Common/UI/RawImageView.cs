using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using PJMath;

namespace Common.UI
{
  [RequireComponent(typeof(RawImage))]
  [RequireComponent(typeof(RectTransform))]
  public class RawImageView : MonoBehaviour {
    public float EraseHeightFrom = 0.6F;
    public float EraseHeightTo = 0.4F;
    [Range(0.1F, 10.0F)]
    public float EraseSeconds = 1.0F;

    public bool ShowAnimationAuto;
    public bool EnableUpdate;

    #region UNITY_METHOD
    // Use this for initialization
    void Start () 
    {
      this.rawImage = GetComponent<RawImage> ();
      this.rectTrans = GetComponent<RectTransform> ();

      #if UNITY_IOS
      this.rectTransDeltaSizeMax = this.rectTrans.sizeDelta;
      this.scaleToRectTransWidth = this.rectTransDeltaSizeMax.x / (float)this.rawImage.texture.width;
      this.scaleToRectTransHeight = this.rectTransDeltaSizeMax.y / (float)this.rawImage.texture.height;
      #else
      this.scaleToRectTransWidth = 1.0F;
      this.scaleToRectTransHeight = 1.0F;
      #endif

      #if UNITY_EDITOR
      this.prevWidthRate = this.ratioWidth;
      this.prevHeightRate = this.ratioHeight;
      #endif

      if (ShowAnimationAuto)
        PlayEraseHeight (this.EraseSeconds);
    }

    #if UNITY_EDITOR
    // Update is called once per frame
    void Update () 
    {

      if(EnableUpdate)
        CutImage();
    }
    #endif

    void OnDisable()
    {
      StopAllCoroutines ();
    }
    #endregion

    #region PUBLIC_METHOD
    public void PlayEraseHeight(float eraseSeconds)
    {
      this.EraseSeconds = eraseSeconds;
      PlayEraseHeight (this.EraseHeightFrom, this.EraseHeightTo, this.EraseSeconds);
    }

    public void PlayEraseHeight(float from, float to, float eraseSeconds)
    {
      StartCoroutine(LerpHelper.LerpCoroutine(eraseSeconds, _percentage =>
        {
          this.ratioHeight = Mathf.Lerp(from, to, _percentage);
          CutImage();
        }));
    }

    public void CutImage()
    {
      #if UNITY_EDITOR
      if (!Mathf.Approximately (this.prevWidthRate, this.ratioWidth) || !Mathf.Approximately (this.prevHeightRate, this.ratioHeight)) 
      {
        lockValidArea = false;
      }
      #endif

      if (lockValidArea) 
      {
        this.ratioWidth = 1.0F - this.ratioX;
        this.ratioHeight = 1.0F - this.ratioY;
      }
        

      this.rawImage.uvRect = new Rect (this.ratioX, this.ratioY, this.ratioWidth, this.ratioHeight);
      this.rectTrans.sizeDelta = new Vector2 (
        this.rawImage.texture.width * this.scaleToRectTransWidth * this.ratioWidth, 
        this.rawImage.texture.height * this.scaleToRectTransHeight * this.ratioHeight);
      this.rectTrans.anchoredPosition = new Vector2 (
        this.rawImage.texture.width * this.scaleToRectTransWidth * (this.ratioX + (this.ratioWidth - 1.0F) * this.rectTrans.pivot.x), 
        this.rawImage.texture.height * this.scaleToRectTransHeight * (this.ratioY + (this.ratioHeight - 1.0F) * this.rectTrans.pivot.y));

      #if UNITY_EDITOR
      this.prevWidthRate = this.ratioWidth;
      this.prevHeightRate = this.ratioHeight;
      #endif
    }
    #endregion

    #region PRIVATE_MEMBER
    [SerializeField, Range(0.0F, 1.0F)]
    float ratioX = 0.0F;
    [SerializeField, Range(0.0F, 1.0F)]
    float ratioY = 0.0F;
    [SerializeField, Range(0.0F, 1.0F)]
    float ratioWidth = 1.0F;
    [SerializeField, Range(0.0F, 1.0F)]
    float ratioHeight = 1.0F;
    [SerializeField]
    bool lockValidArea;

    RawImage rawImage;
    RectTransform rectTrans;
//    Coroutine eraseRawImageCoroutine;

    float prevWidthRate;
    float prevHeightRate;
    Vector2 rectTransDeltaSizeMax;
    float scaleToRectTransWidth;
    float scaleToRectTransHeight;
    #endregion
  }
}
