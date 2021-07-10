using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class DoTweenMove : MonoBehaviour
{
    [SerializeField] private Transform targetPos;
    [SerializeField,Range(0,10)] private float duration;

    [SerializeField]
    Ease easeType = Ease.InElastic;

    [SerializeField]
    LoopType loopType = LoopType.Yoyo;

    [SerializeField]
    int loopTime = -1;

    // Start is called before the first frame update
    void Start()
    {
        transform.DOMove(targetPos.position, duration).SetLoops(loopTime, loopType).SetEase(easeType);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
