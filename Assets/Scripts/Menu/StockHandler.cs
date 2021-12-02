using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StockHandler : MonoBehaviour
{
    [SerializeField]
    Game game;

    [SerializeField]
    GameObject gloves;
    [SerializeField]
    GameObject detector;

    [SerializeField]
    Text lightText;
    [SerializeField]
    Text glovesText;
    [SerializeField]
    Text detectorText;

    [SerializeField]
    Text lightLevelText;
    [SerializeField]
    Text glovesLevelText;
    [SerializeField]
    Text detectorLevelText;

    [SerializeField]
    Button lightButton;
    [SerializeField]
    Button glovesButton;
    [SerializeField]
    Button detectorButton;

    public int price = 100;
    public int lightPrice;
    public int glovesPrice;
    public int detectorPrice;

    private int maxLevel = 5;

    // Update is called once per frame
    void Update()
    {
        gloves.SetActive(game.gameData.gloves);
        detector.SetActive(game.gameData.detector);

        lightPrice = price * (game.gameData.lightLevel + 1);
        glovesPrice = price * (game.gameData.glovesLevel + 1);
        detectorPrice = price * (game.gameData.detectorLevel + 1);

        lightText.text = lightPrice + " coins";
        glovesText.text = glovesPrice + " coins";
        detectorText.text = detectorPrice + " coins";

        lightLevelText.text = "LEVEL: " + game.gameData.lightLevel;
        glovesLevelText.text = "LEVEL: " + game.gameData.glovesLevel;
        detectorLevelText.text = "LEVEL: " + game.gameData.detectorLevel;

        lightButton.interactable = CheckCoins(lightPrice) && game.gameData.lightLevel < maxLevel;
        lightButton.GetComponentInChildren<Text>().enabled = CheckCoins(lightPrice) && game.gameData.lightLevel < maxLevel;
        glovesButton.interactable = CheckCoins(glovesPrice) && game.gameData.glovesLevel < maxLevel;
        glovesButton.GetComponentInChildren<Text>().enabled = CheckCoins(glovesPrice) && game.gameData.glovesLevel < maxLevel;
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

    public void updateGloves()
    {
        if (CheckCoins(glovesPrice) && game.gameData.glovesLevel < maxLevel)
        {
            game.player.coins -= glovesPrice / 10;
            game.gameData.glovesLevel += 1;
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

    private bool CheckCoins(int price)
    {
        return price <= game.player.coins * 10 ? true : false;
    }
}
