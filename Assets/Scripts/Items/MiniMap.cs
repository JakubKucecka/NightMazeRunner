using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniMap : MonoBehaviour
{
    public GameObject player;

    private void Start()
    {

    }

    void OnTriggerEnter(Collider other)
    {
        if (other.name == "Player")
        {
            // show map
        }
    }
}
