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

    void Start()
    {
        cc = GetComponent<CharacterController>();
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

    void TeleportPlayerFromTo(GameObject from, GameObject to)
    {
        bool reenableCc = false;
        if (cc) //Disable CharacterController temporarily, if exists and active
        {
            reenableCc = cc.enabled;
            cc.enabled = false;
        }
        //Teleport player to the relative position of the other scene
        Vector3 relativePos = from.transform.InverseTransformPoint(transform.position);
        transform.position = to.transform.position + relativePos;
        if (reenableCc)
        {
            cc.enabled = true;
        }
    }

    void WarpFuture()
    {
        futureLevel.SetActive(true);
        TeleportPlayerFromTo(pastLevel, futureLevel);
        pastLevel.SetActive(false);
    }

    void WarpPast()
    {
        pastLevel.SetActive(true);
        TeleportPlayerFromTo(futureLevel, pastLevel);
        futureLevel.SetActive(false);
    }

    public void WarpInTime()
    {
        particleEffect.Play();
        if (inPast)
        {
            inPast = false;
            WarpFuture();
        }
        else
        {
            inPast = true;
            WarpPast();
        }
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.F))
        {
            Debug.Log("TimeWarp");
            WarpInTime();
        }
    }
}
