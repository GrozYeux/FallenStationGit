using UnityEngine;
using UnityEngine.Events;

public class Area : MonoBehaviour
{
    private bool EnteredTrigger;
    public UnityEvent myEvent;

    private void Awake()
    {
        if (myEvent == null)
            myEvent = new UnityEvent();
    }

    protected void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            EnteredTrigger = true;
            EnterAction();
        }
    }

    protected virtual void EnterAction()
    {
        myEvent.Invoke();
    }

    public bool IsTriggered()
    {
        return EnteredTrigger;
    }

}
