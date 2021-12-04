using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ghost : MonoBehaviour
{
    public float startSpeed = 4f;
    public float speed;

    [SerializeField]
    Player player;
    [SerializeField]
    List<Canvas> attackCanvas = new List<Canvas>();

    [SerializeField]
    int border = 25;

    private Vector3 startPosition;

    private float changeDirTime = 0;
    private Vector3 dir;
    private Quaternion rotate;
    private Quaternion startRotation;
    private Quaternion childStartRotation;

    private Vector3 up = new Vector3(1, 0, 0);
    private Vector3 down = new Vector3(-1, 0, 0);
    private Vector3 right = new Vector3(0, 0, 1);
    private Vector3 left = new Vector3(0, 0, -1);

    private float blinkTime;
    private float blinkTimeChange = 0.30f;
    private int blinkCounter;
    private bool attack;

    private AudioSource breathingSound;
    private AudioSource attackSound;
    private AudioSource screamSound;
    private bool IsBreathing;

    public Vector3 bonePosition;
    public bool goForBone;
    // Start is called before the first frame update
    void Start()
    {
        goForBone = false;

        var audioSources = GetComponents<AudioSource>();
        foreach (var a in audioSources)
        {
            if (a.clip.name == "attack") attackSound = a;
            if (a.clip.name == "zombi_breathing") breathingSound = a;
            if (a.clip.name == "scream") screamSound = a;
        }
        IsBreathing = false;
        speed = startSpeed;
        startPosition = transform.position;
        startRotation = transform.rotation;
        childStartRotation = transform.GetChild(0).transform.rotation;
    }

    // Update is called once per frame
    void Update()
    {
        if (bonePosition != null && Vector3.Distance(transform.position, bonePosition) < 5)
        {
            goForBone = false;
            transform.rotation = startRotation;
            transform.GetChild(0).transform.rotation = childStartRotation;
        }

        if (player.gameIsStarted)
        {
            if (!IsBreathing)
            {
                IsBreathing = true;
                breathingSound.Play();
            }

            if (changeDirTime <= Time.time)
            {
                float nextTime = Random.Range(0.5f, 2f);
                changeDirTime += nextTime;
                GetDir();
            }

            if (transform.position.x >= border || transform.position.x <= -1 * border
                || transform.position.z >= border || transform.position.z <= -1 * border)
            {
                GetDir();
            }

            transform.Translate(dir * speed * Time.deltaTime);
            transform.GetChild(0).rotation = rotate;

            if (attack && blinkTime < Time.time && blinkCounter < 10)
            {
                blinkTime = Time.time + blinkTimeChange;
                foreach (var a in attackCanvas)
                {
                    a.gameObject.SetActive(!a.gameObject.activeSelf);
                    blinkCounter += 1;
                }
            }
            else
            {
                foreach (var a in attackCanvas)
                {
                    a.gameObject.SetActive(false);
                }
            }
        }
        else
        {
            if (IsBreathing)
            {
                IsBreathing = false;
                breathingSound.Stop();
            }
            goForBone = false;
            transform.rotation = startRotation;
            transform.GetChild(0).transform.rotation = childStartRotation;
        }
    }

    public void GetDir()
    {
        if (!goForBone)
        {
            List<Vector3> dirs = new List<Vector3>();

            if (transform.position.x + 3 <= border) dirs.Add(down);
            if (transform.position.x - 3 >= -1 * border) dirs.Add(up);
            if (transform.position.z + 3 <= border) dirs.Add(right);
            if (transform.position.z - 3 >= -1 * border) dirs.Add(left);

            dirs = getPlayerPosition(dirs);

            int index = Random.Range(0, dirs.Count);
            dir = dirs[index];
            rotate = GetRotate();
        }
        else
        {
            transform.LookAt(new Vector3(bonePosition.x, transform.position.y, bonePosition.z));
            rotate = Quaternion.Inverse(Quaternion.identity);
            dir = Vector3.forward * speed;
        }
    }

    private List<Vector3> getPlayerPosition(List<Vector3> dirs)
    {
        if (player.transform.position.x < transform.position.x)
        {
            dirs.Add(up);
        }
        else
        {
            dirs.Add(down);
        }

        if (player.transform.position.z < transform.position.z)
        {
            dirs.Add(left);
        }
        else
        {
            dirs.Add(right);
        }

        return dirs;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.name == "Player")
        {
            attackSound.Play();
            screamSound.Play();
            player.GetLive();
            if (player.lives <= 0) player.gameover = true;

            attack = true;
            blinkCounter = 0;
            blinkTime = 0;
        }
    }

    public void ReloadGhost()
    {
        transform.position = startPosition;
        attack = false;
        blinkCounter = 0;
        blinkTime = 0;
    }

    Quaternion GetRotate()
    {
        if (dir == up)
        {
            return Quaternion.Euler(Vector3.up * -90);
        }
        else if (dir == down)
        {
            return Quaternion.Euler(Vector3.up * 90);
        }
        else if (dir == right)
        {
            return Quaternion.Euler(Vector3.up * 0);
        }
        else
        {
            return Quaternion.Euler(Vector3.up * 180);
        }
    }
}
