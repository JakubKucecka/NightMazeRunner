using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// script zastresuje kontrolu nad okrajovym kanvasom, kde vidime zivoty, energiu, mini mapu peniaze a ziskane objekty
/// </summary>
public class InfoHandler : MonoBehaviour
{
    [SerializeField]
    Player player;
    [SerializeField]
    Game game;
    [SerializeField]
    GameObject livePrefab;

    [SerializeField]
    GameObject liveSpavner;
    [SerializeField]
    Image energy;
    [SerializeField]
    Text energyText;
    [SerializeField]
    Text coins;
    [SerializeField]
    GameObject nightVission;
    [SerializeField]
    GameObject detector;

    [SerializeField]
    Image miniMap;
    [SerializeField]
    Sprite level3;
    [SerializeField]
    Sprite level5;
    [SerializeField]
    Image playerMapPosition;

    private List<GameObject> lives = new List<GameObject>();
    private float enegyMax = 300;
    private float energyWidth;

    /// <summary>
    /// pri starte sa nastavy pocet zivotov
    /// </summary>
    void Start()
    {
        for (var i = 0; i < player.lives; i++)
        {
            var newLive = Instantiate(livePrefab, liveSpavner.transform);
            newLive.transform.localPosition += getLiveMoves(i);
            lives.Add(newLive);
        }
    }

    /// <summary>
    /// v update funkcii sa run time upravuju vsetky hodnoty
    /// </summary>
    void Update()
    {
        // pridavanie alebo odstranovanie zivotov
        if (player.lives > lives.Count)
        {
            var newLive = Instantiate(livePrefab, liveSpavner.transform);
            newLive.transform.localPosition += getLiveMoves(lives.Count);
            lives.Add(newLive);
        }
        else if (player.lives < lives.Count && player.lives > 0)
        {
            GameObject.Destroy(lives[lives.Count - 1]);
            lives.Remove(lives[lives.Count - 1]);
        }

        // update penazy
        coins.text = $"{player.coins * 10}";

        // update ziskanych objektov
        detector.SetActive(player.gameItems != null && player.gameItems["detector"]);
        nightVission.SetActive(player.gameItems != null && player.gameItems["nightVission"]);
        if (miniMap != null)
        {
            if (game.level == 3)
            {
                miniMap.sprite = level3;
            }
            else if (game.level == 5)
            {
                miniMap.sprite = level5;
            }

            miniMap.gameObject.SetActive(player.useMiniMap);
            // rata sa pozicia bielej gulicky na mini mape podla pohybu hraca
            playerMapPosition.transform.localPosition =
                new Vector3(player.transform.position.z * 4.5f, -player.transform.position.x * 4.5f, playerMapPosition.transform.localPosition.z);
        }

        // update energii
        if (player.energy > 0 || player.energy < 100)
        {
            energyText.text = $"{Math.Round(player.energy, 2)}%";
            energyWidth = player.energy * enegyMax / 100;
        }
        else if (player.energy <= 0)
        {
            energyText.text = "0%";
            energyWidth = 0;
        }
        else if (player.energy <= 100)
        {
            energyText.text = "100%";
            energyWidth = 100;
        }
        // update energy obdlznika
        energy.rectTransform.sizeDelta = new Vector2(energyWidth, energy.rectTransform.sizeDelta.y);
    }

    /// <summary>
    /// zivoty produkuje live spawnr, preto potrebujeme kazdemu objektu vyratat konkretny posun
    /// </summary>
    /// <param name="count"></param>
    /// <returns></returns>
    Vector3 getLiveMoves(int count)
    {
        return Vector3.right * 80 * count;
    }
}
