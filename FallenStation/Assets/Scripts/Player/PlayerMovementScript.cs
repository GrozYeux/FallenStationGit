using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementScript : MonoBehaviour
{
    public CharacterController controller;

    public float speed = 6f;
    public float sprintSpeed = 10f;
    public float crouchSpeed = 4f;
    public float gravity = -15f;
    public float jumpHeight = 1f;

    //------------------------------------

    private Vector3 newPosition = Vector3.zero;
    private Vector3 velocity;
    private bool isGrounded;
    private bool isGravityOn = true;
    private float actualSpeed;
    private float defaultStepOffset;

    //------------------------------------

    public float crouchHeight = 0.8f;
    public float standingHeight = 2f;
    public float timeToCrouch = 0.25f;
    public Vector3 crouchingCenter = new Vector3(0f, 1.4f, 0);
    public Vector3 standinggCenter = new Vector3(0f, 1f, 0);
    private bool isCrouching;
    private bool isDuringCrouchAnimation;

    //------------------------------------

    private void Start()
    {
        this.defaultStepOffset = this.controller.stepOffset;


        //chargement donn�es
        if (MenuScript.load)
        {
            LoadPlayer();
        }
        
    }

    void Update()
    {
        
        // Sur le sol
        if (controller.isGrounded)
        {
            // Reset le stepoffset
            this.controller.stepOffset = this.defaultStepOffset;

            // Si on est pas entrain de commencer un saut
            if (this.velocity.y < 0) 
            {
                // Colle le joueur au sol
                this.velocity.y = -1f;
            }

            // Change vitesse si on appuie sur Shift ou non
            if (Input.GetButton("Sprint")) 
            {
                this.actualSpeed = this.sprintSpeed;
            }
            else
            {
                this.actualSpeed = this.speed;
            }

            if (Input.GetButtonDown("Crouch") && !isDuringCrouchAnimation && !isCrouching)
            {
                StartCoroutine(CrouchStand());
            }
            if (Input.GetButtonUp("Crouch") && !isDuringCrouchAnimation && isCrouching)
            {
                StartCoroutine(CrouchStand());
            }

        }
        // En l'air
        else
        {
            // Stepoffset � 0 pour pas essayer de monter sur les rebords des murs
            this.controller.stepOffset = 0f;
        }

        // Combine les valeurs d'input en une direction par rapport au joueur
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");
        Vector3 inputMoveVector = this.transform.right * x + this.transform.forward * z;

        // Normalise le vecteur de direction si plus long que 1 pour pas bouger plus vite en diagonale
        if (inputMoveVector.sqrMagnitude > 1f)
        {
            inputMoveVector.Normalize();
        }

        // Ajout mouvement horizontal au vecteur velocity
        this.velocity.x = inputMoveVector.x * actualSpeed;
        this.velocity.z = inputMoveVector.z * actualSpeed;

        // Saut
        if (Input.GetButtonDown("Jump") && controller.isGrounded)
        {
            this.velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
            this.controller.stepOffset = 0f;
        }

        // Ajoute gravity si active
        if (this.isGravityOn)
        {
            this.velocity.y += this.gravity * Time.deltaTime;
        }

        // Application du Mouvement 
        this.controller.Move(this.velocity * Time.deltaTime);

    }

    private IEnumerator CrouchStand()
    {
        if(isCrouching && Physics.Raycast(transform.position, Vector3.up, 2))
        {
            yield break;
        }

        isDuringCrouchAnimation = true;

        float timeElapsed = 0;
        float targetHeight = isCrouching ? standingHeight : crouchHeight;
        float currentHeight = controller.height;
        Vector3 targetCenter = isCrouching ? standinggCenter : crouchingCenter;
        Vector3 currentCenter = controller.center;

        while(timeElapsed < timeToCrouch)
        {
            controller.height = Mathf.Lerp(currentHeight, targetHeight, timeElapsed / timeToCrouch);
            controller.center = Vector3.Lerp(currentCenter, targetCenter, timeElapsed / timeToCrouch);
            timeElapsed += Time.deltaTime;
            yield return null;
        }
        controller.height = targetHeight;
        controller.center = targetCenter;

        isCrouching = !isCrouching;
        actualSpeed = isCrouching ? crouchSpeed : speed;

        isDuringCrouchAnimation = false;
    }


    public   void LoadPlayer ()
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
        speed = data.speed;
        sprintSpeed = data.sprintSpeed;
        gravity = data.gravity;
        jumpHeight = data.jumpHeight;

        
        newPosition.x = data.position[0];
        newPosition.y = data.position[1];
        newPosition.z = data.position[2];
        print(newPosition);

       

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
        if(newPosition != Vector3.zero)
        {
            controller.enabled = false;
            controller.transform.position = newPosition;
            controller.enabled = true;
            newPosition = Vector3.zero;
            Debug.Log(transform.position);
        }
    }
}
