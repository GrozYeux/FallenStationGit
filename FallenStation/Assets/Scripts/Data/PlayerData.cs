using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerData
{
    public float speed;
    public float sprintSpeed;
    public float gravity;
    public float jumpHeight;
    public float[] position;
    public float[] rotation;

    public PlayerData(PlayerMovementScript player)
    {
        speed = player.speed;
        sprintSpeed = player.sprintSpeed;
        gravity = player.gravity;
        jumpHeight = player.jumpHeight;

        position = new float[3];
        position[0] = player.transform.position.x;
        position[1] = player.transform.position.y;
        position[2] = player.transform.position.z;

        rotation = new float[4];
        rotation[0] = player.transform.rotation.x;
        rotation[1] = player.transform.rotation.y;
        rotation[2] = player.transform.rotation.z;
        rotation[3] = player.transform.rotation.w;
    }
}
