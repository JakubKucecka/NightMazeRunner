using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuHandler : MonoBehaviour
{
    public Canvas gameoverCanvas;
    public Canvas congratsCanvas;

    [SerializeField]
    Game game;
    [SerializeField]
    List<Canvas> menuCanvases;

    private AudioSource buttonHover;

    // Start is called before the first frame update
    void Start()
    {
        ShowAll();
        buttonHover = GetComponent<AudioSource>();

        foreach (var b in GetComponentsInChildren<Button>())
        {
            b.gameObject.AddComponent<OnPointerEnterButton>().hover = buttonHover;
            b.gameObject.GetComponent<OnPointerEnterButton>().game = game;
        }
        ShowHome();
    }

    public void ShowHome()
    {
        HideAll();
        menuCanvases[0].gameObject.SetActive(true);
    }

    public void ShowCanvas(Canvas canvas)
    {
        HideAll();
        canvas.gameObject.SetActive(true);
    }

    public void HideAll()
    {
        foreach (Canvas c in menuCanvases)
        {
            c.gameObject.SetActive(false);
        }
        gameoverCanvas.gameObject.SetActive(false);
        congratsCanvas.gameObject.SetActive(false);
    }

    public void ShowAll()
    {
        foreach (Canvas c in menuCanvases)
        {
            c.gameObject.SetActive(true);
        }
        gameoverCanvas.gameObject.SetActive(true);
        congratsCanvas.gameObject.SetActive(true);
    }
}
