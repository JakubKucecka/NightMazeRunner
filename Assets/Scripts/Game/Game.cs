using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game : MonoBehaviour
{
    public bool showMenu;

    [SerializeField]
    GameObject night;

    [SerializeField]
    GameObject itemSpavnerGO;

    public int level;
    public Dictionary<int, bool> unlockedLevels;

    public Player player;
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
        loadLevelItems();

        player = GetComponentInChildren<Player>();
        ghost = GetComponentInChildren<Ghost>();
        itemSpawner = itemSpavnerGO.GetComponentInChildren<ItemSpawner>();
        cameraHandler = GetComponentInChildren<CameraHandler>();

        //// for debug
        //level = 1;
        //loadLevel();

        // wait for create object player
        Invoke("LoadGameDataFromJSON", 0.5f);
        showMenu = true;
    }

    // Update is called once per frame
    void Update()
    {
        player.gameIsStarted = !showMenu;
        if (showMenu)
        {
            player.rotateControler.firstPerson = false;
            player.moveControler.firstPerson = false;
        }

        if (showMenu)
        {
            if (!Cursor.visible) Cursor.visible = true;
        }
        else
        {
            Cursor.visible = false;
        }

            if (Input.GetButtonDown("Restart"))
            {
                RestartGame();
            }

            if (Input.GetButtonDown("BackToMenu"))
            {
                showMenu = true;
            }

            if (Input.GetButtonDown("SwitchLight"))
            {
                player.changeUseLight();
            }

            if (Input.GetButtonDown("SwitchGloves"))
            {
                cameraHandler.changeUseGlasses();
            }

            if (Input.GetButtonDown("SwitchDetector"))
            {
                player.changeUseDetector();
            }

            if (Input.GetButtonDown("ChangeCameras"))
            {
                cameraHandler.changeCamera();
            }
    }

    public void RestartGame()
    {
        loadLevel();
    }

    public void loadLevelItems()
    {
        var levelPositions = new Dictionary<GameObject, List<Vector3>>();

        // level 1
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

        levelPositions.Add(glassPrefab, new List<Vector3>());
        levelPositions[glassPrefab].Add(new Vector3(-5f, 6f, -25f));

        itemsByLevels.Add(1, levelPositions);
        levelPositions = new Dictionary<GameObject, List<Vector3>>();

        // level 2
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

        levelPositions.Add(detectorPrefab, new List<Vector3>());
        levelPositions[detectorPrefab].Add(new Vector3(-5f, 6f, 25f));

        itemsByLevels.Add(2, levelPositions);
        levelPositions = new Dictionary<GameObject, List<Vector3>>();

        // level 3
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

        levelPositions.Add(mapPrefab, new List<Vector3>());
        levelPositions[mapPrefab].Add(new Vector3(25f, 6f, 25f));

        itemsByLevels.Add(3, levelPositions);
        levelPositions = new Dictionary<GameObject, List<Vector3>>();
    }

    public void loadLevel()
    {
        // TODO: from JSON
        player.coins = 0;
        player.lives = 3;
        player.energy = 100;
        ghost.ReloadGhost();
        player.RestartPlayer();

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

    public void unlockNext()
    {
        if (unlockedLevels.ContainsKey(level + 1))
        {
            unlockedLevels[level + 1] = true;
        } else
        {
            unlockedLevels.Add(level + 1, true);
        }
    }

    public void lockAll()
    {
        foreach (var key in unlockedLevels.Keys)
        {
            if (key == 1) continue;

            unlockedLevels[key] = false;
        }
    }

    private void LoadGameDataFromJSON()
    {
        // TODO: save to public object

        unlockedLevels = new Dictionary<int, bool>();
        unlockedLevels.Add(1, true);
        //unlockedLevels.Add(2, true);
        //unlockedLevels.Add(3, true);

        player.gameItems = new Dictionary<string, bool>();
        player.gameItems.Add("glasses", false);
        player.gameItems.Add("detector", false);
    }

    private void SaveGameDataToJSON()
    {
        // TODO: save to public object

        // TODO: save to JSON
    }
}
