using UnityEngine;

/// <summary>
/// script zastresuje kotrolu nad objektom beterie
/// </summary>
public class Battery : MonoBehaviour
{
    public Player player;

    /// <summary>
    /// sa koliziu s hracom pridame energiu 
    /// </summary>
    /// <param name="other"></param>
    void OnTriggerEnter(Collider other)
    {
        if (other.name == "Person")
        {
            player.AddEnergy();
        }
    }
}
