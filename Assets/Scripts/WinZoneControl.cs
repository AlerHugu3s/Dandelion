using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinZoneControl : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.tag.Equals("Dandelion"))
        {
            GameEventDispatcher.GetInstance().DispatchEvent(new BaseGameEvent(MainMenuController.GameEventType.GAME_WIN, null, this));
        }
    }
}
