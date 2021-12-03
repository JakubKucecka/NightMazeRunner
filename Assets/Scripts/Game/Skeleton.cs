using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skeleton : MonoBehaviour
{
    public Ghost ghost;
    AudioSource boneSound;

    private void Start()
    {
        boneSound = GetComponent<AudioSource>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.name == "Player")
        {
            boneSound.Play();
            ghost.bonePosition = transform.position;
            ghost.goForBone = true;
        }
    }
}
