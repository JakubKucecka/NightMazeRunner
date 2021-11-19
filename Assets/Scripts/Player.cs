using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public int coins;
    public int lives;
    public float energy;
    public float energyDecrease;

    public bool useNightVission;
    public bool useLight;
    public bool useDetector;

    public bool useItems;

    public PlayerMove moveControler;
    public PlayerRotation rotateControler;
    public GameObject night;

    Light[] lights;

    // Start is called before the first frame update
    void Start()
    {
        useItems = false;
        energyDecrease = 2;

        moveControler = GetComponentInChildren<PlayerMove>();
        rotateControler = GetComponentInChildren<PlayerRotation>();

        lights = GetComponentsInChildren<Light>();

        RestartPlayer();
    }

    // Update is called once per frame
    void Update()
    {
        if (energy > 0)
        {
            if (useItems)
            {
                energy -= energyDecrease * Time.deltaTime;
            }
        } else
        {
            turnOffAllItemsWithout(0);
        }

        if (useLight) {
            turnOnLight();
        } else
        {
            turnOffLight();
        }
    }

    public void RestartPlayer()
    {
        moveControler.transform.position = moveControler.startPositon;
        moveControler.transform.rotation = moveControler.startRotation;
        rotateControler.transform.rotation = rotateControler.startRotation;
        useLight = true;

        foreach (var l in lights)
        {
            l.enabled = true;
        }
    }

    public void changeUseLight()
    {
        if (useLight)
        {
            turnOffAllItemsWithout(0);
            turnOffLight();
        } else
        {
            turnOffAllItemsWithout(1);
            turnOnLight();
        }
    }

    public void turnOnLight()
    {
        if (energy > 0)
        {
            useLight = true;
            useItems = true;
            foreach (var l in lights)
            {
                l.enabled = true;
            }
        }
    }
    public void turnOffLight()
    {
        foreach (var l in lights)
        {
            l.enabled = false;
        }
    }

    public void GetLive()
    {
        lives -= 1;
    }

    public void AddLive()
    {
        lives += 1;
    }

    public void AddEnergy()
    {
        energy += 20;
        if (energy > 100) energy = 100;
    }

    // 1 - light
    // 2 - glasses
    // 3 - detector
    public void turnOffAllItemsWithout(int without)
    {
        if (without != 1) useLight = false;
        if (without != 2)
        {
            night.SetActive(true);
            useNightVission = false;
        }
        if (without != 3) useDetector = false;
        useItems = false;
    }
}
