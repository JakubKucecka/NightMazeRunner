using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuHandler : MonoBehaviour
{
    [SerializeField]
    GameObject gameGO;
    Game game;

    public Camera menuCamera;
    public Camera mainCamera;
    public Camera firstPersonCamera;

    public Canvas levelsCanvas;
    // Start is called before the first frame update
    void Start()
    {
        game = gameGO.GetComponent<Game>();
        Invoke("showLevelsCanvas", 2);
    }

    // Update is called once per frame
    void Update()
    {
        if (!game.showMenu)
        {
            menuCamera.enabled = false;
        } else if (!menuCamera.enabled)
        {
            showLevelsCanvas();
            menuCamera.enabled = true;
        }
    }

    public void startLevel(int level)
    {
        menuCamera.enabled = false;
        game.level = level;
        game.loadLevel();
        game.showMenu = false;
        levelsCanvas.enabled = false;
    }

    void showLevelsCanvas()
    {
        levelsCanvas.gameObject.SetActive(true);
        levelsCanvas.enabled = true;
        levelsCanvas.GetComponent<LevelsHandler>().showLevels();
    }
}
