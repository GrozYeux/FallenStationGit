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

    public Transform groundCheck;
    public float groundDistance = 0.2f;
    public LayerMask groundMask;

    //------------------------------------

    private Vector3 verticalVelocity;
    private bool isGrounded;
    private bool isGravityOn = true;
    private float actualSpeed;
    private float defaultStepOffset;

    //------------------------------------

    private void Start()
    {
        this.defaultStepOffset = this.controller.stepOffset;
    }

    void Update()
    {
        // Check si sol en dessous
        this.isGrounded = Physics.CheckSphere(this.groundCheck.position, this.groundDistance, this.groundMask);

        // Sur le sol
        if (this.isGrounded)
        {
            // Reset le stepoffset
            this.controller.stepOffset = this.defaultStepOffset;

            // Si on est pas entrain de commencer un saut
            if (this.verticalVelocity.y < 0) 
            {
                // Colle le joueur au sol
                this.verticalVelocity.y = -1f;
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
            // Stepoffset à 0 pour pas essayer de monter sur les rebords des murs
            this.controller.stepOffset = 0f;
        }

        // Combine les valeurs d'input en une direction par rapport au joueur
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");
        Vector3 moveVector = this.transform.right * x + this.transform.forward * z;

        // Normalise le vecteur de direction si plus long que 1 pour pas bouger plus vite en diagonal
        if (moveVector.sqrMagnitude > 1f)
        {
            moveVector.Normalize();
        }

        // Mouvement horizontal
        this.controller.Move(moveVector * actualSpeed * Time.deltaTime);

        // Saut
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            this.verticalVelocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
            this.controller.stepOffset = 0f;
        }

        // Ajoute gravity si active
        if (this.isGravityOn)
        {
            this.verticalVelocity.y += this.gravity * Time.deltaTime;
        }

        // Mouvement vertical
        this.controller.Move(this.verticalVelocity * Time.deltaTime);
    }
}
