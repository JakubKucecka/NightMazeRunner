using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopHnadler : MonoBehaviour
{
    [SerializeField]
    Game game;

    [SerializeField]
    GameObject nightVission;
    [SerializeField]
    GameObject detector;

    [SerializeField]
    Text livesText;
    [SerializeField]
    Text batteryText;
    [SerializeField]
    Text nightVissionText;
    [SerializeField]
    Text detectorText;

    [SerializeField]
    Button livesButton;
    [SerializeField]
    Button batteryButton;
    [SerializeField]
    Button nightVissionButton;
    [SerializeField]
    Button detectorButton;

    public int livesPrice = 50;
    public int batteryPrice = 20;
    public int nightVissionPrice = 500;
    public int detectorPrice = 500;
    public int maxLives = 6;

    // Update is called once per frame
    void Update()
    {
        nightVission.SetActive(!game.gameData.nightVission);
        detector.SetActive(!game.gameData.detector);

        livesText.text = livesPrice + "";
        batteryText.text = batteryPrice + "";
        nightVissionText.text = nightVissionPrice + "";
        detectorText.text = detectorPrice + "";

        livesButton.interactable = CheckCoins(livesPrice) && game.player.lives < maxLives;
        livesButton.GetComponentInChildren<Text>().enabled = CheckCoins(livesPrice) && game.player.lives < maxLives;
        batteryButton.interactable = CheckCoins(batteryPrice) && game.player.energy < 100;
        batteryButton.GetComponentInChildren<Text>().enabled = CheckCoins(batteryPrice) && game.player.energy < 100;
        nightVissionButton.interactable = CheckCoins(nightVissionPrice);
        nightVissionButton.GetComponentInChildren<Text>().enabled = CheckCoins(nightVissionPrice);
        detectorButton.interactable = CheckCoins(detectorPrice);
        detectorButton.GetComponentInChildren<Text>().enabled = CheckCoins(detectorPrice);
    }

    public void buyLives()
    {
        if (CheckCoins(livesPrice) && game.player.lives < maxLives)
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

    public void buyNightVission()
    {
        if (CheckCoins(nightVissionPrice))
        {
            game.player.coins -= nightVissionPrice / 10;
            game.player.gameItems["nightVission"] = true;
            game.SaveGameDataToJSON();
        }
    }

    public void buyDetector()
    {
        if (CheckCoins(detectorPrice))
        {
            game.player.coins -= detectorPrice / 10;
            game.player.gameItems["detector"] = true;
            game.SaveGameDataToJSON();
        }
    }

    private bool CheckCoins(int price)
    {
        return price <= game.player.coins * 10 ? true : false;
    }
}
