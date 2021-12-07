using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// script zastresuje kontrolu a zakladne atributy bezca
/// </summary>
public class Player : MonoBehaviour
{
    public bool gameIsStarted;

    public Dictionary<string, bool> gameItems;
    public int coins;
    public int lives;
    public float energy;
    public float energyDecrease;
    public int energyMaxDecreaseLight = 4;
    public int energyMaxDecreaseGloves = 6;
    public int energyMaxDecreaseDetector = 2;
    public int lightLevel;
    public int detectorLevel;
    public float saveDistance;

    public bool useNightVission;
    public bool useLight;
    public bool useDetector;

    public bool useItems;
    public bool gameover = false;
    public bool finish = false;
    public bool started;
    public bool useMiniMap;

    public PlayerMove moveControler;
    public PlayerRotation rotateControler;
    public GameObject detector;
    public GameObject nightVisionGloves;

    [SerializeField]
    List<Canvas> detectorCanvas = new List<Canvas>();
    [SerializeField]
    GameObject night;
    [SerializeField]
    GameObject ghost;
    [SerializeField]
    Material redMat;
    [SerializeField]
    Material greenMat;

    private Light[] lights;
    private float blinkTime;
    private float blinkTimeChange;
    private bool isRed;

    private bool alarmPlay;
    private AudioSource alarmSound;

    /// <summary>
    /// pri starte sa nastavia potrebne atributy
    /// </summary>
    void Start()
    {
        alarmPlay = false;
        detector.SetActive(true);
        alarmSound = detector.GetComponent<AudioSource>();
        detector.SetActive(false);

        started = true;
        saveDistance = 30;
        blinkTime = 0;
        blinkTimeChange = 0.3f;
        isRed = false;
        useItems = false;
        energyDecrease = energyMaxDecreaseLight;
        lightLevel = 1;
        detectorLevel = 1;
        useMiniMap = false;

        lights = GetComponentsInChildren<Light>();
    }

    /// <summary>
    /// v update funkcii kontrolujeme zapinanei a vypinanie detektora a svetla
    /// </summary>
    void Update()
    {
        if (gameIsStarted)
        {
            if (energy > 0)
            {
                // prepocet energie
                if (useItems)
                {
                    energy -= energyDecrease * Time.deltaTime;
                }
            }
            else
            {
                // vypnutie vsetkych objektov a reset energie
                turnOffAllItemsWithout(0);
                energy = 0;
            }

            // zapinaie a vypinanie svetiel
            if (useLight)
            {
                turnOnLight();
            }
            else
            {
                turnOffLight();
            }

            // zapinaie a vypinanie detektora
            if (useDetector && getDistanceOfGhosh() < saveDistance && blinkTime < Time.time)
            {
                changeDetectorColor();
                blinkTime = Time.time + blinkTimeChange;
                if (getDistanceOfGhosh() < saveDistance / 2)
                {
                    blinkTimeChange = 0.1f;
                }
                else
                {
                    blinkTimeChange = 0.3f;
                }
            }
            else if (detector != null && blinkTime < Time.time)
            {
                detector.GetComponent<MeshRenderer>().material = greenMat;
                foreach (var c in detectorCanvas)
                {
                    c.gameObject.SetActive(false);
                }
            }
        }
    }

    /// <summary>
    /// pri restarte hraca sa vypnu svetla a restartne sa pozicia a otocenie
    /// </summary>
    public void RestartPlayer()
    {
        turnOffAllItemsWithout(0);
        moveControler.transform.position = moveControler.startPositon;
        moveControler.transform.rotation = moveControler.startRotation;
        rotateControler.transform.rotation = rotateControler.startRotation;
        useMiniMap = false;

        foreach (var l in lights)
        {
            l.enabled = false;
        }
    }

    /// <summary>
    /// opat zapnutie alebo vypnutie svetla
    /// </summary>
    public void changeUseLight()
    {
        if (useLight)
        {
            turnOffAllItemsWithout(0);
            turnOffLight();
        }
        else
        {
            turnOffAllItemsWithout(1);
            turnOnLight();
        }
    }

    /// <summary>
    /// prepocet ubytku energie pri zapnuti svetla a aj samotne zapnutie
    /// </summary>
    public void turnOnLight()
    {
        // ratanie ubytku energie funguje na odratany mocniny 0.4 podla levelu od maximalneho ubytku pre dany objekt
        energyDecrease = (float)(energyMaxDecreaseLight * Math.Pow(0.4, lightLevel - 1));
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

    /// <summary>
    /// vypnutie svetla
    /// </summary>
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
    // 2 - nightVission
    // 3 - detector
    public void turnOffAllItemsWithout(int without)
    {
        if (without != 1) useLight = false;
        if (without != 2)
        {
            nightVisionGloves.SetActive(false);
            night.SetActive(true);
            useNightVission = false;
        }
        if (without != 3)
        {
            if (detector != null) detector.SetActive(false);
            useDetector = false;
        }
        if (without == 0) useItems = false;
    }

    float getDistanceOfGhosh()
    {
        float dist = Vector3.Distance(transform.position, ghost.transform.position);
        return dist;
    }

    /// <summary>
    /// zapnutie alebo vypnutie detektora
    /// </summary>
    public void changeUseDetector()
    {
        if (useDetector)
        {
            if (detector != null) detector.SetActive(false);
            turnOffAllItemsWithout(0);
        }
        else
        {
            if (gameItems["detector"])
            {
                // ratanie ubytku energie funguje na odratany mocniny 0.4 podla levelu od maximalneho ubytku pre dany objekt
                energyDecrease = (float)(energyMaxDecreaseDetector * Math.Pow(0.4, detectorLevel - 1));
                detector.SetActive(true);
                turnOffAllItemsWithout(3);
                useDetector = true;
                useItems = true;
            }
        }
    }

    public void changeDetectorColor()
    {
        if (isRed)
        {
            isRed = false;
            detector.GetComponent<MeshRenderer>().material = greenMat;
            if (alarmPlay)
            {
                alarmSound.Stop();
                alarmPlay = false;
            }
            foreach (var c in detectorCanvas)
            {
                c.gameObject.SetActive(false);
            }
        }
        else
        {
            isRed = true;
            detector.GetComponent<MeshRenderer>().material = redMat;
            if (!alarmPlay)
            {
                alarmSound.Play();
                alarmPlay = true;
            }
            foreach (var c in detectorCanvas)
            {
                c.gameObject.SetActive(true);
            }
        }
    }
}
