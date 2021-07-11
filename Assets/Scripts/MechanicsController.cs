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
    public enum TriggerMode
    {
        Infinite,
        Temp
    }
    [Range(0, 200)] public float WindForce;
    public Vector3 windVec = Vector3.zero;

    public WindMode windMode = WindMode.LineMode;

    public TriggerMode triggerMode = TriggerMode.Infinite;

    private bool isPause = false;

    [SerializeField,Range(0,10)]
    private float activeTime, pauseTime;

    private Animator animator;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponentInChildren<Animator>();
    }
    void OnTriggerStay2D(Collider2D coll)
    {
        if (!isPause)
        {
            if (triggerMode == TriggerMode.Temp)
                StartCoroutine(pauseEnumerator());
            switch (coll.tag)
            {
                case "Dandelion":
                    GameEventDispatcher.GetInstance()
                        .DispatchEvent(new BaseGameEvent(MainMenuController.GameEventType.DANDELION_GET_WIND, null, this));
                    StartShoot();
                    break;
            }
        }
    }

    void OnTriggerExit2D(Collider2D coll)
    {
        EndShoot();
    }

    IEnumerator pauseEnumerator()
    {
        yield return new WaitForSecondsRealtime(activeTime);
        isPause = true;
        yield return new WaitForSecondsRealtime(pauseTime);
        isPause = false;
    }

    void StartShoot()
    {
        animator?.gameObject.SetActive(true);
        animator?.SetTrigger("Shoot");
    }

    void EndShoot()
    {
        animator?.gameObject.SetActive(false);
    }
}
