using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class Game : MonoBehaviour
{
    public bool showMenu;
    public int maxLevel = 3;

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

    string dataPath;
    public JsonGameData gameData;
    public List<GameObject> levels = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        dataPath = Application.persistentDataPath + "/game_data.json";
        Debug.Log(dataPath);
        gameData = new JsonGameData();
        loadLevelItems();

        player = GetComponentInChildren<Player>();
        ghost = GetComponentInChildren<Ghost>();
        itemSpawner = itemSpavnerGO.GetComponentInChildren<ItemSpawner>();
        cameraHandler = GetComponentInChildren<CameraHandler>();
    }

    // Update is called once per frame
    void Update()
    {
        if (player != null && player.started)
        {
            LoadGameDataFromJSON();
            showMenu = true;
            player.started = false;
        }

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
            if (player.useMiniMap) player.useMiniMap = false;
            RestartGame();
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

        player.detectorLevel = gameData.detectorLevel;
        player.lightLevel = gameData.lightLevel;
        cameraHandler.glovesLevel = gameData.glovesLevel;
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
        LoadGameDataFromJSON();
        ghost.ReloadGhost();
        player.RestartPlayer();

        night.transform.localPosition = Vector3.zero;

        for (var i = 0; i < levels.Count; i++)
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
        ghost.speed = ghost.startSpeed + (1f * level);
    }

    public void unlockNext()
    {
        if (player.useMiniMap) player.useMiniMap = false;

        if ( level < maxLevel)
        {
            if (unlockedLevels.ContainsKey(level + 1))
            {
                unlockedLevels[level + 1] = true;
            }
            else
            {
                unlockedLevels.Add(level + 1, true);
            }
        }
    }

    public void GameOver()
    {
        gameData = new JsonGameData();
        string json = JsonUtility.ToJson(gameData);
        File.WriteAllText(dataPath, json);
        LoadGameDataFromJSON();
    }

    public void LoadGameDataFromJSON()
    {
        if (File.Exists(dataPath))
        {
            string json = File.ReadAllText(dataPath);
            gameData = JsonUtility.FromJson<JsonGameData>(json);
        }

        unlockedLevels = new Dictionary<int, bool>();
        foreach (var i in gameData.unlockLevels)
        {
            unlockedLevels.Add(i, true);
        }

        player.gameItems = new Dictionary<string, bool>();
        player.gameItems.Add("glasses", gameData.gloves);
        player.gameItems.Add("detector", gameData.detector);

        player.lives = gameData.lives;
        player.energy = gameData.energy;
        player.coins = gameData.coins;
    }

    public void SaveGameDataToJSON()
    {
        var tmpLevels = new List<int>();
        foreach (var i in unlockedLevels)
        {
            if (i.Value) tmpLevels.Add(i.Key);
        }

        gameData.unlockLevels = tmpLevels.ToArray();

        gameData.gloves = player.gameItems["glasses"];
        gameData.detector = player.gameItems["detector"];

        gameData.lives = player.lives;
        gameData.energy = player.energy;
        gameData.coins = player.coins;

        string json = JsonUtility.ToJson(gameData);
        File.WriteAllText(dataPath, json);
    }

    [Serializable]
    public class JsonGameData
    {
        public float energy;
        public int lives;
        public int coins;
        public int lightLevel;
        public bool gloves;
        public int glovesLevel;
        public bool detector;
        public int detectorLevel;
        public int[] unlockLevels;

        public JsonGameData(float iEnergy, int iLives, int iCoins, int iLightLevel, bool iGloves, int iGlovesLevel, bool iDetector, int iDetectorLevel, int[] iUnlockLevels)
        {
            energy = iEnergy;
            lives = iLives;
            coins = iCoins;
            lightLevel = iLightLevel;
            gloves = iGloves;
            glovesLevel = iGlovesLevel;
            detector = iDetector;
            detectorLevel = iDetectorLevel;
            unlockLevels = iUnlockLevels;
        }

        public JsonGameData()
        {
            energy = 100;
            lives = 3;
            coins = 50;
            lightLevel = 1;
            gloves = false;
            glovesLevel = 1;
            detector = false;
            detectorLevel = 1;
            unlockLevels = new int[] { 1 };
        }
    }
}
