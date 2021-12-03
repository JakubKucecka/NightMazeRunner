using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;// Required when using Event data.

public class OnPointerEnterButton : MonoBehaviour, IPointerEnterHandler// required interface when using the OnPointerEnter method.
{
    public AudioSource hover;
    public Game game;
    //Do this when the cursor enters the rect area of this selectable UI object.
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (game.showMenu)
        {
            hover.Play();
        }
    }
}
