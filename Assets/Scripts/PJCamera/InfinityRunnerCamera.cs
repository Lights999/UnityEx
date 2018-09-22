using UnityEngine;
using System.Collections;

namespace PJCamera
{
  [RequireComponent(typeof(Camera))]
  public class InfinityRunnerCamera : MonoBehaviour {
    public Transform Target;
    public Camera CameraScript;
    public CAMERA_MOTION_TYPE CurrentMotion = CAMERA_MOTION_TYPE.LOOKAT_HORIZON;
    public LOOKAT_MODE CameraMode = LOOKAT_MODE.DIRECTLY;
    public bool LookAtTargetToCenter;

    [Range(0.1F, 20.0F)]
    public float SmoothSpeed = 2.2f;

    [Range(0.001F, 0.1F)]
    public float NeedToMoveDistanceSqr = 0.001F;
    public bool NeedToMove = false;

    [Range(0.01F, 1.0F)]
    public float ShakeTimeSecondsMax = 0.2F;
    [Range(0.01F, 1.0F)]
    public float ShakeAmount = 0.2F;

    void Start () 
    {
      if (this.CameraScript == null)
        this.CameraScript = GetComponent<Camera> ();

      if (this.LookAtTargetToCenter) {
        positionRelativeToTarget = Vector3.zero;

      } 
      else 
      {
        positionRelativeToTarget = transform.position - Target.position;
      }
      positionRelativeToTarget.z = transform.position.z;

    }

    void LateUpdate() 
    {
      switch (this.CurrentMotion) 
      {
      case CAMERA_MOTION_TYPE.LOOKAT:
        this.LookAt ();
        break;
      case CAMERA_MOTION_TYPE.LOOKAT_HORIZON:
        this.LookAtHorizon ();
        break;
      case CAMERA_MOTION_TYPE.LOOKAT_VERTICAL:
        this.LookAtVertical ();
        break;
      case CAMERA_MOTION_TYPE.SHAKE:
        break;
      case CAMERA_MOTION_TYPE.NONE:
        break;
      }
    }

    public void SetTarget(Transform target, bool lookAtImmediately = false) 
    {
      this.Target = target;
      if (lookAtImmediately) 
      {
        this.targetToCameraPosition = this.Target.position + this.positionRelativeToTarget;
        this.DirectlyLookAt ();
      }
    }

    public void Shake()
    {
      StopCoroutine (this.ShakeCoroutine ());
      StartCoroutine (this.ShakeCoroutine ());
    }

    public IEnumerator ShakeCoroutine()
    {
      this.CurrentMotion = CAMERA_MOTION_TYPE.SHAKE;

      float _timerMax = this.ShakeTimeSecondsMax;
      Vector3 _posOrg = this.transform.position;
      while (_timerMax > 0.0F) 
      {
        _timerMax -= Time.deltaTime;
        Vector3 _offset = Random.insideUnitCircle * this.ShakeAmount;
        this.transform.position = _posOrg + _offset;
        yield return null;
      }
        
      this.transform.position = _posOrg;
      this.CurrentMotion = CAMERA_MOTION_TYPE.LOOKAT;
      yield break;
    }

    void LookAt()
    {
      if (this.Target == null) 
        return;

      this.targetToCameraPosition = this.Target.position + positionRelativeToTarget;
      targetMovedDistance = (this.targetToCameraPosition - this.transform.position).sqrMagnitude;

      if (targetMovedDistance < NeedToMoveDistanceSqr)
        this.NeedToMove = false;
      else 
      {
        
        this.targetToCameraPosition.y = this.transform.position.y;
        this.NeedToMove = true;
      }

      if (!this.NeedToMove)
        return;

      switch(this.CameraMode) 
      {
      case LOOKAT_MODE.DIRECTLY:
        DirectlyLookAt ();
        break;
      case LOOKAT_MODE.SMOOTH:
        SmoothLookAt();
        break;
      }
    }

    void LookAtHorizon()
    {
      if (this.Target == null) 
        return;

      this.targetToCameraPosition = this.Target.position + positionRelativeToTarget;
      this.targetToCameraPosition.y = this.transform.position.y;

      targetMovedDistance = Mathf.Abs(this.transform.position.x - this.targetToCameraPosition.x);
      if (targetMovedDistance < NeedToMoveDistanceSqr)
        this.NeedToMove = false;
      else 
      {
        this.NeedToMove = true;
      }

      if (!this.NeedToMove)
        return;

      switch(this.CameraMode) 
      {
      case LOOKAT_MODE.DIRECTLY:
        DirectlyLookAt ();
        break;
      case LOOKAT_MODE.SMOOTH:
        SmoothLookAt();
        break;
      }
    }

    void LookAtVertical()
    {
      if (this.Target == null) 
        return;

      this.targetToCameraPosition = this.Target.position + positionRelativeToTarget;
      this.targetToCameraPosition.x = this.transform.position.x;

      targetMovedDistance = Mathf.Abs(this.transform.position.y - this.Target.position.y);
      if (targetMovedDistance < NeedToMoveDistanceSqr)
        this.NeedToMove = false;
      else 
      {
        this.NeedToMove = true;
      }

      if (!this.NeedToMove)
        return;

      switch(this.CameraMode) 
      {
      case LOOKAT_MODE.DIRECTLY:
        DirectlyLookAt ();
        break;
      case LOOKAT_MODE.SMOOTH:
        SmoothLookAt();
        break;
      }
    }

    void DirectlyLookAt()
    {
      transform.position = this.targetToCameraPosition; 
    }

    void SmoothLookAt () 
    {
      transform.position = Vector3.MoveTowards (this.transform.position, this.targetToCameraPosition, SmoothSpeed * Time.deltaTime);
    }

    #region PRIVATE

    [SerializeField] Vector3 positionRelativeToTarget;
    [SerializeField] Vector3 targetToCameraPosition;
    [SerializeField] float targetMovedDistance;

    #endregion
  }

  public enum CAMERA_MOTION_TYPE
  {
    NONE = 0,
    LOOKAT,
    LOOKAT_HORIZON,
    LOOKAT_VERTICAL,
    SHAKE,
  }

  public enum LOOKAT_MODE {
    DIRECTLY = 0,
    SMOOTH
  }
}

