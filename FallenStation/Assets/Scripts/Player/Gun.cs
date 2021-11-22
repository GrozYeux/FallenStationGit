using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Gun : MonoBehaviour
{
    [SerializeField] Material color = null;
    public int gunDamage = 1;
    public float range = 200f;
    public float force = 100f;
    public float fireRate = 0.15f;
    public LayerMask Ennemylayer;
    public LayerMask interactionLayer;
    private bool fire = false;
    GameObject lastHit;
    RaycastHit hit;

    [SerializeField] Camera cam;
    [SerializeField] ParticleSystem muzzleFlash;

    private bool interaction = false;
    public float pickUpDistance = 3.0f;
    private int munitions;
    public int chargerCapacity = 60;
    public bool canFire = true;

    private float fireDelta = 0f;
    [SerializeField] private GameObject movingPart;

    TextManager tm;

    // Start is called before the first frame update
    void Start()
    {
        Collectables.Instance.AddAmoClip(5);
        fireDelta = fireRate;
        munitions = chargerCapacity;
    }

    void Update()
    {
        if (Input.GetButton("Fire1") && canFire)
        {
            if(munitions > 0){
                fire = true;
                if(!muzzleFlash.isPlaying)
                    muzzleFlash.Play();
                if (fireDelta >= fireRate )
                {
                    SoundManager.Instance.PlayRandomSound(SoundManager.Instance.shootClips);
                    munitions -= 1;
                    fireDelta = 0f;
                    muzzleFlash.Play();
                    FireForward();
                }
                movingPart.transform.position = transform.position - movingPart.transform.forward * fireDelta;
                fireDelta += Time.deltaTime;
            } else {
                SoundManager.Instance.PlayRandomSound(SoundManager.Instance.noAmmoShootClips);
            }
            
        }
        else
        {
            fire = false;
            if (muzzleFlash.isPlaying)
                muzzleFlash.Stop();
        }

        if (!interaction)
        {
            interaction = Input.GetButtonDown("Interaction");
        }

        //Modification : reload manuel
        if(((Input.GetButtonDown("Reload") && munitions != chargerCapacity)) && Collectables.Instance.HaveAmoClip())
        {
            StartCoroutine(Reload());
        }
    }

    private void FixedUpdate()
    {
        //Vérifications passives

        //Crée un vecteur au centre de la vue de la caméra
        Vector3 rayOrigin = cam.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 0.0f));

        // Vérifie si le raycast a touché un des layers : collectable et enemy
        if (!fire)
        {
            if (Physics.Raycast(rayOrigin, cam.transform.forward, out hit, range, interactionLayer))
            {
                GameObject objHit = hit.collider.gameObject;

                // Remet la couleur du collectable par défaut si on ne vise plus l'objet..
                if ((lastHit != null) && (lastHit != hit.collider.gameObject) && (lastHit.layer == LayerMask.NameToLayer("Collectable") || lastHit.layer == LayerMask.NameToLayer("Puzzle")))
                {
                    lastHit.GetComponent<HighLight>().OnRayCastExit();
                }
                // ..ou si l'on est trop loin de celui-ci
                if ((objHit.layer == LayerMask.NameToLayer("Collectable") || objHit.layer == LayerMask.NameToLayer("Puzzle")) && hit.distance > pickUpDistance)
                {
                    objHit.GetComponent<HighLight>().OnRayCastExit();
                }

                // Vérifie si la cible est un collectable
                if (objHit.CompareTag("access") || objHit.CompareTag("codex") || objHit.CompareTag("amoClip") || objHit.CompareTag("puzzle") || objHit.CompareTag("button") || objHit.CompareTag("lever"))
                {
                    // Vérifie que l'on ne soit pas trop éloigné
                    if (hit.distance < pickUpDistance)
                    {
                        //Change material de l'objets
                        Renderer rend = objHit.GetComponent<Renderer>();
                        rend.material.color = color.color;

                        // Ramasse l'objet si on a utilisé la touche d'interaction
                        if (interaction)
                        {
                            GameObject[] collectables; //tableau dans lequel on mettra les objets a destroy (dont les doublons)
                            if (objHit.CompareTag("access")) //carte dacces
                            {
                                SoundManager.Instance.PlayRandomSound(SoundManager.Instance.pickupClips);
                                Collectables.Instance.AddObject(objHit.name);
                                UITextManager.Instance.PrintText("Item " + objHit.name + " collecté");
                                collectables = GameObject.FindGameObjectsWithTag("access");
                            }
                            else if (objHit.CompareTag("amoClip")) //chargeur
                            {
                                SoundManager.Instance.PlayRandomSound(SoundManager.Instance.pickupClips);
                                Collectables.Instance.AddAmoClip(1);
                                UITextManager.Instance.PrintText("1 chargeur collecté");
                                collectables = GameObject.FindGameObjectsWithTag("amoClip");
                            }
                            else if (objHit.CompareTag("puzzle")) //puzzle
                            {
                                objHit.GetComponent<Puzzle>().Action();
                                collectables = GameObject.FindGameObjectsWithTag("amoClip");
                            }
                            else if (objHit.CompareTag("lever")) //puzzle
                            {
                                objHit.GetComponent<LeverScript>().leverNumber();
                                collectables = GameObject.FindGameObjectsWithTag("amoClip");
                            }
                            else if (objHit.CompareTag("button")) //boutton
                            {
                                UITextManager.Instance.PrintText("tourelles désactivées");
                                objHit.GetComponent<DisarmeTurret>().OnClick();
                                collectables = new GameObject[0];
                            }
                            else // note du codex
                            {
                                Collectables.Instance.AddNote(objHit.name);
                                UITextManager.Instance.PrintText("Nouvelle entrée dans le Codex : " + objHit.name);
                                collectables = GameObject.FindGameObjectsWithTag("codex");
                                UINote.canvasNote.SetActive(true);
                                UINote.Pause();
                                tm = GameObject.Find("CanvasNote").GetComponent<TextManager>();
                                tm.DisplayNote(objHit.name);
                                SaveSystem.SaveCodex(Collectables.Instance);
                            }

                            // Supprime le collectable et les doublons si il y en a
                            foreach (GameObject obj in collectables)
                            {
                                if (obj.name == objHit.name)
                                {
                                    Destroy(obj);
                                }

                            }
                        }
                    }

                }

                // Vérifie si la cible est une porte
                if (objHit.CompareTag("door") && hit.distance < pickUpDistance)
                {
                    // Tente d'ouvrir la porte si on a utilisé la touche d'interaction
                    if (interaction)
                    {
                        objHit.GetComponent<Door>().Open();

                        if (objHit.TryGetComponent(out Sas sas))
                        {
                            Sas.Save(this.GetComponentInParent<PlayerMovementScript>(), Collectables.Instance);
                            print(sas.name);
                            if (sas.name == "PorteSas")
                            {
                                Sas.nextLevel();
                            }
                        }
                    }
                }

                lastHit = objHit;
            }
            else
            {
                // Si on a rien touché et que l'ancien objet touché était un collectable, remet son material par défaut
                if (lastHit != null && (lastHit.layer == LayerMask.NameToLayer("Collectable") || lastHit.layer == LayerMask.NameToLayer("Puzzle")))
                {
                    lastHit.GetComponent<HighLight>().OnRayCastExit();
                    lastHit = null;
                }
            }
        }
        interaction = false;
    }

    //GESTION DE L'ATTAQUE
    void FireForward()
    {
        //Crée un vecteur au centre de la vue de la caméra
        Vector3 rayOrigin = cam.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 0.0f));
        // Vérifie si le raycast a touché un des layers : collectable et enemy
        if (Physics.Raycast(rayOrigin, cam.transform.forward, out hit, range, Ennemylayer))
        {
            GameObject objHit = hit.collider.gameObject;
            print("Player shoot");


            // Vérifie si la cible a un RigidBody attaché
            if (hit.rigidbody != null)
            {   //AddForce = Ajoute Force = Pousse le RigidBody avec la force de l'impact
                hit.rigidbody.AddForce(-hit.normal * force);
            }
            CharacterStats ennemyStats = objHit.GetComponent<CharacterStats>();
            if (ennemyStats != null)
            { //S'assure que la cible touchée peut être blessée
                CharacterCombat playerCombat = GetComponent<CharacterCombat>();
                //Envoie les dommages à la cible
                playerCombat.Attack(ennemyStats);

            }
            //Sinon, s'assure que la cible touchée est un boss (à modifier)
            else if (objHit.GetComponent<Boss>() != null)
            {
                Boss boss = objHit.GetComponent<Boss>();
                //Envoie les dommages à la cible
                boss.TakeDamage(gunDamage);

            }
        }
    }

    private IEnumerator Reload()
    {
        Debug.Log("Reload");
        canFire = false;
        SoundManager.Instance.PlayRandomSound(SoundManager.Instance.removeGunClips);
        Collectables.Instance.AddAmoClip(-1);
        yield return new WaitForSeconds(1f);
        SoundManager.Instance.PlayRandomSound(SoundManager.Instance.addGunClips);
        yield return new WaitForSeconds(0.2f);
        fireDelta = fireRate;
        munitions = chargerCapacity;
        canFire = true;
    }
}
