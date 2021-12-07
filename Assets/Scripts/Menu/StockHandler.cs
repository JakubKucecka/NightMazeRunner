using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// zastresuje spravu vylepseni
/// </summary>
public class StockHandler : MonoBehaviour
{
    public int price = 100;
    public int lightPrice;
    public int nightVissionPrice;
    public int detectorPrice;

    [SerializeField]
    Game game;

    [SerializeField]
    GameObject nightVission;
    [SerializeField]
    GameObject detector;

    [SerializeField]
    Text lightText;
    [SerializeField]
    Text nightVissionText;
    [SerializeField]
    Text detectorText;

    [SerializeField]
    Text lightLevelText;
    [SerializeField]
    Text nightVissionLevelText;
    [SerializeField]
    Text detectorLevelText;

    [SerializeField]
    Button lightButton;
    [SerializeField]
    Button nightVissionButton;
    [SerializeField]
    Button detectorButton;

    private int maxLevel = 5;

    /// <summary>
    /// rovnako ako v obchode aj tu kontrolujeme, ktory blok mozeme zobrazit
    /// </summary>
    void Update()
    {
        nightVission.SetActive(game.gameData.nightVission);
        detector.SetActive(game.gameData.detector);

        lightPrice = price * (game.gameData.lightLevel + 1);
        nightVissionPrice = price * (game.gameData.nightVissionLevel + 1);
        detectorPrice = price * (game.gameData.detectorLevel + 1);

        lightText.text = lightPrice + "";
        nightVissionText.text = nightVissionPrice + "";
        detectorText.text = detectorPrice + "";

        lightLevelText.text = "LEVEL: " + game.gameData.lightLevel;
        nightVissionLevelText.text = "LEVEL: " + game.gameData.nightVissionLevel;
        detectorLevelText.text = "LEVEL: " + game.gameData.detectorLevel;

        lightButton.interactable = CheckCoins(lightPrice) && game.gameData.lightLevel < maxLevel;
        lightButton.GetComponentInChildren<Text>().enabled = CheckCoins(lightPrice) && game.gameData.lightLevel < maxLevel;
        nightVissionButton.interactable = CheckCoins(nightVissionPrice) && game.gameData.nightVissionLevel < maxLevel;
        nightVissionButton.GetComponentInChildren<Text>().enabled = CheckCoins(nightVissionPrice) && game.gameData.nightVissionLevel < maxLevel;
        detectorButton.interactable = CheckCoins(detectorPrice) && game.gameData.detectorLevel < maxLevel;
        detectorButton.GetComponentInChildren<Text>().enabled = CheckCoins(detectorPrice) && game.gameData.detectorLevel < maxLevel;
    }

    public void updateLight()
    {
        if (CheckCoins(lightPrice) && game.gameData.lightLevel < maxLevel)
        {
            game.player.coins -= lightPrice / 10;
            game.gameData.lightLevel += 1;
            game.SaveGameDataToJSON();
        }
    }

    public void updatenightVission()
    {
        if (CheckCoins(nightVissionPrice) && game.gameData.nightVissionLevel < maxLevel)
        {
            game.player.coins -= nightVissionPrice / 10;
            game.gameData.nightVissionLevel += 1;
            game.SaveGameDataToJSON();
        }
    }

    public void updateDetector()
    {
        if (CheckCoins(detectorPrice) && game.gameData.detectorLevel < maxLevel)
        {
            game.player.coins -= detectorPrice / 10;
            game.gameData.detectorLevel += 1;
            game.SaveGameDataToJSON();
        }
    }

    /// <summary>
    /// tiez stale kontrolujeme ci mame dostatok penazi na vylepsenie objektu 
    /// </summary>
    /// <param name="price"></param>
    /// <returns></returns>
    private bool CheckCoins(int price)
    {
        return price <= game.player.coins * 10 ? true : false;
    }
}
