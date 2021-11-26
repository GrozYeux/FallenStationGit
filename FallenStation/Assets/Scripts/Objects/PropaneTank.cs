using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PropaneTank : CharacterStats
{
    [SerializeField]
    GameObject particleEffect;
    [SerializeField]
    GameObject leakEffect;
    [SerializeField]
    GameObject body;
    [SerializeField]
    AudioSource shootSoundSource;
    [SerializeField]
    LayerMask explosionLayerMask;
    int nbShoots = 0;
    bool shot = false;
    bool dead = false;
    bool isGrounded;

    public float maxDamage = 200f;
    public float fallDistance;
    float lastPosition;

    public void Start()
    {
        particleEffect.SetActive(false);
        leakEffect.SetActive(false);
    }
    public override void Die()
    {
        dead = true;
        particleEffect.SetActive(true);
        particleEffect.GetComponent<ParticleSystem>().Play();
        GetComponent<Collider>().enabled = false;
        body.SetActive(false);
        leakEffect.SetActive(false);

        //Check de collision
        Collider[] collisions = Physics.OverlapSphere(transform.position, 4f);
        foreach(Collider hit in collisions)
        {
            //Applique dégats si dans le masque de collision
            if( (explosionLayerMask & (1 << hit.gameObject.layer)) != 0)
            {
                CharacterStats hitObj = hit.gameObject.GetComponent<CharacterStats>();
                if (hitObj != null)
                {
                    bool canDamage = true;
                    PropaneTank p = hitObj.gameObject.GetComponent<PropaneTank>();
                    if (p != null) //Propane tanks
                        canDamage = !(p.dead);

                    if (canDamage) { 
                        Debug.Log("HitObj ok : " + hit.gameObject.name);
                        float distance = (hitObj.transform.position - transform.position).sqrMagnitude;
                        Debug.Log("distance = " + distance);
                        hitObj.TakeDamage(maxDamage / (distance+1) );
                        Debug.Log("Damage given  : " + maxDamage / (distance + 1) + " to " + hitObj.name.ToString());
                    }
                }
            }
            
            //Applique force à tous les objets
            Rigidbody body = hit.attachedRigidbody;
            if(body != null)
                body.AddExplosionForce(500f, transform.position, 8);
        }

        //Suppression de l'objet
        Destroy(gameObject, 3.0f);
    }

    protected override void Hurt(float newAlpha)
    {
        shootSoundSource.pitch = 1.0f + 0.25f*nbShoots;
        if(nbShoots < 1)
        {
            leakEffect.SetActive(true);
        }
        shootSoundSource.Play();
        nbShoots++;
        shot = true;
    }

    private void Update()
    {
        fallDamage();

        if (shot && !dead)
        {
            currentHealth -= Time.deltaTime * (maxHealth/5);
            if (currentHealth <= 0)
            {
                Die();
            }
        }
    }

    protected override void Awake()
    {
        base.Awake();
    }

    
    bool IsGrounded()
    {
        if(Physics.Raycast(transform.position, Vector3.down, GetComponent<Collider>().bounds.extents.y))
        {
            isGrounded = true;
            
        } else
        {
            isGrounded = false;
        }
        return isGrounded;
    }
    void fallDamage()
    {
        if (lastPosition > gameObject.transform.position.y)
        {
            fallDistance += lastPosition - gameObject.transform.position.y;
        }
        lastPosition = gameObject.transform.position.y;
        if (fallDistance > 7 && IsGrounded())
        {
            TakeDamage(maxHealth);
            fallDistance = lastPosition = 0;
        } else if (fallDistance <= 7 && fallDistance >= 4 && IsGrounded())
        {
            TakeDamage(maxHealth / 2);
            fallDistance = lastPosition = 0;
        }
    }
}