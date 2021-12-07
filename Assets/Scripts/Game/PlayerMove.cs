using UnityEngine;

/// <summary>
/// script ovlada pohyb hraca
/// </summary>
public class PlayerMove : MonoBehaviour
{
    public bool firstPerson;

    [SerializeField]
    float moveSpeed = 8;
    [SerializeField]
    float sensitivity = 0.5f;
    [SerializeField]
    float yRotationLimit = 88f;

    [SerializeField]
    ButtonFunction buttonFunction;
    [SerializeField]
    Camera bodyCamera;

    const string xAxis = "Mouse X";
    const string yAxis = "Mouse Y";
    private Vector2 rotation = Vector2.zero;

    public Vector3 startPositon;
    public Quaternion startRotation;

    private AudioSource stepSound;
    private bool isWalking;

    /// <summary>
    /// pri starte sa ulozia startovacie informacie a nacita zvukovy zdroj
    /// </summary>
    void Start()
    {
        stepSound = GetComponent<AudioSource>();
        isWalking = false;

        startRotation = transform.rotation;
        startPositon = transform.position;
    }

    /// <summary>
    /// v update sa kontroluje pohyb pomocou stlacenia kalves v InputManagery
    /// pri stlaceni dvoch klaves sa znasovila rychlost a nastal problem s ratanim fyziky
    /// preto pri stlaceni klaves je hrac spomaleny na polovicu
    /// 
    /// trieda buttonFunction, ktora zabezpecuje animacie je prebrata z assetu _GhoulZombie
    /// </summary>
    void Update()
    {
        if (Input.GetButton("MoveUp"))
        {
            buttonFunction.Walk();
            if (!Input.GetButton("MoveDown") && !Input.GetButton("MoveRight") && !Input.GetButton("MoveLeft"))
            {
                transform.Translate(Vector3.forward * moveSpeed * Time.deltaTime);
            }
            else
            {
                transform.Translate(Vector3.forward * moveSpeed * Time.deltaTime / 2);
            }
        }
        if (Input.GetButton("MoveDown"))
        {
            buttonFunction.Walk();
            if (!Input.GetButton("MoveUp") && !Input.GetButton("MoveRight") && !Input.GetButton("MoveLeft"))
            {
                transform.Translate(Vector3.back * moveSpeed * Time.deltaTime);
            }
            else
            {
                transform.Translate(Vector3.back * moveSpeed * Time.deltaTime / 2);
            }
        }
        if (Input.GetButton("MoveRight"))
        {
            buttonFunction.Walk();
            if (!Input.GetButton("MoveUp") && !Input.GetButton("MoveDown") && !Input.GetButton("MoveLeft"))
            {
                transform.Translate(Vector3.right * moveSpeed * Time.deltaTime);
            }
            else
            {
                transform.Translate(Vector3.right * moveSpeed * Time.deltaTime / 2);
            }
        }
        if (Input.GetButton("MoveLeft"))
        {
            buttonFunction.Walk();
            if (!Input.GetButton("MoveUp") && !Input.GetButton("MoveDown") && !Input.GetButton("MoveRight"))
            {
                transform.Translate(Vector3.left * moveSpeed * Time.deltaTime);
            }
            else
            {
                transform.Translate(Vector3.left * moveSpeed * Time.deltaTime / 2);
            }
        }

        // ak nie je stlaceny ziaden klaves hrac je animovany ako by stal ale hybe hlavou
        // v opacnom pripade je pusteny zvuk chodze
        if (!Input.GetButton("MoveUp") && !Input.GetButton("MoveDown") && !Input.GetButton("MoveRight") && !Input.GetButton("MoveLeft"))
        {
            buttonFunction.Idle();
            if (isWalking)
            {
                isWalking = false;
                stepSound.Stop();
            }
        }
        else
        {
            if (!isWalking)
            {
                isWalking = true;
                stepSound.Play();
            }
        }
    }

    /// <summary>
    /// tu sa vykonava natocenie hraca pomocou mysi ak pouzivame first person kameru
    /// </summary>
    void OnGUI()
    {
        if (firstPerson)
        {
            rotation.x += Input.GetAxis(xAxis) * sensitivity;
            rotation.y += Input.GetAxis(yAxis) * sensitivity;
            rotation.y = Mathf.Clamp(rotation.y, -yRotationLimit, yRotationLimit * 2);
            var xQuat = Quaternion.AngleAxis(rotation.x, Vector3.up * Time.deltaTime);
            var yQuat = Quaternion.AngleAxis(rotation.y, Vector3.right * Time.deltaTime);

            transform.localRotation = xQuat;
            transform.transform.Rotate(0, -90, 0);
            bodyCamera.transform.localRotation = yQuat;
            bodyCamera.transform.Rotate(0, 180, 0);
        }
    }
}
