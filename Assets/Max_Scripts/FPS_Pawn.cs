using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FPS_Pawn : Pawn {

    #region Pawn Properties
    public float moveSpeed = 1.0f;
    public GameObject head;
    public float defaultFOV = 60;
    public float zoomedFOV = 40;
    public float zoomSpeed;
    #endregion

    #region Pawn member vars
    protected Rigidbody _rb;
    protected GameObject highlightedObject;

    protected bool doExamine = false;
    protected bool isCrouching = false;

    protected float _forwardVelocity = 0.0f;
    protected float _strafeVelocity = 0.0f;
    protected float _zoomPercent = 0.0f;
    #endregion

    protected virtual void Start()
    {
        _rb = gameObject.AddComponent<Rigidbody>();
        _rb.freezeRotation = true;
    }

    protected virtual void Update()
    {
        CameraZoom();
    }

    protected virtual void FixedUpdate()
    {
        _rb.velocity = GetMoveVelocity();
    }

    public virtual void LookHorizontal(float value)
    {

    }

    public virtual void LookVertical(float value)
    {

    }

    public virtual void MoveHorizontal(float value)
    {
        _strafeVelocity = value;
    }

    public virtual void MoveVertical(float value)
    {
        _forwardVelocity = value;
    }

    public virtual void Interact(bool value)
    {
        //highLighted.getComponent<Interactable>().interact(this);
    }

    public virtual void Examine(bool value)
    {
        doExamine = value;
    }

    public virtual void Crouch(bool value)
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
            if (doExamine && _zoomPercent > 0.0f)
            {
                _zoomPercent += Time.deltaTime * zoomSpeed;
            }
            else if (_zoomPercent < 1.0f)
            {
                _zoomPercent -= Time.deltaTime * zoomSpeed;
            }

        
            playerCamera.fieldOfView = Mathf.Lerp(zoomedFOV, defaultFOV, _zoomPercent);
        }
    }
}
