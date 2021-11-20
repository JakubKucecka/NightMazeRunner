using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Detector : MonoBehaviour
{
    public GameObject player;

    private void Start()
    {

    }

    void OnTriggerEnter(Collider other)
    {
        if (other.name == "Player")
        {
            player.GetComponent<Player>().gameItems["detector"] = true;
        }
    }
}
