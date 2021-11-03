using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    [SerializeField] Transform endTransform;
    private Vector3 startPosition;
    private Vector3 endPosition;
    public float desiredOpeningDuration = 0.5f;
    private float elapsedTime = 0.0f;
    private bool openCorouRunning = false;
    private bool closeCorouRunning = false;

    private bool isOpen = false;

    public bool isLocked;
    public string cardToUnlock;

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
            }
        }
    }

    public void Close()
    {
        if (!closeCorouRunning && isOpen)
        {
            StartCoroutine(CloseCorou());
        }
    }

    IEnumerator OpenCorou()
    {
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
