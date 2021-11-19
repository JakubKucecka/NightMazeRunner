using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ghost : MonoBehaviour
{
    [SerializeField]
    Player player;

    [SerializeField]
    private int border = 25;
    public float speed = 4f;
    public Vector3 startPosition;

    float changeDirTime = 0;
    Vector3 dir;

    Vector3 up = new Vector3(1, 0, 0);
    Vector3 down = new Vector3(-1, 0, 0);
    Vector3 right = new Vector3(0, 0, 1);
    Vector3 left = new Vector3(0, 0, -1);

    // Start is called before the first frame update
    void Start()
    {
        startPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
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
    }

    public void GetDir()
    {
        List<Vector3> dirs = new List<Vector3>();

        if (transform.position.x <= border) dirs.Add(down);
        if (transform.position.x >= -1 * border) dirs.Add(up);
        if (transform.position.z <= border) dirs.Add(right);
        if (transform.position.z >= -1 * border) dirs.Add(left);

        dirs = getPlayerPosition(dirs);

        int index = Random.Range(0, dirs.Count);
        dir = dirs[index];
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
            player.GetLive();
        }
    }

    public void ReloadGhost()
    {
        transform.position = startPosition;
    }
}
