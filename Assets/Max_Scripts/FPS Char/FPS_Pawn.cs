using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FPS_Pawn : Pawn
{

    #region Pawn Properties

    public float defaultFOV = 60.0f;

    public float Health = 100.0f;
    public float MaxBloodyTime = 7.0f;
    public float MaxHealthBloodiness = 0.5f;

    public float moveSpeed = 1.0f;
    public bool allowSprint = false;
    public float sprintMultiplier = 2.0f;

    public GameObject head;
    public Socket handDominant;
    public Socket handSubordinate;

    public GameObject subordinateStarterItem;

    public float look_xSensitivity = 2.0f;
    public float look_ySensitivity = 2.0f;
    public float look_maxVerticalRotation = -90.0f;
    public float look_minVerticalRotation = 90.0f;

    public float HeadBobY;
    public float HeadBobDelta = 0; 
    public float HeadBobAmplitude = .5f;
    public float HeadBobSpeed = 0; 
    public float distanceTraveled = 0.0f;
    public float HeadBobValue = 5.0f;
    public bool PerformHeadBob = false;
    int BobDirection = 1;
    float BobAugment = 1.0f;
    float BobSpringAugment = 1.0f;
    float BobWalkAugment = 1.25f; 

    public float interactRange = 2.0f;
    public float interactSensitivity = 0.1f;

    public float crouchSpeed = 0.2f;

    public bool hasKey = false;

    public AudioSource footSteps;
    #endregion

    #region Pawn Member Variables
    protected Rigidbody _rb;
    protected GameObject _highlightedObject;
    protected CapsuleCollider _col;
    protected ImageEffectManager _iem;

    protected bool _lighterActive = false;
    protected bool _isCrouching = false;
    protected bool _isSprinting = false;
    protected bool _cursorIsLocked = true;

    protected float _forwardVelocity = 1.0f;
    protected float _strafeVelocity = 1.0f;
    protected float _speedMultiplier = 1.0f;

    protected float _playerHeight;
    protected float _playerInitialScale;
    protected float _crouchPercent = 0.0f;

    protected float _maxHealth;
    protected float _timeSinceDamaged = 7.0f;

    protected ModifierTable _fovMultipliers;

    protected int[] _fovKeys; //[0] = modifier given by crouching, [1] = modifier given by sprinting

    protected float _inputXRotation = 0.0f;
    protected float _inputYRotation = 0.0f;

    protected Quaternion _desiredBodyRotation;
    protected Quaternion _desiredCameraRotation;
    #endregion

    protected virtual void Start()
    {
        IsSpectator = false;
        IgnoresDamage = false;
        LogDamageEvents = false;

        HeadBobY = head.transform.localPosition.y;
        _iem = head.GetComponent<ImageEffectManager>();
        if(!_iem) { LOG("No ImageEffectManager found on head."); }

        footSteps = gameObject.GetComponent<AudioSource>(); //gets audio
        if(!footSteps) { LOG_ERROR("FPS_Pawn of \"" + name + "\" has no audio source!"); }

        _fovMultipliers = new ModifierTable();
        _fovKeys = new int[2];
        for (int i = 0; i < _fovKeys.Length; i++)
        {
            _fovKeys[i] = -1;
        }

        _playerInitialScale = transform.localScale.y;

        _rb = gameObject.AddComponent<Rigidbody>();
        _rb.freezeRotation = true;

        _desiredBodyRotation = transform.rotation;
        if (!head)
        {
            LOG_ERROR("No head object assigned to " + name);
        }
        _desiredCameraRotation = Quaternion.Euler(Vector3.zero);

        _col = gameObject.GetComponentInChildren<CapsuleCollider>();
        _playerHeight = _col.height;

        _maxHealth = Health;

        SpawnOffhandItem();

        SetCursorLock(_cursorIsLocked);
    }

    #region Update Functions
    protected virtual void Update()
    {
        ManageFOV();

        if (CheckIfDead())
        {
            // Check for Head bob
            if (_rb.velocity.magnitude > 0)
            {
                //LOG("Footsteps playing");
                //Head bob
                distanceTraveled++;
                distanceTraveled *= _rb.velocity.magnitude / 3;

                if (distanceTraveled >= HeadBobValue)
                {
                     
                    if (!PerformHeadBob)
                    {
                        HeadBobSpeed = distanceTraveled / HeadBobValue;
                        Debug.Log("distanceTraveled:" + distanceTraveled); 
                        PerformHeadBob = true;
                    }
                    distanceTraveled = 0;

                }

            }

            // Do Head Bob
            if (PerformHeadBob)
            {
                if (_isSprinting) { BobAugment = BobSpringAugment; }
                else { BobAugment = BobWalkAugment; }

                HeadBobDelta += Time.deltaTime * HeadBobSpeed * BobDirection * BobAugment;
                //HeadBobDelta += Time.deltaTime * BobDirection;

                if (HeadBobDelta >= HeadBobAmplitude)
                {
                    HeadBobDelta = HeadBobAmplitude;
                    BobDirection = -1; // head down 
                }

                if (HeadBobDelta <= 0)
                {
                    HeadBobDelta = 0;
                    BobDirection = 1; // head down 
                    PerformHeadBob = false;
                }


                head.transform.localPosition =
                       new Vector3(head.transform.localPosition.x, HeadBobY + HeadBobDelta, head.transform.localPosition.z);

            }
        }
    }

    protected virtual void FixedUpdate()
    {

        if (CheckIfDead())
        {
            _rb.velocity = GetMoveVelocity();

            if (_rb.velocity.magnitude > 0)
            {
                footSteps.pitch = _rb.velocity.magnitude / 3; //adjusts pitch based on velocity
                if (!footSteps.isPlaying)
                {
                    footSteps.Play();
                }
            }
            else
            {
                //LOG("Footsteps  NOT playing");

                footSteps.Stop();
            }

            ManageDamageEffect();
            HandleCrouching();
            HandleLookRotation();
        }
    }
    #endregion

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

    //Uses the item being held in the dominant hand
    public virtual void Fire1(bool value)
    {
        if (value)
        {
            if (_lighterActive)
            {
                Lighter_Item lighter = (Lighter_Item)handSubordinate.EquippedItem;
                if (lighter)
                {
                    lighter.Ignite(this);
                }
            }
            else
            {
                handDominant.UseItem(this);
            }
        }
    }

    //Uses the item being held in the subordinate hand
    public virtual void Fire2(bool value)
    {
        if (value)
        {
            _lighterActive = handSubordinate.UseItem(this);
        }
    }

    //Crouches the player when held
    public virtual void Fire3(bool value)
    {
        if (_isCrouching && !value)
        {
            Vector3 p1 = _col.transform.position;
            Vector3 p2 = p1 + (Vector3.up * _playerHeight * 0.524f);
            float checkRadius = _col.radius * 0.9f;

            int layermask = 1 << LayerMask.NameToLayer("Player");
            layermask = ~layermask;

            bool didCollide = Physics.CheckCapsule(p1, p2, checkRadius, layermask, QueryTriggerInteraction.Ignore);
            if (!didCollide)
            {
                _fovMultipliers.Remove(this, _fovKeys[0]);
                _isCrouching = false;
            }
        }
        else if (value)
        {
            if (!_fovMultipliers.KeyIsActive(_fovKeys[0]))
            {
                _fovKeys[0] = _fovMultipliers.Add(0.8f, this);
            }
            _isCrouching = true;
        }
    }

    //Interacts with the object the player is looking at in the world
    public virtual void Fire4(bool value)
    {
        if (value)
        {
            GameObject highlighted = GetInteractableObject();
            if (highlighted)
            {
                Interactable other = highlighted.GetComponentInChildren<Interactable>();
                if (other)
                {
                    other.InteractWith(this, controller);
                }
                else
                {
                    Equip(null);
                }
            }
            else
            {
                Equip(null);
            }
        }
    }

    //Allows the player to sprint when held
    public virtual void Fire5(bool value)
    {
        if (value && allowSprint && !_isCrouching)
        {
            _isSprinting = true;
        }
        else
        {
            _isSprinting = false;
        }
    }
    #endregion

    #region Movement Related Methods



    protected virtual Vector3 GetMoveVelocity() //Known issue: moving diagonally is faster than moving on other axes.
    {
        Vector3 moveVelocity = new Vector3(0.0f, 0.0f, 0.0f);

        //Sprint only applies in the forward direction
        if (_isSprinting && _forwardVelocity > 0.0f)
        {
            _forwardVelocity *= sprintMultiplier;
            if (!_fovMultipliers.KeyIsActive(_fovKeys[1]))
            {
                _fovKeys[1] = _fovMultipliers.Add(1.2f, this);
            }
        }
        else
        {
            _fovMultipliers.Remove(this, _fovKeys[1]);
        }

        moveVelocity += transform.forward * _forwardVelocity + transform.right * _strafeVelocity;

        moveVelocity *= moveSpeed * _speedMultiplier;

        moveVelocity.y += _rb.velocity.y;

        return moveVelocity;
    }

    protected virtual void HandleCrouching()
    {
        float playerHeightScale = Mathf.Lerp(_playerInitialScale, _playerInitialScale * 0.5f, _crouchPercent);
        transform.localScale = new Vector3(1.0f, playerHeightScale, 1.0f);

        if (_isCrouching && _crouchPercent < 1.0f)
        {
            _crouchPercent += Time.fixedDeltaTime * crouchSpeed;
        }
        else if (!_isCrouching && _crouchPercent > 0.0f)
        {
            _crouchPercent -= Time.fixedDeltaTime * crouchSpeed;
        }
    }
    #endregion

    #region Mouselook
    protected virtual void HandleLookRotation()
    {
        if (!_cursorIsLocked)
        {
            //return;
        }
        _desiredBodyRotation *= Quaternion.Euler(0.0f, _inputYRotation, 0.0f);
        _desiredCameraRotation *= Quaternion.Euler(-_inputXRotation, 0.0f, 0.0f);

        _desiredCameraRotation = ClampRotationAroundX(_desiredCameraRotation);

        transform.localRotation = _desiredBodyRotation;
        head.transform.localRotation = _desiredCameraRotation;
    }

    protected virtual Quaternion ClampRotationAroundX(Quaternion quatIn)
    {
        Quaternion q = quatIn;

        q.x /= q.w;
        q.y /= q.w;
        q.z /= q.w;
        q.w = 1.0f;

        float angleX = 2.0f * Mathf.Rad2Deg * Mathf.Atan(q.x);

        angleX = Mathf.Clamp(angleX, look_minVerticalRotation, look_maxVerticalRotation);

        q.x = Mathf.Tan(0.5f * Mathf.Deg2Rad * angleX);

        //Check for bad values:
        if (float.IsNaN(q.x) || float.IsNaN(q.y) || float.IsNaN(q.z))
        {
            return quatIn;
        }

        return q;
    }

    public virtual void SetCursorLock(bool newLockState)
    {
        _cursorIsLocked = newLockState;
        if (newLockState)
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

    protected virtual void ManageFOV()
    {
        float overallFovModifier = _fovMultipliers.Product();

        Camera playerCamera = head.GetComponent<Camera>();
        if (playerCamera)
        {
            float newFOV = Mathf.Lerp(playerCamera.fieldOfView, defaultFOV * overallFovModifier, 0.2f);
            playerCamera.fieldOfView = newFOV;
        }
    }

    public virtual bool Equip(Item item)
    {
        if (!handDominant)
        {
            return false;
        }

        if (handDominant.HasItem)
        {
            handDominant.Unequip();
        }
        if (!item)
        {
            return true;
        }
        return handDominant.Equip(item);
    }

    public virtual GameObject GetInteractableObject()
    {
        //Raycast to determine what the player is looking at
        /*int layermask = 1 << LayerMask.NameToLayer("Player");
        layermask = ~layermask;

        RaycastHit hitInfo;
        Physics.Raycast(head.transform.position, head.transform.forward, out hitInfo, interactRange, layermask, QueryTriggerInteraction.Ignore);

        if(hitInfo.collider)
        {
            return hitInfo.collider.gameObject;
        }
        else
        {
            return null;
        }*/

        //Spherecast to determine what the player is looking at
        int layermask = 1 << LayerMask.NameToLayer("Player");
        layermask = ~layermask;

        RaycastHit hitInfo;
        Physics.SphereCast(head.transform.position, interactSensitivity, head.transform.forward, out hitInfo, interactRange, layermask, QueryTriggerInteraction.Ignore);

        if (hitInfo.collider)
        {
            return hitInfo.collider.gameObject;
        }
        else
        {
            return null;
        }
    }

    protected virtual void SpawnOffhandItem()
    {
        if (!subordinateStarterItem) { return; }

        GameObject spawnedGameObject = Factory(subordinateStarterItem, handSubordinate.transform.position, handSubordinate.transform.rotation);
        Item spawnedItem = spawnedGameObject.GetComponent<Item>();
        if (!spawnedItem)
        {
            Destroy(spawnedGameObject);
            return;
        }
        handSubordinate.Equip(spawnedItem);
    }

    #region Health and Dying
    protected override bool ProcessDamage(Actor Source, float Value, DamageEventInfo EventInfo, Controller Instigator)
    {
        Health -= Value;
        _timeSinceDamaged = 0.0f;

        return CheckIfDead();
    }

    //Returns true if alive, false if dead.
    protected virtual bool CheckIfDead()
    {
        if (Health <= 0)
        {
            Die();
            return false;
        }
        return true;
    }

    protected virtual void Die()
    {
        if (!_controller) { return; }

        FPS_Controller FPC = (FPS_Controller)_controller;
        if (FPC)
        {
            FPC.PawnHasDied();
        }
        else if (_controller)
        {
            _controller.UnPossesPawn(this);
        }
        //Do something to the camera image effect
        handDominant.Unequip();
        handSubordinate.Unequip();
        _rb.freezeRotation = false;
    }

    public override void OnUnPossession()
    {
        //SetCursorLock(false);
        IgnoresDamage = true;

        if (Health > 0)
        {
            _inputXRotation = 0.0f;
            _inputYRotation = 0.0f;
            _strafeVelocity = 0.0f;
            _forwardVelocity = 0.0f;
        }

        //Destroy(gameObject, 5.0f);
    }

    protected virtual void ManageDamageEffect()
    {
        _timeSinceDamaged += Time.fixedDeltaTime;

        if (!_iem) { return; }

        float baseBloodiness = MaxHealthBloodiness * (1.0f - (Health / _maxHealth));

        if(_timeSinceDamaged <= MaxBloodyTime)
        {
            _iem.targetBloodiness = Mathf.Lerp(1.0f - 0.5f * MaxHealthBloodiness, baseBloodiness, _timeSinceDamaged / MaxBloodyTime);
        }
        else
        {
            _iem.targetBloodiness = baseBloodiness;
        }
    }
    #endregion
}