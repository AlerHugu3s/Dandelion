using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Tilemaps;

public class ObstacleControl : MonoBehaviour
{
    private Tilemap tilemap;

    [SerializeField,Range(0,20)]
    private float maxSpeed = 10.0f;

    [SerializeField, Range(0, 1)] private float speedThreshold = 0.5f;

    [SerializeField, Range(0, 1)] private float weightDec = 0.25f;

    void Start()
    {
        tilemap = GetComponent<Tilemap>();
    }
    void OnCollisionEnter2D(Collision2D coll)
    {
        if (coll.collider.tag.Equals("Dandelion"))
        {
            GameObject dandelion = coll.gameObject;
            GrivityControl dancelionGc = dandelion.GetComponent<GrivityControl>();
            Vector3 dandelionPoint = dandelion.transform.position;
            Vector3 collPoint = coll.GetContact(0).point;

            Vector3 dandelionLocalPos = dandelion.transform.localPosition;
            Vector3 collLocalPos = dandelion.transform.InverseTransformPoint(collPoint);

            Vector2 pulseVec = collPoint - dandelionPoint;
            Vector2 pulseLocalVec = collLocalPos - dandelionLocalPos;
            Vector2 dandelionVelocity = dandelion.GetComponent<Rigidbody2D>().velocity;

            float impulsePower = Vector3.Magnitude(Vector3.Project(dandelionVelocity, pulseVec));


            float tempWeightDec;
            if (impulsePower >= maxSpeed * speedThreshold)
                tempWeightDec = weightDec * 2;
            else
                tempWeightDec = weightDec;

            if (pulseLocalVec.x >= 0 && pulseLocalVec.y >= 0)
            {
                dancelionGc.pWeight_2 -= tempWeightDec;
                if (dancelionGc.pWeight_2 < 0) dancelionGc.pWeight_2 = 0;
            }
            else if (pulseLocalVec.x >= 0 && pulseLocalVec.y < 0)
            {
                dancelionGc.pWeight_1 -= tempWeightDec;
                if (dancelionGc.pWeight_1 < 0) dancelionGc.pWeight_1 = 0;
            }
            else if (pulseLocalVec.x < 0 && pulseLocalVec.y >= 0)
            {
                dancelionGc.pWeight_4 -= tempWeightDec;
                if (dancelionGc.pWeight_4 < 0) dancelionGc.pWeight_4 = 0;
            }
            else if (pulseLocalVec.x < 0 && pulseLocalVec.y < 0)
            {
                dancelionGc.pWeight_3 -= tempWeightDec;
                if (dancelionGc.pWeight_3 < 0) dancelionGc.pWeight_3 = 0;
            }

        } 
    }
}
