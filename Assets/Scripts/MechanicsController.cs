using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms;

public class MechanicsController : MonoBehaviour
{
    public enum WindMode
    {
        LineMode,
        GlobalMode
    }
    [Range(0, 200)] public float WindForce;
    public Vector3 windVec = Vector3.zero;

    public WindMode windMode = WindMode.LineMode;
    // Start is called before the first frame update
    void Start()
    {
        
    }
    void OnTriggerStay2D(Collider2D coll)
    {
        switch (coll.tag)
        {
            case "Dandelion":
                GameEventDispatcher.GetInstance()
                    .DispatchEvent(new BaseGameEvent(CoreController.GameEventType.DANDELION_GET_WIND, null, this));
                break;
        }
    }
}
