using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchRoomArea : Area
{
    [SerializeField]
    private RoomNode root;
    [SerializeField]
    private RoomNode children;

    protected override void EnterAction()
    {
        RoomsTreeManager manager = RoomsTreeManager.Instance;
        print("Player entered switchRoom");
        if (RoomsTreeManager.Instance.GetCurrentRoom() == root)
        {
            RoomsTreeManager.Instance.ChangeRoom(children);
        }
        else
        {
            RoomsTreeManager.Instance.ChangeRoom(root);
        }
    }
}
