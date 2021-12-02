using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopHnadler : MonoBehaviour
{
    [SerializeField]
    Game game;

    [SerializeField]
    GameObject gloves;
    [SerializeField]
    GameObject detector;

    [SerializeField]
    Text livesText;
    [SerializeField]
    Text batteryText;
    [SerializeField]
    Text glovesText;
    [SerializeField]
    Text detectorText;

    [SerializeField]
    Button livesButton;
    [SerializeField]
    Button batteryButton;
    [SerializeField]
    Button glovesButton;
    [SerializeField]
    Button detectorButton;

    public int livesPrice = 50;
    public int batteryPrice = 20;
    public int glovesPrice = 500;
    public int detectorPrice = 500;

    // Update is called once per frame
    void Update()
    {
        gloves.SetActive(!game.gameData.gloves);
        detector.SetActive(!game.gameData.detector);

        livesText.text = livesPrice + " coins";
        batteryText.text = batteryPrice + " coins";
        glovesText.text = glovesPrice + " coins";
        detectorText.text = detectorPrice + " coins";

        livesButton.interactable = CheckCoins(livesPrice) && game.player.lives < 5;
        livesButton.GetComponentInChildren<Text>().enabled = CheckCoins(livesPrice) && game.player.lives < 5;
        batteryButton.interactable = CheckCoins(batteryPrice) && game.player.energy < 100;
        batteryButton.GetComponentInChildren<Text>().enabled = CheckCoins(batteryPrice) && game.player.energy < 100;
        glovesButton.interactable = CheckCoins(glovesPrice);
        glovesButton.GetComponentInChildren<Text>().enabled = CheckCoins(glovesPrice);
        detectorButton.interactable = CheckCoins(detectorPrice);
        detectorButton.GetComponentInChildren<Text>().enabled = CheckCoins(detectorPrice);
    }

    public void buyLives()
    {
        if (CheckCoins(livesPrice) && game.player.lives < 5)
        {
            game.player.coins -= livesPrice / 10;
            game.player.AddLive();
            game.SaveGameDataToJSON();
        }
    }

    public void buyBattery()
    {
        if (CheckCoins(batteryPrice) && game.player.energy < 100)
        {
            game.player.coins -= batteryPrice / 10;
            game.player.AddEnergy();
            game.SaveGameDataToJSON();
        }
    }

    public void buyGloves()
    {
        if (CheckCoins(glovesPrice))
        {
            game.player.coins -= glovesPrice / 10;
            game.gameData.gloves = true;
            game.SaveGameDataToJSON();
        }
    }

    public void buyDetector()
    {
        if (CheckCoins(detectorPrice))
        {
            game.player.coins -= detectorPrice / 10;
            game.gameData.detector = true;
            game.SaveGameDataToJSON();
        }
    }

    private bool CheckCoins(int price)
    {
        return price <= game.player.coins * 10 ? true : false;
    }
}
