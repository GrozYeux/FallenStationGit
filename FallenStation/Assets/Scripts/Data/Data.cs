using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Data
{
    public PlayerData playerData;
    public CodexData codexData;
    public string[] objets;
    public Data(PlayerMovementScript player, Collectables collectable)
    {
        playerData = new PlayerData(player);
        codexData = new CodexData(collectable);
        objets = collectable.ArrayObjects();


    }
}