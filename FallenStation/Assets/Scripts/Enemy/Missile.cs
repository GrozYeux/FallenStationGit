using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Missile : MonoBehaviour
{
    public bool followPlayer = false;
    public float speed = 10.0f;
    public LayerMask layerMask;

    private Vector3 direction;
    private Quaternion rotation;
    private float rotationSpeed = 2f;

    private float collisionDelay = 0.5f;
    private float collisionDelta = 0.0f;

    void Start()
    {
    }

    private void Explode()
    {
        Destroy(gameObject);
    }

    private void FixedUpdate()
    {
        if (followPlayer)
        {
            GameObject player = GameManager.Instance.GetPlayer();
            direction = (player.transform.position - transform.position).normalized;

            rotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, rotation, rotationSpeed * Time.deltaTime );
        }
        transform.Translate(transform.forward * speed * Time.deltaTime, Space.World);

        //Check for collisions
        if(collisionDelta >= collisionDelay)
        {
            if (Physics.CheckSphere(transform.position, 0.25f, layerMask))
            {
                /*CharacterCombat enemyCombat = GetComponent<CharacterCombat>();

                if (enemyCombat != null)
                {
                    Debug.Log("Touched player !");
                    enemyCombat.Attack(player.GetComponent<CharacterStats>());

                }*/

                Explode();
            }
        }
        else
        {
            collisionDelta++;
        }
    }
}
