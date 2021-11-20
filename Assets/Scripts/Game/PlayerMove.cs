using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    public float moveSpeed = 8;
    public bool firstPerson;

    public Vector3 startPositon;
    public Quaternion startRotation;
    public ButtonFunction buttonFunction;

    public Camera bodyCamera;

    // Start is called before the first frame update
    void Start()
    {
        startRotation = transform.rotation;
        startPositon = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.W))
        {
            buttonFunction.Walk();
            transform.Translate(Vector3.forward * moveSpeed * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.S))
        {
            buttonFunction.Walk();
            transform.Translate(Vector3.back * moveSpeed * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.D))
        {
            buttonFunction.Walk();
            transform.Translate(Vector3.right * moveSpeed * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.A))
        {
            buttonFunction.Walk();
            transform.Translate(Vector3.left * moveSpeed * Time.deltaTime);
        }

        if (!Input.GetKey(KeyCode.W) && !Input.GetKey(KeyCode.S) && !Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.D)) buttonFunction.Idle();
    }

    public float sensitivity = 2f;
    Vector2 rotation = Vector2.zero;
    const string xAxis = "Mouse X";
    const string yAxis = "Mouse Y";
    public float yRotationLimit = 88f;

    void OnGUI()
    {
        if (firstPerson)
        {
            rotation.x += Input.GetAxis(xAxis) * sensitivity;
            rotation.y += Input.GetAxis(yAxis) * sensitivity;
            rotation.y = Mathf.Clamp(rotation.y, -yRotationLimit, yRotationLimit);
            var xQuat = Quaternion.AngleAxis(rotation.x, Vector3.up);
            var yQuat = Quaternion.AngleAxis(rotation.y, Vector3.right);

            transform.localRotation = xQuat;
            bodyCamera.transform.localRotation = yQuat;
            bodyCamera.transform.Rotate(0, 180, 0);
        }
    }
}
