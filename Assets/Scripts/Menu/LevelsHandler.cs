using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// script kontroluje intrakciu tlacidiel levelov
/// </summary>
public class LevelsHandler : MonoBehaviour
{
    [SerializeField]
    Game game;

    /// <summary>
    /// ak je level odomknuty, vieme na dane tlacidlo kliknut
    /// </summary>
    public void showLevels()
    {
        var levelsButtons = GetComponentsInChildren<Button>();

        for (var i = 0; i < levelsButtons.Length-1; i++)
        {
            levelsButtons[i].interactable = game.unlockedLevels.ContainsKey(i + 1) && game.unlockedLevels[i + 1] ? true : false;
        }
    }
}
