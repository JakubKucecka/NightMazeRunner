using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuGameSwitcher : MonoBehaviour
{
    [SerializeField]
    GameObject gameGO;
    Game game;

    public Camera menuCamera;
    public Camera mainCamera;
    public Camera bodyCamera;

    public MenuHandler menu;
    // Start is called before the first frame update
    void Start()
    {
        game = gameGO.GetComponent<Game>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!game.showMenu)
        {
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

        if (game.player.gameover)
        {
            menu.ShowCanvas(menu.menuCanvases[menu.menuCanvases.Count - 1]);
            game.showMenu = true;
            menuCamera.enabled = true;
            if (bodyCamera != null && bodyCamera.enabled) bodyCamera.enabled = false;
            if (mainCamera != null && mainCamera.enabled) mainCamera.enabled = false;
            game.player.gameover = !game.player.gameover;
        }
    }

    public void startLevel(int level)
    {
        if (level == 0)
        {
            int lastLevel = 0;
            foreach (var l in game.unlockedLevels)
            {
                if(l.Value && l.Key > lastLevel)
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
    }

    public void ExitApplication()
    {
        Application.Quit();
    }
}
