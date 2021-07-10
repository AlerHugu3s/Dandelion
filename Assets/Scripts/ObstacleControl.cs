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

    [SerializeField,Range(0,30)]
    private float maxSpeed = 10.0f;

    [SerializeField, Range(0, 1)] private float speedThreshold = 0.5f;

    [SerializeField, Range(0, 1)] private float weightDec = 0.25f;

    [SerializeField, Range(10, 500)] private float bouncePower = 500;

    void Start()
    {

    }
    void OnCollisionEnter2D(Collision2D coll)
    {
        if (coll.collider.tag.Equals("Dandelion") && !CoreController.isInvincible)
        {
            GameObject dandelion = coll.gameObject;
            GrivityControl dancelionGc = dandelion.GetComponent<GrivityControl>();
            Vector3 dandelionPoint = dandelion.transform.position;
            Vector3 collPoint = coll.GetContact(0).point;

            Vector3 collLocalPos = dandelion.transform.InverseTransformPoint(collPoint);

            Vector2 pulseVec = collPoint - dandelionPoint;
            Vector2 dandelionVelocity = dancelionGc.velocityBeforePhysicsUpdate;

            Debug.Log(pulseVec);
            Debug.Log(dandelionVelocity);

            Vector3 normalVec = Vector3.Cross(pulseVec, dandelionVelocity);
            float alphaAngle = Vector3.Angle(dandelionVelocity, pulseVec);
            float bounceAngle = 2 * (90 - alphaAngle);
            if (normalVec.z < 0)
                bounceAngle =  - bounceAngle;

            Debug.Log(bouncePower * Vector3.Normalize(Quaternion.AngleAxis(bounceAngle, Vector3.forward) * dandelionVelocity));
            Vector2 BounceVec = (90 - alphaAngle) / 90 * bouncePower * (Quaternion.AngleAxis(bounceAngle, Vector3.forward) * dandelionVelocity);
            coll.collider.GetComponent<Rigidbody2D>().AddForce(BounceVec,ForceMode2D.Force);


            float impulsePower = Vector3.Magnitude(Vector3.Project(dandelionVelocity, pulseVec));

            float tempWeightDec;
            if (impulsePower >= maxSpeed * speedThreshold)
                tempWeightDec = weightDec * 2;
            else
                tempWeightDec = weightDec;

            if (collLocalPos.x >= 0 && collLocalPos.y >= 0)
            {
                dancelionGc.pWeight_2 -= tempWeightDec;
                if (dancelionGc.pWeight_2 < 0) dancelionGc.pWeight_2 = 0;
            }
            else if (collLocalPos.x >= 0 && collLocalPos.y < 0)
            {
                dancelionGc.pWeight_1 -= tempWeightDec;
                if (dancelionGc.pWeight_1 < 0) dancelionGc.pWeight_1 = 0;
            }
            else if (collLocalPos.x < 0 && collLocalPos.y >= 0)
            {
                dancelionGc.pWeight_4 -= tempWeightDec;
                if (dancelionGc.pWeight_4 < 0) dancelionGc.pWeight_4 = 0;
            }
            else if (collLocalPos.x < 0 && collLocalPos.y < 0)
            {
                dancelionGc.pWeight_3 -= tempWeightDec;
                if (dancelionGc.pWeight_3 < 0) dancelionGc.pWeight_3 = 0;
            }
            CoreController.isInvincible = true;
            StartCoroutine(DisableInvincible());
            SpriteController._instance.RefreshPetalState();
        } 
    }

    IEnumerator DisableInvincible()
    {
        yield return new WaitForSecondsRealtime(1);
        CoreController.isInvincible = false;
    }
}
