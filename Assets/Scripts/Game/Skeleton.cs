using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skeleton : MonoBehaviour
{
    public Game game;
    AudioSource boneSound;

    private void Start()
    {
        boneSound = GetComponent<AudioSource>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (!game.showMenu && other.name == "Player")
        {
            boneSound.Play();
            game.ghost.bonePosition = transform.position;
            game.ghost.goForBone = true;
        }
    }
}
