using UnityEngine;

/// <summary>
/// script zastresuje kotrolu nad objektom mini mapy
/// </summary>
public class MiniMap : MonoBehaviour
{
    public Player player;

    /// <summary>
    ///  pri kolizii s hracom sa zobrazi minimapa v pravom dolnom rohy info canvasu
    /// </summary>
    /// <param name="other"></param>
    void OnTriggerEnter(Collider other)
    {
        if (other.name == "Person")
        {
            player.useMiniMap = true;
        }
    }
}
