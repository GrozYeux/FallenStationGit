using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeWarp : MonoBehaviour
{
    public GameObject futureLevel;
    public GameObject pastLevel;

    public bool inPast = false;
    [SerializeField]
    private ParticleSystem particleEffect;

    private CharacterController cc;

    private bool canWarp = true;
    [SerializeField] private LayerMask securityWarpLayer;

    void Awake()
    {
        
        cc = GetComponent<CharacterController>();
        /*
        if (SaveSystem.LoadPlayer() != null){
            PlayerData data = SaveSystem.LoadPlayer();
            inPast = data.inPast;
        }*/
        if (inPast)
        {
            futureLevel.SetActive(false);
            pastLevel.SetActive(true);
        }
        else
        {
            pastLevel.SetActive(false);
            futureLevel.SetActive(true);
        }
    }

    bool TeleportPlayerFromTo(GameObject from, GameObject to)
    {
        bool reenableCc = false;
        if (cc) //Disable CharacterController temporarily, if exists and active
        {
            reenableCc = cc.enabled;
            cc.enabled = false;
        }
        //Teleport player to the relative position of the other scene
        Vector3 relativePos = from.transform.InverseTransformPoint(transform.position);
        Vector3 teleportPos = to.transform.position + relativePos;

        if(!Physics.CheckCapsule(teleportPos + new Vector3(0f, 0.5f, 0f), teleportPos + new Vector3(0f, 1.5f, 0f), 0.5f, securityWarpLayer, QueryTriggerInteraction.Ignore)) // Check si quelque chose bloquerait le player apres le warp
        {
            transform.position = teleportPos;
            if (reenableCc)
            {
                cc.enabled = true;
            }
            return true;
        }
        else
        {
            if (reenableCc)
            {
                cc.enabled = true;
            }
            return false;
        }
    }

    bool WarpFuture()
    {
        futureLevel.SetActive(true);
        if(TeleportPlayerFromTo(pastLevel, futureLevel))
        {
            canWarp = false;
            Invoke("setPastLevelInactive", 1f);
            return true;
        }
        else // cancel le tp si obstacle
        {
            futureLevel.SetActive(false);
            return false;
        }
        
    }

    bool WarpPast()
    {
        pastLevel.SetActive(true);
        if (TeleportPlayerFromTo(futureLevel, pastLevel))
        {
            canWarp = false;
            Invoke("setFutureLevelInactive", 1f);
            return true;
        }
        else // cancel le tp si obstacle
        {
            pastLevel.SetActive(false);
            return false;
        }
        
    }

    public void WarpInTime()
    {
        if (inPast)
        {
            if(WarpFuture())
            {
                inPast = false;
                particleEffect.Play();
            }
            else // tp failed
            {
                UITextManager.Instance.PrintText("Time warp failed");
            }
            
        }
        else
        {
            if(WarpPast())
            {
                inPast = true;
                particleEffect.Play();
            }
            else // tp failed
            {
                UITextManager.Instance.PrintText("Time warp failed");
            }
        }
        if(RoomsTreeManager.Instance != null)
        {
            RoomsTreeManager.Instance.SwitchTimeline();
        }
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.F) && canWarp)
        {
            Debug.Log("TimeWarp");
            WarpInTime();
        }
    }

    void setFutureLevelInactive()
    {
        futureLevel.SetActive(false);
        canWarp = true;
    }

    void setPastLevelInactive()
    {
        pastLevel.SetActive(false);
        canWarp = true;
    }

}
