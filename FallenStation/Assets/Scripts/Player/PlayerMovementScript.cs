using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementScript : MonoBehaviour
{
    public CharacterController controller;

    public float speed = 6f;
    public float sprintSpeed = 10f;
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
            if (Input.GetKey("left shift")) 
            {
                this.actualSpeed = this.sprintSpeed;
            }
            else
            {
                this.actualSpeed = this.speed;
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
