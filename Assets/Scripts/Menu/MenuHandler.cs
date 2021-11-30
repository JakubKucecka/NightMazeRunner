using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuHandler : MonoBehaviour
{
    public List<Canvas> menuCanvases;
    // Start is called before the first frame update
    void Start()
    {
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
    }
}
