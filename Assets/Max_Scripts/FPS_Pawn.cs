﻿using System.Collections;
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

    public float interactRange = 2.0f;

    public float crouchSpeed = 0.2f;
    #endregion

    #region Pawn Member Variables
    protected Rigidbody _rb;
    protected GameObject _highlightedObject;
    protected CapsuleCollider _col;

    protected bool _doExamine = false;
    protected bool _isCrouching = false;

    protected float _forwardVelocity = 0.0f;
    protected float _strafeVelocity = 0.0f;
    protected float _playerHeight;
    protected float _playerInitialScale;
    protected float _crouchPercent = 0.0f;
    protected float _zoomPercent = 0.0f;
    protected float _inputXRotation = 0.0f;
    protected float _inputYRotation = 0.0f;

    protected Quaternion _desiredBodyRotation;
    protected Quaternion _desiredCameraRotation;
    #endregion

    protected virtual void Start()
    {
        _playerInitialScale = transform.localScale.y;

        _rb = gameObject.AddComponent<Rigidbody>();
        _rb.freezeRotation = true;

        _desiredBodyRotation = transform.rotation;
        if(!head)
        {
            LOG_ERROR("No head object assigned to " + name);
        }
        _desiredCameraRotation = head.transform.rotation;

        _col = gameObject.GetComponentInChildren<CapsuleCollider>();
        _playerHeight = _col.height;
    }

    protected virtual void Update()
    {
        CameraZoom();
        HandleLookRotation();
    }

    protected virtual void FixedUpdate()
    {
        _rb.velocity = GetMoveVelocity();
        HandleCrouching();
    }

    #region Pawn's Controller Inputs
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
        if(value)
        {
            SetCursorLock(true);

            //Raycast to determine what player is looking at
            int layermask = 1 << LayerMask.NameToLayer("Player");
            layermask = ~layermask;

            RaycastHit hitInfo;
            Physics.Raycast(head.transform.position, head.transform.forward, out hitInfo, interactRange, layermask);

            if(hitInfo.collider)
            {
                Interactable other = hitInfo.collider.GetComponentInChildren<Interactable>();
                if(other)
                {
                    other.InteractWith(this, controller);
                }
            }
        }
    }

    public virtual void Fire2(bool value)
    {
        _doExamine = value;
        if (value)
        {
            SetCursorLock(true);
        }
    }

    public virtual void Fire3(bool value)
    {
        if (_isCrouching && !value)
        {
            Vector3 p1 = _col.transform.position;
            p1.y = _playerHeight * 0.26f;
            Vector3 p2 = p1 + (Vector3.up * _playerHeight * 0.5f);

            int layermask = 1 << LayerMask.NameToLayer("Player");
            layermask = ~layermask;

            bool didCollide = Physics.CheckCapsule(p1, p2, _col.radius * 0.9f, layermask);
            
            if(!didCollide)
            {
                _isCrouching = false;
            }
        }
        else if (value)
        {
            _isCrouching = true;
        }
    }
    #endregion

    protected virtual Vector3 GetMoveVelocity() //Known issue: moving diagonally is faster than moving on other axes.
    {
        Vector3 moveVelocity = new Vector3(0.0f, 0.0f, 0.0f);
        moveVelocity += transform.forward * _forwardVelocity + transform.right * _strafeVelocity;
        moveVelocity *= moveSpeed;
        moveVelocity.y += _rb.velocity.y;

        return moveVelocity;
    }

    protected virtual void HandleCrouching()
    {
        float playerHeightScale = Mathf.Lerp(_playerInitialScale, _playerInitialScale * 0.5f, _crouchPercent);
        transform.localScale = new Vector3(1.0f, playerHeightScale, 1.0f);

        if(_isCrouching && _crouchPercent < 1.0f)
        {
            _crouchPercent += Time.fixedDeltaTime * crouchSpeed;
        }
        else if(!_isCrouching && _crouchPercent > 0.0f)
        {
            _crouchPercent -= Time.fixedDeltaTime * crouchSpeed;
        }
    }

    #region Mouselook
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
    #endregion

    protected virtual void CameraZoom()
    {
        Camera playerCamera = head.GetComponent<Camera>();

        if (playerCamera)
        {
            if (_doExamine && _zoomPercent < 1.0f)
            {
                _zoomPercent += Time.deltaTime * zoomSpeed;
            }
            else if (!_doExamine && _zoomPercent > 0.0f)
            {
                _zoomPercent -= Time.deltaTime * zoomSpeed;
            }


            playerCamera.fieldOfView = Mathf.Lerp(defaultFOV, zoomedFOV, _zoomPercent);
        }
    }
}