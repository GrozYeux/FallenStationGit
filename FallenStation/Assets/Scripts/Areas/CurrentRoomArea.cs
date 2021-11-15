using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurrentRoomArea : Area
{
    [SerializeField]
    private RoomNode room;

    protected override void EnterAction()
    {
        RoomsTreeManager manager = RoomsTreeManager.Instance;
        print("Player entered zoneRoom :"+gameObject.name);
        if(room == null)
        {
            Debug.LogError("Error :: "+gameObject.name + "Area room is NULL ! Fix needed");
            return;
        }
        if(manager.GetCurrentRoom() != room && manager.GetNextRoom() != room)
        {
           
            manager.ChangeRoom(room, null, true);
        }
    }
}
