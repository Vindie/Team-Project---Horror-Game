using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FPS_Pawn : Pawn {

    #region Pawn Properties
    
    public float defaultFOV = 60.0f;
    public float zoomedFOV = 40.0f;
    public float zoomSpeed;

    public float moveSpeed = 1.0f;

    public GameObject head;
    public float look_xSensitivity = 2.0f;
    public float look_ySensitivity = 2.0f;
    public float look_maxVerticalRotation = -90.0f;
    public float look_minVerticalRotation = 90.0f;
    #endregion

    #region Pawn member vars
    protected Rigidbody _rb;
    protected GameObject highlightedObject;

    protected bool doExamine = false;
    protected bool isCrouching = false;

    protected float _forwardVelocity = 0.0f;
    protected float _strafeVelocity = 0.0f;
    protected float _zoomPercent = 0.0f;

    protected float _inputXRotation = 0.0f;
    protected float _inputYRotation = 0.0f;
    protected Quaternion _desiredBodyRotation;
    protected Quaternion _desiredCameraRotation;
    #endregion

    protected virtual void Start()
    {
        _rb = gameObject.AddComponent<Rigidbody>();
        _rb.freezeRotation = true;

        _desiredBodyRotation = transform.rotation;
        if(!head)
        {
            LOG_ERROR("No head object assigned to " + name);
        }
        _desiredCameraRotation = head.transform.rotation;
    }

    protected virtual void Update()
    {
        CameraZoom();
        HandleLookRotation();
    }

    protected virtual void FixedUpdate()
    {
        _rb.velocity = GetMoveVelocity();
    }

    public virtual void LookHorizontal(float value)
    {
        _inputXRotation = value * look_xSensitivity;
    }

    public virtual void LookVertical(float value)
    {
        _inputYRotation = value * look_ySensitivity;
    }

    public virtual void MoveHorizontal(float value)
    {
        _strafeVelocity = value;
    }

    public virtual void MoveVertical(float value)
    {
        _forwardVelocity = value;
    }

    public virtual void Fire1(bool value)
    {
        //highLighted = raycast(head.forward)
        //highLighted.getComponent<Interactable>().interact(this);
        if(value) LOG("Interact!");
        if(value)
        {
            SetCursorLock(true);
        }
    }

    public virtual void Fire2(bool value)
    {
        doExamine = value;
        if (value)
        {
            SetCursorLock(true);
        }
    }

    public virtual void Fire3(bool value)
    {
        if (isCrouching && !value)
        {
            //Uncrouch: Do a Physics.CapsuleCast() to see if the default Capsule height would collide with anything. If it would, don't uncrouch. Else, set collider size to standing and lerp camera height.
            isCrouching = false;
        }
        else if (value)
        {
            //Crouch: Set collider size to crouching.
            isCrouching = true;
        }

        if (value) LOG("Crouching: " + isCrouching);
    }

    protected virtual Vector3 GetMoveVelocity() //Known issue: moving diagonally is faster than moving on other axes.
    {
        Vector3 moveVelocity = new Vector3(0.0f, _rb.velocity.y, 0.0f);
        moveVelocity += transform.forward * _forwardVelocity + transform.right * _strafeVelocity;
        moveVelocity *= moveSpeed;

        return moveVelocity;
    }

    protected virtual void CameraZoom()
    {
        Camera playerCamera = head.GetComponent<Camera>();

        if (playerCamera)
        {
            if (doExamine && _zoomPercent < 1.0f)
            {
                _zoomPercent += Time.deltaTime * zoomSpeed;
            }
            else if (!doExamine && _zoomPercent > 0.0f)
            {
                _zoomPercent -= Time.deltaTime * zoomSpeed;
            }

        
            playerCamera.fieldOfView = Mathf.Lerp(defaultFOV, zoomedFOV, _zoomPercent);
        }
    }

    protected virtual void HandleLookRotation()
    {
        _desiredBodyRotation *= Quaternion.Euler(0.0f, _inputYRotation, 0.0f);
        _desiredCameraRotation *= Quaternion.Euler(-_inputXRotation, 0.0f, 0.0f);

        _desiredCameraRotation = ClampRotationAroundX(_desiredCameraRotation);

        transform.localRotation = _desiredBodyRotation;
        head.transform.localRotation = _desiredCameraRotation;
    }

    protected virtual Quaternion ClampRotationAroundX(Quaternion q)
    {
        q.x /= q.w;
        q.y /= q.w;
        q.z /= q.w;
        q.w = 1.0f;

        float angleX = 2.0f * Mathf.Rad2Deg * Mathf.Atan(q.x);

        angleX = Mathf.Clamp(angleX, look_minVerticalRotation, look_maxVerticalRotation);

        q.x = Mathf.Tan(0.5f * Mathf.Deg2Rad * angleX);

        return q;
    }

    public virtual void SetCursorLock(bool newLockState)
    {
        if(newLockState)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
        else
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }
}