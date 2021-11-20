using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRotation : MonoBehaviour
{
    public bool firstPerson;

    public Quaternion startRotation;

    private Camera cam;

    void Start()
    {
        cam = Camera.main;
        startRotation = transform.rotation;
        firstPerson = false;
    }

    void OnGUI()
    {
        if (cam != null && cam.enabled)
        {
            Event currentEvent = Event.current;
            Vector2 mousePos = new Vector2();

            mousePos.x = currentEvent.mousePosition.x;
            mousePos.y = cam.pixelHeight - currentEvent.mousePosition.y;

            Vector3 point = cam.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y, cam.transform.position.y - transform.position.y));
            transform.LookAt(point);
        }
    }
}
