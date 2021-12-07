using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// pridavame zvuk na prechod kurzorom ponad tlacidlo
/// Required when using Event data.
/// </summary>
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
