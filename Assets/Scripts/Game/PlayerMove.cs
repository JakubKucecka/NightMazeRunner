using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    public float moveSpeed = 8;
    public bool firstPerson;

    public float sensitivity = 0.5f;
    Vector2 rotation = Vector2.zero;
    const string xAxis = "Mouse X";
    const string yAxis = "Mouse Y";
    public float yRotationLimit = 88f;

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
        if (Input.GetButton("MoveUp"))
        {
            buttonFunction.Walk();
            transform.Translate(Vector3.forward * moveSpeed * Time.deltaTime);
        }
        if (Input.GetButton("MoveDown"))
        {
            buttonFunction.Walk();
            transform.Translate(Vector3.back * moveSpeed * Time.deltaTime);
        }
        if (Input.GetButton("MoveRight"))
        {
            buttonFunction.Walk();
            transform.Translate(Vector3.right * moveSpeed * Time.deltaTime);
        }
        if (Input.GetButton("MoveLeft"))
        {
            buttonFunction.Walk();
            transform.Translate(Vector3.left * moveSpeed * Time.deltaTime);
        }

        if (!Input.GetButton("MoveUp") && !Input.GetButton("MoveDown") && !Input.GetButton("MoveRight") && !Input.GetButton("MoveLeft")) buttonFunction.Idle();
    }

    void OnGUI()
    {
        if (firstPerson)
        {
            rotation.x += Input.GetAxis(xAxis) * sensitivity;
            rotation.y += Input.GetAxis(yAxis) * sensitivity;
            rotation.y = Mathf.Clamp(rotation.y, -yRotationLimit, yRotationLimit*2);
            var xQuat = Quaternion.AngleAxis(rotation.x, Vector3.up * Time.deltaTime);
            var yQuat = Quaternion.AngleAxis(rotation.y, Vector3.right * Time.deltaTime);

            transform.localRotation = xQuat;
            bodyCamera.transform.localRotation = yQuat;
            bodyCamera.transform.Rotate(0, 180, 0);
        }
    }
}
