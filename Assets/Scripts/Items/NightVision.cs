using UnityEngine;

/// <summary>
/// script zastresuje kotrolu nad objektom okuliarov na nocne videnie
/// </summary>
public class NightVision : MonoBehaviour
{
    public Player player;

    /// <summary>
    /// pri kolizii s hracom aktivujeme objekt okuliarov na nocne videnie
    /// </summary>
    /// <param name="other"></param>
    void OnTriggerEnter(Collider other)
    {
        if (other.name == "Person")
        {
            player.gameItems["nightVission"] = true;
        }
    }
}
