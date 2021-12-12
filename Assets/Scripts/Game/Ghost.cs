using System.Collections.Generic;
using UnityEngine;

public class Ghost : MonoBehaviour
{
    public float startSpeed = 4f;
    public float speed;

    [SerializeField]
    Player player;
    [SerializeField]
    Canvas globalAttackCanvas;
    [SerializeField]
    Canvas playerAttackCanvas;

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

    /// <summary>
    /// pri starte inicializujeme zvuky, ktore dany objekt obsahuje a ulozime si startovaciu poziciu a otocenie
    /// </summary>
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
        childStartRotation = transform.GetChild(0).transform.localRotation;
    }

    /// <summary>
    /// update zabezpecuje pohyb a spustanie zvukov
    /// </summary>
    void Update()
    {
        // ak je aktivovany pohyb ku kostre tak kontrolujeme ako je duch daleko
        // ak je blizko ukoncime pohyb ku kostre
        if (bonePosition != null && Vector3.Distance(transform.position, bonePosition) < 5)
        {
            goForBone = false;
            transform.rotation = startRotation;
            transform.GetChild(0).transform.localRotation = childStartRotation;
        }

        // ak je hra spustena, teda nie je zobrazenen menu zacne pohyb ducha
        if (player.gameIsStarted)
        {
            // zapneme zvuk dychu ducha
            if (!IsBreathing)
            {
                IsBreathing = true;
                breathingSound.Play();
            }

            // ak je cas na zmenu pohybu tak vyratame novy cas zmeny a tiez smer pohybu
            if (changeDirTime <= Time.time)
            {
                float nextTime = Random.Range(0.5f, 2f);
                changeDirTime += nextTime;
                GetDir();
            }

            // zmenu smeru pohybu ratame aj v tom pripade, ze duch je pri okrajy areny
            if (transform.position.x >= border || transform.position.x <= -1 * border
                || transform.position.z >= border || transform.position.z <= -1 * border)
            {
                GetDir();
            }

            // po vyratany smeru pohybu zacneme duchom hybat v danom smere
            if (goForBone)
            {
                transform.Translate(dir * 7 * speed * Time.deltaTime);
                transform.GetChild(0).localRotation = childStartRotation;
            }
            else
            {
                transform.Translate(dir * speed * Time.deltaTime);
                transform.GetChild(0).rotation = rotate;
            }

            // ak nastane kolizia s hracom pustime zvuk utoku a zabezpecime blikanie aby sme hracovy ak vizualne naznacili utok
            if (attack && blinkTime < Time.time && blinkCounter < 10)
            {
                blinkTime = Time.time + blinkTimeChange;
                if (player.moveControler.firstPerson)
                {
                    playerAttackCanvas.gameObject.SetActive(!playerAttackCanvas.gameObject.activeSelf);
                }
                else
                {
                    globalAttackCanvas.gameObject.SetActive(!globalAttackCanvas.gameObject.activeSelf);
                }
                    blinkCounter += 1;
            }
            else
            {
                globalAttackCanvas.gameObject.SetActive(false);
                playerAttackCanvas.gameObject.SetActive(false);
            }
        }
        else
        {
            // ak je zobrazene menu tak vypiname zvuky a pohyb ku kostre
            if (IsBreathing)
            {
                IsBreathing = false;
                breathingSound.Stop();
            }
            goForBone = false;
            transform.rotation = startRotation;
            transform.GetChild(0).transform.localRotation = childStartRotation;
        }
    }

    /// <summary>
    /// tiez kontrolujeme okraje
    /// ak ale nie sme pri okraji pridame do listu styri smery, hore, dole, doprava a dolava
    /// tiez vyratame dva smery, ktore smeruju ku hracovy
    /// nahodnym vyberom vyberieme jedel smer z listu
    /// k danemu smeru doratame otocenie prveho dietate objektu ducha
    /// </summary>
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
            // ak hrac stupil na kostru, duch sa pohybuje priamo ku kostre
            // v danom pripade sa natoci cely objekt ku kostre a hybe sa priamo dopredu
            transform.LookAt(new Vector3(bonePosition.x, transform.position.y, bonePosition.z));
            dir = Vector3.forward;
        }
    }

    /// <summary>
    /// funkcia rata smery ducha k hracovy
    /// jedna sa iba o jednoduche porovnanie x a y suradnic
    /// </summary>
    /// <param name="dirs"></param>
    /// <returns></returns>
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

    /// <summary>
    /// pri kolizii ducha s hracom aktivujeme atak a hracovy odoberieme zivot
    /// ak by bol zivot posledny zobrazime screen GameOver
    /// </summary>
    /// <param name="other"></param>
    void OnTriggerEnter(Collider other)
    {
        if (other.name == "Person")
        {
            attack = true;
            attackSound.Play();
            screamSound.Play();
            player.GetLive();
            if (player.lives <= 0) player.gameover = true;
            blinkCounter = 0;
            blinkTime = 0;
        }
    }

    /// <summary>
    /// pri restarte hraca prenastavime startovaciu poziciu a vypneme vsetko potrebne
    /// </summary>
    public void ReloadGhost()
    {
        transform.position = startPosition;
        attack = false;
        blinkCounter = 0;
        blinkTime = 0;
    }

    /// <summary>
    /// fukcia vrati quaterion z eulerovho uhola pre dany smer
    /// </summary>
    /// <returns></returns>
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
