using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using static MainMenuController;

public class CoreController : MonoBehaviour
{
    public static bool isInvincible = false;
    public enum LevelType
    {
        LongLevel,
        ShortLevel
    }

    [SerializeField] private Vector2 maxBorder, minBorder;

    [SerializeField]
    private LevelType levelType = LevelType.ShortLevel;


    public CinemachineVirtualCamera vCam = null;
    public CinemachineConfiner cineConfiner = null;

    private string playerUrl = "Prefabs/Bird", dandelionUrl = "Prefabs/Dandelion3";
    GameObject player , dandelion;

    [SerializeField] private Transform playerPos, dandelionPos;

    [SerializeField, Range(0f, 1000.0f)]
    private float playerWindForce = 10.0f;

    [SerializeField, Range(0, 30.0f)] private float maxWindLength = 5f;

    private Rigidbody2D dandelionRig2D;
    void Awake()
    {
        player = InstantiateObjByURL(playerUrl, playerPos.position, new Quaternion(0, 0, 0,0));
        dandelion = InstantiateObjByURL(dandelionUrl, dandelionPos.position, new Quaternion(0, 0, 0, 0));

        player.GetComponent<CloudController>().maxBorder = maxBorder;
        player.GetComponent<CloudController>().minBorder = minBorder;

        dandelionRig2D = dandelion.GetComponent<Rigidbody2D>();

        switch (levelType)
        {
            case LevelType.ShortLevel:
                vCam.enabled = false;
                cineConfiner.enabled = false;

                Camera.main.gameObject.transform.position = -Vector3.forward;
                break;
            case LevelType.LongLevel:
                vCam.enabled = true;
                cineConfiner.enabled = true;
                vCam.Follow = player.transform;

                break;
        }

        Cursor.visible = false;
    }

    private GameObject InstantiateObjByURL(string url,Vector3 pos,Quaternion rot)
    {
        return Instantiate(Resources.Load<GameObject>(url), pos, rot);
    }

    // Start is called before the first frame update
    void Start()
    {
        GameEventDispatcher.GetInstance().AddEventListener(GameEventType.DANDELION_GET_PLAYER_WIND, OnDandelionGetPlayerWind);
        GameEventDispatcher.GetInstance().AddEventListener(GameEventType.DANDELION_GET_WIND, OnDandelionGetWind);
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
        GameEventDispatcher.GetInstance().RemoveEventListener(GameEventType.DANDELION_GET_PLAYER_WIND, OnDandelionGetPlayerWind);
        GameEventDispatcher.GetInstance().RemoveEventListener(GameEventType.DANDELION_GET_WIND, OnDandelionGetWind);
    }

    void OnDandelionGetPlayerWind(BaseGameEvent gEvent)
    {
        GrivityControl gcComponent = ((GameObject)gEvent.Sender).GetComponent<GrivityControl>();
        float windWeight = 1.0f - Mathf.Clamp(Vector3.Distance(
                               player.transform.position, dandelion.transform.position),0,maxWindLength) / maxWindLength;
        gcComponent.GetForceByDistance(player.transform.position, playerWindForce * windWeight);
    }

    void OnDandelionGetWind(BaseGameEvent gEvent)
    {
        MechanicsController mechanics = (MechanicsController)gEvent.Sender;
        GrivityControl gcComponent = (dandelion).GetComponent<GrivityControl>();

        if (mechanics.windMode == MechanicsController.WindMode.LineMode)
        {
            gcComponent.GetForceDirectly(mechanics.gameObject.transform.position, mechanics.WindForce);
        }
        else if (mechanics.windMode == MechanicsController.WindMode.GlobalMode)
        {
            gcComponent.GetForceByVec(mechanics.windVec,mechanics.WindForce);
        }
    }
}
