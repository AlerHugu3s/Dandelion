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

    [SerializeField, Range(0, 30.0f)] private float maxWindLength = 5f;

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
        GameEventDispatcher.GetInstance().AddEventListener(GameEventType.GAME_OVER, OnGameOver);
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
        GameEventDispatcher.GetInstance().RemoveEventListener(GameEventType.GAME_OVER, OnGameOver);
    }

    void OnDandelionGetWind(BaseGameEvent gEvent)
    {
        GrivityControl gcComponent = ((GameObject)gEvent.Sender).GetComponent<GrivityControl>();
        float windWeight = 1.0f - Mathf.Clamp(Vector3.Distance(
            player.transform.position, dandelion.transform.position),0,maxWindLength) / maxWindLength;
        gcComponent.GetForce(player.transform.position, playerWindForce * windWeight);
    }

    void OnGameOver(BaseGameEvent gEvent)
    {
        Debug.Log("GameOver!");
    }


}
