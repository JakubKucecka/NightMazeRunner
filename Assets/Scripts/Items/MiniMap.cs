using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniMap : MonoBehaviour
{
    public Player player;

    void OnTriggerEnter(Collider other)
    {
        if (other.name == "Player")
        {
            player.useMiniMap = true;
        }
    }
}
