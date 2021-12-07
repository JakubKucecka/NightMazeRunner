using UnityEngine;

/// <summary>
/// script zabezpecuje rotaciu brzca
/// </summary>
public class PlayerRotation : MonoBehaviour
{
    public bool firstPerson;
    public Quaternion startRotation;

    private Camera cam;

    /// <summary>
    /// pri starte sa ulozia startovacie informacie
    /// </summary>
    void Start()
    {
        cam = Camera.main;
        startRotation = transform.rotation;
        firstPerson = false;
    }

    /// <summary>
    /// rotacia je nastavena pomocou ratanaia natozenia hraca ku kurzoru mysi
    /// </summary>
    void OnGUI()
    {
        if (cam != null && cam.enabled && !firstPerson)
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
