using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelsHandler : MonoBehaviour
{
    [SerializeField]
    GameObject gameGO;

    public void showLevels()
    {
        Game game = gameGO.GetComponent<Game>();
        var levelsButtons = GetComponentsInChildren<Button>();

        for (var i = 0; i < levelsButtons.Length; i++)
        {
            levelsButtons[i].interactable = game.unlockedLevels.ContainsKey(i + 1) && game.unlockedLevels[i + 1] ? true : false;
        }
    }
}
