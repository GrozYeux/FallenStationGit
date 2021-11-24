using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SasDouchesEventLvl2 : EventActionner
{
    public override void Activate()
    {
        this.gameObject.SetActive(true);
        GetComponent<Animator>().Play("Base Layer.sasWarning1");
    }

    public override void Deactivate()
    {
        this.gameObject.SetActive(false);
    }
}
