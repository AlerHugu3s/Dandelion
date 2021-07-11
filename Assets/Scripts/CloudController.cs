using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudController : MonoBehaviour
{
    [SerializeField,Range(0,20.0f)] private float rotateSpeed = 5f;

    [SerializeField, Range(0, 20.0f)] private float moveSpeed = 5f;

    private GameObject colliderObj;

    private BoxCollider2D collider;

    public Vector2 maxBorder,minBorder;

    [SerializeField] private GameObject cursorImage;

    private bool isWindSound = false;

    void Awake()
    {
        colliderObj = transform.Find("Collider2D").gameObject;
        collider = colliderObj.GetComponent<BoxCollider2D>();
        collider.enabled = false;
        collider.isTrigger = true;

        cursorImage = Instantiate(cursorImage);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        ProcessInput();
    }

    void ProcessInput()
    {
        ProcessMouseInput();
        ProcessMove();
        ProcessRotate();
    }

    IEnumerator WindSoundFx()
    {
        isWindSound = true;
        yield return new WaitForSecondsRealtime(5);
        isWindSound = false;
    }

    void ProcessMouseInput()
    {
        float mouseY = Input.GetAxisRaw("Mouse ScrollWheel");

        //鼠标左键处理
        if(Input.GetMouseButtonDown(0))
        {
            collider.gameObject.SetActive(true);
        }
        else if (Input.GetMouseButtonUp(0))
        {
            collider.gameObject.SetActive(false);
        }
        else if(Input.GetMouseButton(0) && !isWindSound)
        {
            AudioController._instance.PlayAudioClip("Wind");
            StartCoroutine(WindSoundFx());
        }

        //鼠标右键处理
        if (Input.GetMouseButtonDown(1))
        {

        }
        else if (Input.GetMouseButtonUp(1))
        {

        }

        //滚轮处理
        if (mouseY != 0)
        {

        }
    }

    void ProcessMove()
    {
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");
        Vector3 newPos = new Vector3(Mathf.Clamp(moveX + transform.position.x ,minBorder.x,maxBorder.x)
            , Mathf.Clamp(moveY + transform.position.y, minBorder.y, maxBorder.y), 0);
        transform.position = Vector3.Slerp(transform.position, newPos,
            Time.deltaTime * moveSpeed);
    }

    void ProcessRotate()
    {
        Vector2 direction = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        float constraintAngle = Mathf.Clamp(angle, -45, 45);

        Vector2 InputPos = (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition);
        cursorImage.transform.position = InputPos;
        cursorImage.transform.rotation = Quaternion.AngleAxis(angle + 225, Vector3.forward); ;

        bool isFlip = true;
        if (transform.InverseTransformPoint(InputPos).x < 0)
        {
            isFlip = false;
            constraintAngle = -constraintAngle;
        }
        else
            isFlip = true;
        GetComponent<SpriteRenderer>().flipX = isFlip;

        Quaternion rotationSelf = Quaternion.AngleAxis(constraintAngle, Vector3.forward);
        Quaternion rotationCollision = Quaternion.AngleAxis(angle, Vector3.forward);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotationSelf, Time.deltaTime * rotateSpeed);
        colliderObj.transform.rotation = Quaternion.Slerp(colliderObj.transform.rotation, rotationCollision, Time.deltaTime * rotateSpeed);

    }
}
