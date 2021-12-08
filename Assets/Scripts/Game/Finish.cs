using UnityEngine;

/// <summary>
/// objekt predstavuje script pre koncovu branu, ktora obsahuje collider nastaveny na Triggered = true
/// </summary>
public class Finish : MonoBehaviour
{
    [SerializeField]
    Game game;

    /// <summary>
    /// ak nastane kolizia s inym colliderom, skontroluje sa ci je to player
    /// ak ano odomkne sa novy level, udaje sa ulozia  a skontrolujeme ci sme v poslednom levely
    /// ak ano zobrazi sa congratulation screen inak menu
    /// </summary>
    /// <param name="other"></param>
    void OnTriggerEnter(Collider other)
    {
        if (other.name == "Person")
        {
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
