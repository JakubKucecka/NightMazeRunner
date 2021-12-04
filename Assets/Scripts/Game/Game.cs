using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

public class Game : MonoBehaviour
{
    public bool showMenu;
    public int maxLevel;
    public Canvas infoCanvas;

    [SerializeField]
    GameObject night;

    [SerializeField]
    GameObject itemSpavnerGO;

    public int level;
    public Dictionary<int, bool> unlockedLevels;

    public Player player;
    public Ghost ghost;
    ItemSpawner itemSpawner;
    CameraHandler cameraHandler;

    public Dictionary<int, Dictionary<GameObject, List<Vector3>>> itemsByLevels = new Dictionary<int, Dictionary<GameObject, List<Vector3>>>();

    // prefabs ganme items
    public GameObject batteryPrefab;
    public GameObject coinPrefab;
    public GameObject nightVissionPrefab;
    public GameObject detectorPrefab;
    public GameObject mapPrefab;

    string dataPath;
    public JsonGameData gameData;
    public List<GameObject> levels = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        maxLevel = levels.Count;
        dataPath = Application.persistentDataPath + "/game_data";
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

            if (Input.GetButtonDown("Restart"))
            {
                RestartGame();
            }

            if (Input.GetButtonDown("SwitchLight"))
            {
                player.changeUseLight();
                infoCanvas.gameObject.SetActive(false);
            }

            if (Input.GetButtonDown("SwitchNightVission"))
            {
                cameraHandler.changeUseNightVission();
                infoCanvas.gameObject.SetActive(false);
            }

            if (Input.GetButtonDown("SwitchDetector"))
            {
                player.changeUseDetector();
                infoCanvas.gameObject.SetActive(false);
            }

