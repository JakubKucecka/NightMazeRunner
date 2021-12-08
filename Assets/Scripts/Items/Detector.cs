using UnityEngine;

/// <summary>
/// script zastresuje kotrolu nad objektom detektora
/// </summary>
public class Detector : MonoBehaviour
{
    public Player player;

    /// <summary>
    /// pri kolizii s hracom aktivujeme objekt detektora
    /// </summary>
    /// <param name="other"></param>
    void OnTriggerEnter(Collider other)
    {
        if (other.name == "Person")
        {
            player.gameItems["detector"] = true;
        }
    }
}
