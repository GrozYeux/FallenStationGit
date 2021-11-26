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
    public string cardToUnlock;

    [Header("Sound")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip doorOpen;
    [SerializeField] private AudioClip doorClose;
    [SerializeField] private AudioClip doorFail;

    void Start()
    {
        startPosition = this.transform.position;
        endPosition = endTransform.position;
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
                if (cardToUnlock != "")
                {
                    if (Collectables.Instance.CheckObject(cardToUnlock))
                    {
                        StartCoroutine(OpenCorou());
                        Invoke("Close", 5f);
                    }
                    else
                    {
                        UITextManager.Instance.PrintText("Item " + cardToUnlock + " nécessaire pour ouvrir");
                        if (audioSource.isPlaying) audioSource.Stop();
                        audioSource.pitch = 0.1f;
                        audioSource.PlayOneShot(doorFail);
                        audioSource.pitch = 1f;
                    }
                }
                else
                {
                    UITextManager.Instance.PrintText("Porte bloquée");
                    if (audioSource.isPlaying) audioSource.Stop();
                    audioSource.pitch = 0.1f;
                    audioSource.PlayOneShot(doorFail);
                    audioSource.pitch = 1f;
                }
            }
            else
            {
                StartCoroutine(OpenCorou());
                Invoke("Close", 5f);
            }
        }
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

        while (elapsedTime < desiredOpeningDuration)
        {
            this.transform.position = Vector3.Lerp(startPosition, endPosition, elapsedTime / desiredOpeningDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        elapsedTime = 0;
        this.transform.position = endPosition;
        isOpen = true;
        openCorouRunning = false;
    }

    IEnumerator CloseCorou()
    {
        audioSource.PlayOneShot(doorClose);
        closeCorouRunning = true;
        float elapsedTime = 0;

        while (elapsedTime < desiredOpeningDuration)
        {
            this.transform.position = Vector3.Lerp(endPosition, startPosition, elapsedTime / desiredOpeningDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        elapsedTime = 0;
        this.transform.position = startPosition;
        isOpen = false;
        closeCorouRunning = false;
    }
}
