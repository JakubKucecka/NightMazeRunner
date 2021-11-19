using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game : MonoBehaviour
{
    [SerializeField]
    GameObject night;

    [SerializeField]
    GameObject itemSpavnerGO;

    public int level;
    public Dictionary<int, bool> unlockedLevels;
    float lastTime;

    Player player;
    Ghost ghost;
    ItemSpawner itemSpawner;
    CameraHandler cameraHandler;

    public Dictionary<int, Dictionary<GameObject, List<Vector3>>> itemsByLevels = new Dictionary<int, Dictionary<GameObject, List<Vector3>>>();

    // prefabs ganme items
    public GameObject batteryPrefab;
    public GameObject coinPrefab;
    public GameObject glassPrefab;
    public GameObject detectorPrefab;
    public GameObject mapPrefab;

    // Start is called before the first frame update
    void Start()
    {
        lastTime = Time.time - 0.5f;
        loadLevelItems();

        player = GetComponentInChildren<Player>();
        ghost = GetComponentInChildren<Ghost>();
        itemSpawner = itemSpavnerGO.GetComponentInChildren<ItemSpawner>();
        cameraHandler = GetComponentInChildren<CameraHandler>();

        // for debug
        level = 1;
        loadLevel();

        // wait for create object player
        Invoke("LoadGameDataFromJSON", 2);
    }

    // Update is called once per frame
    void Update()
    {
        Cursor.visible = false;

        if (Time.time - lastTime > 0.5f)
        {
            if (Input.GetKey(KeyCode.R))
            {
                RestartGame();
                lastTime = Time.time;
            }

            if (Input.GetKey(KeyCode.Escape))
            {
                // show menu
                lastTime = Time.time;
            }

            if (Input.GetKey(KeyCode.Alpha1))
            {
                player.changeUseLight();
                lastTime = Time.time;
            }

            if (Input.GetKey(KeyCode.Alpha2))
            {
                cameraHandler.changeUseGlasses();
                lastTime = Time.time;
            }

            if (Input.GetKey(KeyCode.Alpha3))
            {
                // useDetector
                lastTime = Time.time;
            }

            if (Input.GetKey(KeyCode.C))
            {
                cameraHandler.changeCamera();
                lastTime = Time.time;
            }
        }
    }

    public void RestartGame()
    {
        loadLevel();
        ghost.ReloadGhost();
        player.RestartPlayer();
    }

    public void loadLevelItems()
    {
        unlockedLevels = new Dictionary<int, bool>();

        var levelPositions = new Dictionary<GameObject, List<Vector3>>();

        // level 1
        unlockedLevels.Add(1, true);
        levelPositions.Add(coinPrefab, new List<Vector3>());
        levelPositions[coinPrefab].Add(new Vector3(15f, 6f, 15f));
        levelPositions[coinPrefab].Add(new Vector3(-25f, 6f, 15f));
        levelPositions[coinPrefab].Add(new Vector3(-5f, 6f, 15f));
        levelPositions[coinPrefab].Add(new Vector3(15f, 6f, -15f));
        levelPositions[coinPrefab].Add(new Vector3(-25f, 6f, -15f));

        levelPositions.Add(batteryPrefab, new List<Vector3>());
        levelPositions[batteryPrefab].Add(new Vector3(-15f, 6f, -25f));
        levelPositions[batteryPrefab].Add(new Vector3(-15f, 6f, 25f));
        levelPositions[batteryPrefab].Add(new Vector3(5f, 6f, -5f));

        itemsByLevels.Add(1, levelPositions);
        levelPositions = new Dictionary<GameObject, List<Vector3>>();

        // level 2
        unlockedLevels.Add(2, true);
        levelPositions.Add(coinPrefab, new List<Vector3>());
        levelPositions[coinPrefab].Add(new Vector3(25f, 6f, -25f));
        levelPositions[coinPrefab].Add(new Vector3(-15f, 6f, -15f));
        levelPositions[coinPrefab].Add(new Vector3(15f, 6f, 15f));
        levelPositions[coinPrefab].Add(new Vector3(-5f, 6f, 5f));
        levelPositions[coinPrefab].Add(new Vector3(5f, 6f, -25f));

        levelPositions.Add(batteryPrefab, new List<Vector3>());
        levelPositions[batteryPrefab].Add(new Vector3(-5f, 6f, -15f));
        levelPositions[batteryPrefab].Add(new Vector3(-15f, 6f, 15f));
        levelPositions[batteryPrefab].Add(new Vector3(25f, 6f, 25f));

        itemsByLevels.Add(2, levelPositions);
        levelPositions = new Dictionary<GameObject, List<Vector3>>();

        // level 3
        unlockedLevels.Add(3, true);
        levelPositions.Add(coinPrefab, new List<Vector3>());
        levelPositions[coinPrefab].Add(new Vector3(5f, 6f, -5f));
        levelPositions[coinPrefab].Add(new Vector3(-5.1f, 6f, -15f));
        levelPositions[coinPrefab].Add(new Vector3(-25f, 6f, -15f));
        levelPositions[coinPrefab].Add(new Vector3(-15f, 6f, 15f));
        levelPositions[coinPrefab].Add(new Vector3(15f, 6f, 15f));

        levelPositions.Add(batteryPrefab, new List<Vector3>());
        levelPositions[batteryPrefab].Add(new Vector3(25f, 6f, -25f));
        levelPositions[batteryPrefab].Add(new Vector3(-15f, 6f, 5f));
        levelPositions[batteryPrefab].Add(new Vector3(5f, 6f, 25f));

        itemsByLevels.Add(3, levelPositions);
        levelPositions = new Dictionary<GameObject, List<Vector3>>();
    }

    public void loadLevel()
    {
        // TODO: from JSON
        player.coins = 0;
        player.lives = 3;
        player.energy = 100;

        night.transform.localPosition = Vector3.zero;

        GameObject[] levels = GameObject.FindGameObjectsWithTag("Level");

        for (var i = 0; i < levels.Length; i++)
        {
            if (i == level - 1)
            {
                levels[i].transform.position = Vector3.zero;
            }
            else
            {
                levels[i].transform.position = Vector3.forward * 300f;
            }
        }

        itemSpawner.ReloadItems(itemsByLevels[level]);
    }

    private void LoadGameDataFromJSON()
    {
        // TODO: save to public object

        var JSONLevels = new Dictionary<int, bool>();

        foreach (var k in JSONLevels.Keys)
        {
            unlockedLevels[k] = JSONLevels[k];
        }
    }

    private void SaveGameDataToJSON()
    {
        // TODO: save to public object

        // TODO: save to JSON
    }
}
