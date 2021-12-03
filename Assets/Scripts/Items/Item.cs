using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    float rotationSpeed = 50;
    public AudioSource pickUp;

    // Update is called once per frame
    void Update()
    {
        if (GetComponent<NightVision>() != null || GetComponent<MiniMap>() != null)
        {
            transform.Rotate(0, rotationSpeed * Time.deltaTime, 0);
        }
        else
        {
            transform.Rotate(0, 0, rotationSpeed * Time.deltaTime);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.name == "Player")
        {
            pickUp.Play();
            Destroy(gameObject);
        }
    }
}
