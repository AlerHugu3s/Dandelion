using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChildTrigger : MonoBehaviour
{
    void OnTriggerStay2D(Collider2D coll)
    {
        switch (coll.tag)
        {
            case "Dandelion":
                GameEventDispatcher.GetInstance()
                    .DispatchEvent(new BaseGameEvent(MainMenuController.GameEventType.DANDELION_GET_PLAYER_WIND, null, coll.gameObject));
                break;
        }
    }
}
