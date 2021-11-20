using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    public GameObject player;

    private void Start()
    {
        
    }

    void OnTriggerEnter()
    {
        player.GetComponent<Player>().coins += 1;
    }
}
