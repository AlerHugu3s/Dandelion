using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudController : MonoBehaviour
{
    [SerializeField,Range(0,20.0f)] private float rotateSpeed = 5f;

    [SerializeField, Range(0, 20.0f)] private float moveSpeed = 5f;

    private BoxCollider2D collider;

    [SerializeField] private GameObject cursorImage;

    void Awake()
    {
        collider = GetComponent<BoxCollider2D>();
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

    void ProcessMouseInput()
    {
        float mouseY = Input.GetAxisRaw("Mouse ScrollWheel");

        //鼠标左键处理
        if(Input.GetMouseButtonDown(0))
        {
            collider.enabled = true;
        }
        else if (Input.GetMouseButtonUp(0))
        {
            collider.enabled = false;
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
        Vector3 newPos = new Vector3(Mathf.Clamp(moveX + transform.position.x ,-14.2f,14.2f)
            , Mathf.Clamp(moveY + transform.position.y, -8.0f, 8.0f), 0);
        transform.position = Vector3.Slerp(transform.position, newPos,
            Time.deltaTime * moveSpeed);
    }

    void ProcessRotate()
    {
        Vector2 direction = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * rotateSpeed);

        cursorImage.transform.position = (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition);
        cursorImage.transform.rotation = Quaternion.AngleAxis(angle + 225, Vector3.forward); ;
    }

    void OnTriggerStay2D(Collider2D coll)
    {
        switch (coll.tag)
        {
            case "Dandelion":
                GameEventDispatcher.GetInstance()
                    .DispatchEvent(new BaseGameEvent(CoreController.GameEventType.Dandelion_Get_Wind, null, coll.gameObject));
                break;
        }
    }
}
