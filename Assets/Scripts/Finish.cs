using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Finish : MonoBehaviour
{
    [SerializeField]
    public GameObject playerGO;

    Player player;

    private void Start()
    {
        player = playerGO.GetComponent<Player>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.name == "Player")
        {
            player.RestartPlayer();
        }
    }
}
