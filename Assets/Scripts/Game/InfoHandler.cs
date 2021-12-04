using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
    GameObject gloves;
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

    // Start is called before the first frame update
    void Start()
    {
        for (var i = 0; i < player.lives; i++)
        {
            var newLive = Instantiate(livePrefab, liveSpavner.transform);
            newLive.transform.localPosition += getLiveMoves(i);
            lives.Add(newLive);
        }
    }

    // Update is called once per frame
    void Update()
    {
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

        coins.text = $"{player.coins * 10}";

        detector.SetActive(player.gameItems != null && player.gameItems["detector"]);
        gloves.SetActive(player.gameItems != null && player.gameItems["glasses"]);
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
            playerMapPosition.transform.localPosition =
                new Vector3(player.transform.position.z * 4.5f, -player.transform.position.x * 4.5f, playerMapPosition.transform.localPosition.z);
        }

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
        energy.rectTransform.sizeDelta = new Vector2(energyWidth, energy.rectTransform.sizeDelta.y);
    }

    Vector3 getLiveMoves(int count)
    {
        return Vector3.right * 80 * count;
    }
}
