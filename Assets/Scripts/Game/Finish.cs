using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Finish : MonoBehaviour
{
    [SerializeField]
    GameObject gameGO;
    Game game;

    private void Start()
    {
        game = gameGO.GetComponent<Game>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.name == "Player")
        {
            // TODO: save JSON
            game.unlockNext();
            game.SaveGameDataToJSON();
            if (game.level == game.maxLevel)
            {
                game.player.finish = true;
            }
            else
            {
                game.showMenu = true;
            }
        }
    }
}
