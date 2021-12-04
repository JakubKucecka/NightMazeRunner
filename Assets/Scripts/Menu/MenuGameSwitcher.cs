using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuGameSwitcher : MonoBehaviour
{
    [SerializeField]
    MenuHandler menu;

    [SerializeField]
    Game game;
    [SerializeField]
    Camera menuCamera;
    [SerializeField]
    Camera mainCamera;
    [SerializeField]
    Camera bodyCamera;

    private AudioSource backgroundSound;
    private bool soundIsPlay;

    private bool newGameCounter = false;

    // Start is called before the first frame update
    void Start()
    {
        backgroundSound = GetComponent<AudioSource>();
        soundIsPlay = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!game.showMenu)
        {
            if (soundIsPlay)
            {
                soundIsPlay = false;
                backgroundSound.Stop();
            }
            mainCamera.enabled = true;
            if (menuCamera != null && menuCamera.enabled) menuCamera.enabled = false;
        }
        else if (!menuCamera.enabled)
        {
            menuCamera.enabled = true;
            if (bodyCamera != null && bodyCamera.enabled) bodyCamera.enabled = false;
            if (mainCamera != null && mainCamera.enabled) mainCamera.enabled = false;
            menu.ShowHome();
        }

        if (game.showMenu && !soundIsPlay)
        {
            soundIsPlay = true;
            backgroundSound.Play();
        }

        if (game.player.gameover)
        {
            game.player.gameover = false;
            game.player.gameIsStarted = false;
            if (bodyCamera != null && bodyCamera.enabled) bodyCamera.enabled = false;
            if (mainCamera != null && mainCamera.enabled) mainCamera.enabled = false;
            game.ResetGameData();
            game.showMenu = true;
            menu.ShowCanvas(menu.gameoverCanvas);
            menuCamera.enabled = true;
        }

        if (game.player.finish)
        {
            game.player.finish = false;
            game.player.gameIsStarted = false;
            if (bodyCamera != null && bodyCamera.enabled) bodyCamera.enabled = false;
            if (mainCamera != null && mainCamera.enabled) mainCamera.enabled = false;
            game.showMenu = true;
            menu.ShowCanvas(menu.congratsCanvas);
            menuCamera.enabled = true;
        }
    }

    void CallShowHome()
    {
        menu.ShowHome();
    }

    public void startLevel(int level)
    {
        if (level == 0)
        {
            int lastLevel = 0;
            foreach (var l in game.unlockedLevels)
            {
                if (l.Value && l.Key > lastLevel)
                {
                    lastLevel = l.Key;
                }
            }
            game.level = lastLevel;
        }
        else
        {
            game.level = level;
        }

        menuCamera.enabled = false;
        game.loadLevel();
        game.showMenu = false;
        newGameCounter = false;
    }

    public void StartNewGame()
    {
        if (newGameCounter)
        {
            newGameCounter = false;
            game.ResetGameData();
            startLevel(1);
        }
        newGameCounter = true;
    }

    public void ExitApplication()
    {
        Application.Quit();
    }
}
