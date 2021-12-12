using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

/// <summary>
/// script zastresuje kontrolu nad vacsinou objektov hry
/// </summary>
public class Game : MonoBehaviour
{
    public bool showMenu;

    public int maxLevel;
    public int level;
    public Dictionary<int, bool> unlockedLevels;
    public List<GameObject> levels = new List<GameObject>();

    public Dictionary<int, Dictionary<GameObject, List<Vector3>>> itemsByLevels = new Dictionary<int, Dictionary<GameObject, List<Vector3>>>();
    public JsonGameData gameData;

    public Player player;
    public Ghost ghost;

    public GameObject batteryPrefab;
    public GameObject coinPrefab;
    public GameObject nightVissionPrefab;
    public GameObject detectorPrefab;
    public GameObject mapPrefab;

    [SerializeField]
    Canvas infoCanvas;
    [SerializeField]
    GameObject night;
    [SerializeField]
    CameraHandler cameraHandler;
    [SerializeField]
    ItemSpawner itemSpawner;

    private string dataPath;

    /// <summary>
    /// pri starte triedy sa inicializuje pocet levelov podla nacitanych hernych objektov levelov
    /// inicializuje sa aj JSON objekt, cesta kam sa objekt ulozi a nacitaju sa objekty do levelov
    /// </summary>
    void Start()
    {
        maxLevel = levels.Count;
        dataPath = Application.persistentDataPath + "/game_data";
        gameData = new JsonGameData();
        loadLevelItems();
    }

    /// <summary>
    /// v update sa vykonava viacero akcii
    /// </summary>
    void Update()
    {
        // update caka na inicializaciu hraca, kedze je v hierarchii nizsie, az potom sa upravy JSON objekt
        if (player != null && player.started)
        {
            LoadGameDataFromJSON();
            showMenu = true;
            player.started = false;
        }

        player.gameIsStarted = !showMenu;
        if (showMenu)
        {
            // ak sa zobrazi manu vypne sa first person camera ak treba a zapne sa kurzor
            player.rotateControler.firstPerson = false;
            player.moveControler.firstPerson = false;
            if (!Cursor.visible) Cursor.visible = true;
        }
        else
        {
            // kontroluje sa stlacenie tlacidiel, ktore su zadefinovane v InputManagery
            // ak nie je menu tak sa kurzor vypne a reagujeme na stlacenie klaves
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

        // na stlacenie Esc sa vieme vratit na hlavnu stranku menu zo vsadial
        if (Input.GetButtonDown("BackToMenu"))
        {
            if (player.useMiniMap) player.useMiniMap = false;
            RestartGame();
            showMenu = true;
        }

        // tiez si sem prenasame levely jednotlivych objektov
        player.detectorLevel = gameData.detectorLevel;
        player.lightLevel = gameData.lightLevel;
        cameraHandler.nightVissionLevel = gameData.nightVissionLevel;
    }

    /// <summary>
    /// dana funkcia iba obnovy dany level
    /// </summary>
    public void RestartGame()
    {
        loadLevel();
    }

    /// <summary>
    /// tu su zadefinovane suardnice hernych objektov (batrky, penazy, mini mapy, detektoru a okuliarov) pre konkretne levely
    /// tie sa ulozia do daneho objektu aby boli pripravene pre inicializaciu v spravnom momente
    /// </summary>
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
        levelPositions[batteryPrefab].Add(new Vector3(-15f, 6f, -5f));
        levelPositions[batteryPrefab].Add(new Vector3(25f, 6f, -5f));

        levelPositions.Add(mapPrefab, new List<Vector3>());
        levelPositions[mapPrefab].Add(new Vector3(25f, 6f, 15f));

        itemsByLevels.Add(5, levelPositions);
        levelPositions = new Dictionary<GameObject, List<Vector3>>();
    }

    /// <summary>
    /// funkcia zabezpecuje nacitanie konkretneho levelu
    /// </summary>
    public void loadLevel()
    {
        // pre istotu na zaciatku nacitame herne udaje a restartujeme hraca a ducha
        LoadGameDataFromJSON();
        ghost.ReloadGhost();
        player.RestartPlayer();
        cameraHandler.reloadCameras();

        // nastavenie tmy na vektor 0,0,0
        night.transform.localPosition = Vector3.zero;

        // rovnaka operacia pre konkretny level ako pri tme
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

        // zobrazime konkretne objekty pre konkretny level, ktore sme si skor nacitali
        itemSpawner.ReloadItems(itemsByLevels[level]);
        ghost.speed = ghost.startSpeed + (1f * level);
    }

    /// <summary>
    /// v globalnej premennej ma ulozeny aktualny level
    /// vdaka nemu overi ci existuje este dalsi level
    /// ak ano tak ho odmkne inak nerobi nic
    /// tiez vypina minimapu
    /// </summary>
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

    /// <summary>
    /// hrac ma moznost zacat od zaciatku alebo sa moze stat, ze ho prisera zabije
    /// v danom pripade nastane reset dat, ktory obnovi JSON a ulozi ho
    /// </summary>
    public void ResetGameData()
    {
        gameData = new JsonGameData();
        string json = JsonUtility.ToJson(gameData);
        Obfuscate(dataPath, json);
        LoadGameDataFromJSON();
    }

    /// <summary>
    /// funkcia nacita JSON objekt zo suboru
    /// obsah je ale zakodovnay aby nebol citatelny pre bezneho pouzivatela, preto ho najpr odkoduje a potom prida do objektu
    /// z daneho objektu sa inicializuju herne atributy jednotlivych objektov
    /// </summary>
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

    /// <summary>
    /// ukladanie funguje naopak ako nacitanie
    /// vytvorime JSON z atributov jednotlivych objektov a ten zakodujeme a ulozime do suboru
    /// </summary>
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

    /// <summary>
    /// fukcia zakoduje string a ulozi ho do suboru
    /// zdroj tohto kodu je: https://stackoverflow.com/questions/13980207/c-sharp-file-encoding-and-decoding-unreadable#answer-13980571
    /// </summary>
    /// <param name="fileName"></param>
    /// <param name="data"></param>
    void Obfuscate(string fileName, string data)
    {
        var bytes = Encoding.UTF8.GetBytes(data);
        for (int i = 0; i < bytes.Length; i++) bytes[i] ^= 0x5a;
        File.WriteAllText(fileName, Convert.ToBase64String(bytes));
    }

    /// <summary>
    /// dana funkcia nacita obsah suboru a odkoduje ho
    /// zdroj tohto kodu je: https://stackoverflow.com/questions/13980207/c-sharp-file-encoding-and-decoding-unreadable#answer-13980571
    /// </summary>
    /// <param name="fileName"></param>
    /// <returns></returns>
    string Deobfuscate(string fileName)
    {
        var bytes = Convert.FromBase64String(File.ReadAllText(fileName));
        for (int i = 0; i < bytes.Length; i++) bytes[i] ^= 0x5a;
        return Encoding.UTF8.GetString(bytes);
    }

    /// <summary>
    /// serrializovatelny objekt v podobe JSON-u
    /// </summary>
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
