using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NightVision : MonoBehaviour
{
    public GameObject player;

    private void Start()
    {

    }

    void OnTriggerEnter(Collider other)
    {
        if (other.name == "Player")
        {
            player.GetComponent<Player>().gameItems["nightVission"] = true;
        }
    }
}
