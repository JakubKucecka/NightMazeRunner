using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// script prepina jednotlive canvasy menu
/// </summary>
public class MenuHandler : MonoBehaviour
{
    public Canvas gameoverCanvas;
    public Canvas congratsCanvas;

    [SerializeField]
    Game game;
    [SerializeField]
    List<Canvas> menuCanvases;

    private AudioSource buttonHover;

    /// <summary>
    /// pri starte sa nastavi hover sound pre kazde tlacidlo a zobrazy sa domov
    /// </summary>
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

    /// <summary>
    /// zobrazy home canvas
    /// </summary>
    public void ShowHome()
    {
        HideAll();
        menuCanvases[0].gameObject.SetActive(true);
    }

    /// <summary>
    /// zobrazi konkretny canvas
    /// </summary>
    /// <param name="canvas"></param>
    public void ShowCanvas(Canvas canvas)
    {
        HideAll();
        canvas.gameObject.SetActive(true);
    }

    /// <summary>
    /// skyje vsetky canvasy
    /// </summary>
    public void HideAll()
    {
        foreach (Canvas c in menuCanvases)
        {
            c.gameObject.SetActive(false);
        }
        gameoverCanvas.gameObject.SetActive(false);
        congratsCanvas.gameObject.SetActive(false);
    }

    /// <summary>
    /// pre nastavenie hover efektu potrebujeme zobrazit vsetky canvasy
    /// </summary>
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
