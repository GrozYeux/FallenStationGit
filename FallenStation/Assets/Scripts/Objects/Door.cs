using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    [SerializeField] Transform endTransform;
    private Vector3 startPosition;
    private Vector3 endPosition;
    public float desiredOpeningDuration = 0.5f;
    private bool openCorouRunning = false;
    private bool closeCorouRunning = false;

    private bool isOpen = false;

    public bool isLocked;
    public bool hardLock = false;
    public string cardToUnlock;

    [Header("Sound")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip doorOpen;
    [SerializeField] private AudioClip doorClose;

    [Header("Body")]
    [SerializeField] private GameObject doorBody;
    private Animator doorBodyAnimator;

    void Start()
    {
        startPosition = this.transform.position;
        endPosition = endTransform.position;
        if (doorBody)
        {
            doorBodyAnimator = doorBody.GetComponent<Animator>();
        }
    }


    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Invoke("Close", 1.0f);
        }
    }

    public void Open()
    {
        if (!openCorouRunning && !isOpen)
        {
            if (isLocked)
            {
                if (cardToUnlock != "" && hardLock == false)
                {
                    if (Collectables.Instance.CheckObject(cardToUnlock))
                    {
                        StartCoroutine(OpenCorou());
                        Invoke("Close", 5f);
                    }
                    else
                    {
                        UITextManager.Instance.PrintText("Item " + cardToUnlock + " nécessaire pour ouvrir");
                    }
                }
                else
                {
                    UITextManager.Instance.PrintText("Porte bloquée");
                }
            }
            else
            {
                StartCoroutine(OpenCorou());
                Invoke("Close", 5f);
            }
        }
    }

    public void HardLock()
    {
        this.hardLock = true;
    }

    public void UnHardLock()
    {
        this.hardLock = false;
    }

    public void Close()
    {
        if (!closeCorouRunning && isOpen)
        {
            CancelInvoke("Close");
            StartCoroutine(CloseCorou());
        }
    }

    IEnumerator OpenCorou()
    {
        audioSource.PlayOneShot(doorOpen);
        openCorouRunning = true;
        float elapsedTime = 0;

        if (doorBody)
        {
            doorBodyAnimator.speed = 1.1f / desiredOpeningDuration;
            doorBodyAnimator.Play("Base Layer.Open");
        }
        while (elapsedTime < desiredOpeningDuration)
        {
            if (doorBody == null)
                this.transform.position = Vector3.Lerp(startPosition, endPosition, elapsedTime / desiredOpeningDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        elapsedTime = 0;
        if (doorBody == null)
            this.transform.position = endPosition;
        else //Disable collider
            doorBody.GetComponent<BoxCollider>().enabled = false;
        isOpen = true;
        openCorouRunning = false;
    }

    IEnumerator CloseCorou()
    {
        audioSource.PlayOneShot(doorClose);
        closeCorouRunning = true;
        float elapsedTime = 0;

        if (doorBody)
        {
            doorBodyAnimator.speed = 1.1f / desiredOpeningDuration;
            doorBodyAnimator.Play("Base Layer.Close");
        }
        while (elapsedTime < desiredOpeningDuration)
        {
            if (doorBody == null)
                this.transform.position = Vector3.Lerp(endPosition, startPosition, elapsedTime / desiredOpeningDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        elapsedTime = 0;
        if (doorBody == null)
            this.transform.position = startPosition;
        else //Enable collider
            doorBody.GetComponent<BoxCollider>().enabled = true;
        isOpen = false;
        closeCorouRunning = false;
    }
}
