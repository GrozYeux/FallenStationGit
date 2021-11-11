using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]

public class ObjectData
{
    public string[] objects;
    public ObjectData(Collectables collectable)
    {
        objects = collectable.ArrayObjects();
    }
}
