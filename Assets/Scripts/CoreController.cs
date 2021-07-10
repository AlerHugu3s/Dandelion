using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoreController : MonoBehaviour
{
    public enum GameEventType
    {
        GAME_OVER,
        GAME_WIN,
        PAUSE,
        Dandelion_Get_Wind
    }

    private string playerUrl = "Prefabs/Cloud", dandelionUrl = "Prefabs/Dandelion";
    GameObject player , dandelion;

    [SerializeField] private Transform playerPos, dandelionPos;

    [SerializeField, Range(0f, 100.0f)]
    private float playerWindForce = 10.0f;

    private Rigidbody2D dandelionRig2D;
    void Awake()
    {
        player = InstantiateObjByURL(playerUrl, playerPos.position, new Quaternion(0, 0, 0,0));
        dandelion = InstantiateObjByURL(dandelionUrl, dandelionPos.position, new Quaternion(0, 0, 0, 0));

        dandelionRig2D = dandelion.GetComponent<Rigidbody2D>();
    }

    private GameObject InstantiateObjByURL(string url,Vector3 pos,Quaternion rot)
    {
        return Instantiate(Resources.Load<GameObject>(url), pos, rot);
    }

    // Start is called before the first frame update
    void Start()
    {
        GameEventDispatcher.GetInstance().AddEventListener(GameEventType.Dandelion_Get_Wind, OnDandelionGetWind);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void FixedUpdate()
    {

    }

    void OnDestroy()
    {
        GameEventDispatcher.GetInstance().RemoveEventListener(GameEventType.Dandelion_Get_Wind, OnDandelionGetWind);
    }

    void OnDandelionGetWind(BaseGameEvent gEvent)
    {
        GrivityControl gcComponent = ((GameObject)gEvent.Sender).GetComponent<GrivityControl>();
        gcComponent.GetForce(player.transform.position, playerWindForce);
    }


}
