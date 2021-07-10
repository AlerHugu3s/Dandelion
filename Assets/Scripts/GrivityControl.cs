using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.U2D.Path;
using UnityEngine;

public class GrivityControl : MonoBehaviour
{
    [SerializeField]
    private Transform tr_1, tr_2, tr_3, tr_4 , stick;

    [SerializeField,Range(-180,180)] private float maxRotation;

    [SerializeField,Range(0,100)] private float HorizontalForce = 1.0f;

    [SerializeField, Range(0.0f, 10.0f)] private float AddictiveGravity = 1.0f;

    [SerializeField, Range(0.0f, 10.0f)] private float AddictiveForce = 1.0f;

    private Vector2 HorizontalForceVec;
    private Vector3 stickPos;

    public float pWeight_1 = 1.0f, pWeight_2 = 1.0f, pWeight_3 = 1.0f, pWeight_4 = 1.0f;

    private Vector3 centerOfMass,centerPos;

    public Vector3 velocityBeforePhysicsUpdate;

    private Rigidbody2D rig2D;

    void Awake()
    {
        stickPos = stick.localPosition + Vector3.down * 0.5f;
        centerOfMass = (tr_1.localPosition + tr_2.localPosition + tr_3.localPosition  + tr_4.localPosition + stickPos) / 5.0f;
        centerPos = (tr_1.localPosition + tr_2.localPosition + tr_3.localPosition + tr_4.localPosition) / 4.0f;

        HorizontalForceVec = Vector2.right * HorizontalForce;
        rig2D = GetComponent<Rigidbody2D>();
        rig2D.centerOfMass = centerOfMass;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        RecalculatePhysics();
    }

    void FixedUpdate()
    {
        rig2D.AddTorque(Physics2D.gravity.y * (centerOfMass.x - centerPos.x) / 0.5f , ForceMode2D.Force);
        rig2D.AddForce(Physics.gravity * (4.0f - (pWeight_1 + pWeight_2 + pWeight_3 + pWeight_4)) / 4.0f * AddictiveGravity, ForceMode2D.Force);

        velocityBeforePhysicsUpdate = rig2D.velocity;
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.TransformPoint(centerPos), transform.TransformPoint(centerOfMass));
    }

    private void RecalculatePhysics()
    {
        centerOfMass = (tr_1.localPosition * pWeight_1 
                        + tr_2.localPosition * pWeight_2 + tr_3.localPosition * pWeight_3 
                        + tr_4.localPosition * pWeight_4 + stick.localPosition) / 5.0f;
        rig2D.centerOfMass = centerOfMass;


        float rotZ = transform.rotation.eulerAngles.z;
        if (rotZ >= 180) rotZ = rotZ - 360;
        transform.rotation = Quaternion.Euler(0,0,Mathf.Clamp( rotZ, -maxRotation, maxRotation));
    }
    
    public void GetForce(Vector3 orgPos, float force)
    {

        Vector3 forceVec = Vector3.Normalize(transform.position - orgPos) * force;
        float radian = Mathf.Deg2Rad * Vector3.Angle(centerOfMass, forceVec);
        float torqueForce = Mathf.Sin(radian) * Vector3.Magnitude(forceVec);

        rig2D.AddTorque(torqueForce * (centerOfMass.x - centerPos.x) / 0.5f, ForceMode2D.Force);
        rig2D.AddForce(forceVec + forceVec * (4.0f - (pWeight_1 + pWeight_2 + pWeight_3 + pWeight_4)) / 4.0f * AddictiveForce, ForceMode2D.Force);
    }


}
