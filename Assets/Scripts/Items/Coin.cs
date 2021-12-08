using UnityEngine;

/// <summary>
/// script zastresuje kotrolu nad objektom penazy
/// </summary>
public class Coin : MonoBehaviour
{
    public Player player;

    /// <summary>
    /// pri kolizii s hracom pridame peniaze
    /// </summary>
    /// <param name="other"></param>
    void OnTriggerEnter(Collider other)
    {
        if (other.name == "Person")
        {
            player.coins += 2;
        }
    }
}