            if (Input.GetButtonDown("ChangeCameras"))
            {
                cameraHandler.changeCamera();
                infoCanvas.gameObject.SetActive(false);
            }
        }

        if (Input.GetButtonDown("BackToMenu"))
        {
            if (player.useMiniMap) player.useMiniMap = false;
            RestartGame();
            showMenu = true;
        }

        player.detectorLevel = gameData.detectorLevel;
        player.lightLevel = gameData.lightLevel;
        cameraHandler.nightVissionLevel = gameData.nightVissionLevel;
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

        levelPositions.Add(nightVissionPrefab, new List<Vector3>());
        levelPositions[nightVissionPrefab].Add(new Vector3(-5f, 6f, -25f));

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

        // level 4
        levelPositions.Add(coinPrefab, new List<Vector3>());
        levelPositions[coinPrefab].Add(new Vector3(25f, 6f, 25f));
        levelPositions[coinPrefab].Add(new Vector3(-15f, 6f, 5f));
        levelPositions[coinPrefab].Add(new Vector3(-5f, 6f, -25f));
        levelPositions[coinPrefab].Add(new Vector3(5f, 6f, 5f));
        levelPositions[coinPrefab].Add(new Vector3(5f, 6f, 25f));
        levelPositions[coinPrefab].Add(new Vector3(15f, 6f, -5f));
        levelPositions[coinPrefab].Add(new Vector3(25f, 6f, -15f));

        levelPositions.Add(batteryPrefab, new List<Vector3>());
        levelPositions[batteryPrefab].Add(new Vector3(-5f, 6f, 15f));
        levelPositions[batteryPrefab].Add(new Vector3(-25f, 6f, -25f));
        levelPositions[batteryPrefab].Add(new Vector3(15f, 6f, -25f));

        itemsByLevels.Add(4, levelPositions);
        levelPositions = new Dictionary<GameObject, List<Vector3>>();

        // level 5
        levelPositions.Add(coinPrefab, new List<Vector3>());
        levelPositions[coinPrefab].Add(new Vector3(-25f, 6f, -15f));
        levelPositions[coinPrefab].Add(new Vector3(-25f, 6f, 5f));
        levelPositions[coinPrefab].Add(new Vector3(-15f, 6f, -25f));
        levelPositions[coinPrefab].Add(new Vector3(-15f, 6f, 25f));
        levelPositions[coinPrefab].Add(new Vector3(-5f, 6f, 5f));
        levelPositions[coinPrefab].Add(new Vector3(25f, 6f, 25f));

        levelPositions.Add(batteryPrefab, new List<Vector3>());
        levelPositions[batteryPrefab].Add(new Vector3(5f, 6f, -15f));
        levelPositions[batteryPrefab].Add(new Vector3(-15f, 6f, 15f));
        levelPositions[batteryPrefab].Add(new Vector3(25f, 6f, -5f));

        levelPositions.Add(mapPrefab, new List<Vector3>());
        levelPositions[mapPrefab].Add(new Vector3(25f, 6f, 15f));

        itemsByLevels.Add(5, levelPositions);
        levelPositions = new Dictionary<GameObject, List<Vector3>>();
    }

    public void loadLevel()
    {
        LoadGameDataFromJSON();
        ghost.ReloadGhost();
        player.RestartPlayer();
        infoCanvas.gameObject.SetActive(true);

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

        if (level < maxLevel)
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

    public void ResetGameData()
    {
        gameData = new JsonGameData();
        string json = JsonUtility.ToJson(gameData);
        Obfuscate(dataPath, json);
        LoadGameDataFromJSON();
    }

    public void LoadGameDataFromJSON()
    {
        if (File.Exists(dataPath))
        {
            gameData = JsonUtility.FromJson<JsonGameData>(Deobfuscate(dataPath));
        }

        unlockedLevels = new Dictionary<int, bool>();
        foreach (var i in gameData.unlockLevels)
        {
            unlockedLevels.Add(i, true);
        }

        player.gameItems = new Dictionary<string, bool>();
        player.gameItems.Add("nightVission", gameData.nightVission);
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

        gameData.nightVission = player.gameItems["nightVission"];
        gameData.detector = player.gameItems["detector"];

        gameData.lives = player.lives;
        gameData.energy = player.energy;
        gameData.coins = player.coins;

        string json = JsonUtility.ToJson(gameData);
        Obfuscate(dataPath, json);
    }

    void Obfuscate(string fileName, string data)
    {
        var bytes = Encoding.UTF8.GetBytes(data);
        for (int i = 0; i < bytes.Length; i++) bytes[i] ^= 0x5a;
        File.WriteAllText(fileName, Convert.ToBase64String(bytes));
    }

    string Deobfuscate(string fileName)
    {
        var bytes = Convert.FromBase64String(File.ReadAllText(fileName));
        for (int i = 0; i < bytes.Length; i++) bytes[i] ^= 0x5a;
        return Encoding.UTF8.GetString(bytes);
    }

    [Serializable]
    public class JsonGameData
    {
        public float energy;
        public int lives;
        public int coins;
        public int lightLevel;
        public bool nightVission;
        public int nightVissionLevel;
        public bool detector;
        public int detectorLevel;
        public int[] unlockLevels;

        public JsonGameData(float iEnergy, int iLives, int iCoins, int iLightLevel, bool iNightVission, int iNightVissionLevel, bool iDetector, int iDetectorLevel, int[] iUnlockLevels)
        {
            energy = iEnergy;
            lives = iLives;
            coins = iCoins;
            lightLevel = iLightLevel;
            nightVission = iNightVission;
            nightVissionLevel = iNightVissionLevel;
            detector = iDetector;
            detectorLevel = iDetectorLevel;
            unlockLevels = iUnlockLevels;
        }

        public JsonGameData()
        {
            energy = 100;
            lives = 5;
            coins = 50;
            lightLevel = 1;
            nightVission = false;
            nightVissionLevel = 1;
            detector = false;
            detectorLevel = 1;
            unlockLevels = new int[] { 1 };
        }
    }
}
