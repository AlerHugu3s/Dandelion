using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudController : MonoBehaviour
{
    [SerializeField,Range(0,20.0f)] private float rotateSpeed = 5f;

    [SerializeField, Range(0, 20.0f)] private float moveSpeed = 5f;

    private BoxCollider2D collider;

    public Vector2 maxBorder,minBorder;

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

        //����������
        if(Input.GetMouseButtonDown(0))
        {
            collider.enabled = true;
        }
        else if (Input.GetMouseButtonUp(0))
        {
            collider.enabled = false;
        }

        //����Ҽ�����
        if (Input.GetMouseButtonDown(1))
        {

        }
        else if (Input.GetMouseButtonUp(1))
        {

        }

        //���ִ���
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
                    .DispatchEvent(new BaseGameEvent(CoreController.GameEventType.DANDELION_GET_PLAYER_WIND, null, coll.gameObject));
                break;
        }
    }
}
