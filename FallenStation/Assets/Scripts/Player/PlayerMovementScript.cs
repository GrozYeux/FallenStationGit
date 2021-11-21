using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementScript : MonoBehaviour
{
    public bool canMove { get; private set; } = true;
    [SerializeField] public bool isFlying = false;
    private bool shouldSprint => canSprint && Input.GetButton("Sprint") && !isZoomedIn;
    private bool shouldJump => characterController.isGrounded && !isFlying && Input.GetButtonDown("Jump");
    private bool shouldCrouch => characterController.isGrounded && !isDuringCrouchAnimation && !isFlying && Input.GetButtonDown("Crouch");
    private bool shouldGoUp => isFlying && Input.GetButton("Jump");
    private bool shouldGoDown => isFlying && Input.GetButton("Crouch");

    [Header("Functional Options")]
    [SerializeField] private bool canSprint = true;
    [SerializeField] private bool canJump = true;
    [SerializeField] private bool canCrouch = true;
    [SerializeField] private bool canHeadBob = true;
    [SerializeField] private bool canZoom = true;
    [SerializeField] private bool footStepsSound = true;

    [Header("Movement Parameters")]
    [SerializeField] private float walkSpeed = 6.0f;
    [SerializeField] private float sprintSpeed = 10.0f;
    [SerializeField] private float crouchSpeed = 3.0f;
    private Vector2 currentInput;
    private Vector3 velocity;
    private Vector3 loadPosition = Vector3.zero;

    [Header("Jumping Parameters")]
    [SerializeField] private float jumpHeight = 1.0f;
    [SerializeField] private float gravity = 25.0f;
    [SerializeField] private float defaultStepOffset = 0.5f;

    [Header("Crouch Parameters")]
    [SerializeField] private float crouchingHeight = 1f;
    [SerializeField] private float standingHeight = 2f;
    [SerializeField] private float timeToCrouch = 0.1f;
    [SerializeField] private Vector3 crouchingCameraHeight = new Vector3(0f, 0.9f, 0);
    [SerializeField] private Vector3 standingCameraHeight = new Vector3(0f, 1.8f, 0);
    [SerializeField] private Vector3 crouchingCenter = new Vector3(0f, 0.5f, 0);
    [SerializeField] private Vector3 standingCenter = new Vector3(0f, 1f, 0);
    private bool isCrouching;
    private bool isDuringCrouchAnimation;

    [Header("HeadBob Parameters")]
    [SerializeField] private float walkBobSpeed = 14f;
    [SerializeField] private float walkBobAmount = 0.08f;
    [SerializeField] private float sprintBobSpeed = 18f;
    [SerializeField] private float sprintBobAmount = 0.16f;
    [SerializeField] private float crouchBobSpeed = 8f;
    [SerializeField] private float crouchBobAmount = 0.05f;
    private float bobTimer;

    [Header("Zoom Parameters")]
    [SerializeField] private float timeToZoom = 0.15f;
    [SerializeField] private float zoomFOV = 30f;
    private bool isZoomedIn = false;
    private float defaultFOV;
    private Coroutine zoomRoutine;

    [Header("FootSteps Parameters")]
    [SerializeField] private float baseStepSpeed = 0.5f;
    [SerializeField] private float crouchStepMultiplier = 1.5f;
    [SerializeField] private float sprintStepMultiplier = 0.6f;
    private float footStepTimer = 0;
    private float GetCurrentOffsetFootSteps => isCrouching ? baseStepSpeed * crouchStepMultiplier : shouldSprint ? baseStepSpeed * sprintStepMultiplier : baseStepSpeed;

    // Components needed
    private Camera playerCamera;
    private CharacterController characterController;

    

    private void Awake()
    {
        playerCamera = GetComponentInChildren<Camera>();
        characterController = GetComponent<CharacterController>();

        defaultFOV = playerCamera.fieldOfView;
        LoadPlayer();
    }

    private void Update()
    {
        if (canMove)
        {
            HandleMovementInput();

            if (canJump)
                HandleJump();

            if (canCrouch)
                HandleCrouch();

            if (canHeadBob && !isFlying)
                HandleHeadBob();

            if (canZoom)
                HandleZoom();

            if (footStepsSound)
                HandleFootSteps();

            ApplyFinalMovements();
        }
    }

    private void HandleMovementInput()
    {
        currentInput = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        if (currentInput.sqrMagnitude > 1) currentInput.Normalize();


        if(!isFlying)
        {
            float velocityY = velocity.y;
            velocity = (transform.TransformDirection(Vector3.right) * currentInput.x) + (transform.TransformDirection(Vector3.forward) * currentInput.y);
            velocity *= (isCrouching || isZoomedIn ? crouchSpeed : shouldSprint ? sprintSpeed : walkSpeed);
            velocity.y = velocityY;
        }
        else
        {
            velocity.y = 0;
            velocity = (transform.TransformDirection(Vector3.right) * currentInput.x) + (playerCamera.transform.TransformDirection(Vector3.forward) * currentInput.y) + ((transform.TransformDirection(Vector3.up)*(shouldGoUp ? 1 : shouldGoDown ? -1 : 0)));
            velocity *= (shouldSprint ? sprintSpeed : walkSpeed);
        }
          
    }

    private void HandleJump()
    {
        if(shouldJump && !isFlying)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * 2f * gravity);
            characterController.stepOffset = 0;
        }
    }

    private void HandleCrouch()
    {
        if (shouldCrouch && !isFlying)
        {
            StartCoroutine(CrouchStand());
        }
    }

    private IEnumerator CrouchStand()
    {
        if (isCrouching && Physics.Raycast(new Vector3(transform.position.x, transform.position.y+0.5f, transform.position.z), Vector3.up, 1.5f))
        {
            yield break;
        }

        isDuringCrouchAnimation = true;

        float timeElapsed = 0;
        float targetHeight = isCrouching ? standingHeight : crouchingHeight;
        float currentHeight = characterController.height;
        Vector3 targetCameraHeight = isCrouching ? standingCameraHeight : crouchingCameraHeight;
        Vector3 currentCameraHeight = playerCamera.transform.localPosition;
        Vector3 targetCenter = isCrouching ? standingCenter : crouchingCenter;
        Vector3 currentCenter = characterController.center;

        while (timeElapsed < timeToCrouch)
        {
            characterController.height = Mathf.Lerp(currentHeight, targetHeight, timeElapsed / timeToCrouch);
            characterController.center = Vector3.Lerp(currentCenter, targetCenter, timeElapsed / timeToCrouch);
            playerCamera.transform.localPosition = Vector3.Lerp(currentCameraHeight, targetCameraHeight, timeElapsed / timeToCrouch);
            timeElapsed += Time.deltaTime;
            yield return null;
        }
        characterController.height = targetHeight;
        characterController.center = targetCenter;
        playerCamera.transform.localPosition = targetCameraHeight;

        isCrouching = !isCrouching;

        isDuringCrouchAnimation = false;
    }

    private void HandleHeadBob()
    {
        if (characterController.isGrounded)
        {
            if(Mathf.Abs(velocity.x) > 0.1f || Mathf.Abs(velocity.z) > 0.1f)
            {
                bobTimer += Time.deltaTime * (isCrouching || isZoomedIn ? crouchBobSpeed : shouldSprint ? sprintBobSpeed : walkBobSpeed);
                playerCamera.transform.localPosition = new Vector3(
                    playerCamera.transform.localPosition.x,
                    (isCrouching ? crouchingCameraHeight.y : standingCameraHeight.y) + (Mathf.Sin(bobTimer) * (isCrouching || isZoomedIn ? crouchBobAmount : shouldSprint ? sprintBobAmount : walkBobAmount)),
                    playerCamera.transform.localPosition.z);
                if (bobTimer >= 360) bobTimer = 0;
            }
        }

    }

    private void HandleZoom()
    {
        if(Input.GetButtonDown("Fire2"))
        {
            if(zoomRoutine != null)
            {
                StopCoroutine(zoomRoutine);
                zoomRoutine = null;
            }

            zoomRoutine = StartCoroutine(ToggleZoom(true));
            isZoomedIn = true;
        }

        if (Input.GetButtonUp("Fire2"))
        {
            if (zoomRoutine != null)
            {
                StopCoroutine(zoomRoutine);
                zoomRoutine = null;
            }

            zoomRoutine = StartCoroutine(ToggleZoom(false));
            isZoomedIn = false;
        }
    }

    private IEnumerator ToggleZoom(bool isEnter)
    {
        float targetFOV = isEnter ? zoomFOV : defaultFOV;
        float startingFOV = playerCamera.fieldOfView;
        float timeElapsed = 0;

        while (timeElapsed < timeToZoom)
        {
            playerCamera.fieldOfView = Mathf.Lerp(startingFOV, targetFOV, timeElapsed / timeToZoom);
            timeElapsed += Time.deltaTime;
            yield return null;
        }

        playerCamera.fieldOfView = targetFOV;
        zoomRoutine = null;

    }

    private void HandleFootSteps()
    {
        if (currentInput == Vector2.zero) return;
        if(!characterController.isGrounded)
        {
            footStepTimer = 0;
            return;
        }

        footStepTimer -= Time.deltaTime;

        if (footStepTimer <= 0)
        {
            SoundManager.Instance.PlayRandomSound(SoundManager.Instance.footStepAudioClips);
            footStepTimer = GetCurrentOffsetFootSteps;
        }

    }

    private void ApplyFinalMovements()
    {
        if (!characterController.isGrounded && !isFlying) // in air
        {
            velocity.y -= gravity * Time.deltaTime;
        }
        else if (velocity.y < 0 && !isFlying) // grounded not starting a jump
        {
            velocity.y = -1;
            characterController.stepOffset = defaultStepOffset;
        }

        characterController.Move(velocity * Time.deltaTime);
    }


    public void LoadPlayer ()
    {
        PlayerData data;
        print("dans load");
        if (SaveSystem.LoadPlayer() != null)
        {
            print("dans sauvegarde local");
            data = SaveSystem.LoadPlayer();


        } else
        {
            print("dans sauvegarde sas");
            Data AllData = Sas.Load();
            data = AllData.playerData;
        }
        
        loadPosition.x = data.position[0];
        loadPosition.y = data.position[1];
        loadPosition.z = data.position[2];
        Debug.Log(loadPosition);

       

        Quaternion rotation;
        rotation.x = data.rotation[0];
        rotation.y = data.rotation[1];
        rotation.z = data.rotation[2];
        rotation.w = data.rotation[3];
        transform.rotation = rotation;

        MenuScript.load = false;
    }

    private void LateUpdate()
    {
        if(loadPosition != Vector3.zero)
        {
            characterController.enabled = false;
            characterController.transform.position = loadPosition;
            characterController.enabled = true;
            loadPosition = Vector3.zero;
            Debug.Log(transform.position);
        }
    }
}
