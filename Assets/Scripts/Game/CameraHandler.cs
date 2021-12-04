using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraHandler : MonoBehaviour
{
    [SerializeField]
    Player player;
    private PlayerMove moveControler;
    private PlayerRotation rotateControler;

    [SerializeField]
    Camera firstPersonCamera;
    [SerializeField]
    Camera overheadCamera;

    [SerializeField]
    GameObject night;
    [SerializeField]
    GameObject nightVisionGloves;
    [SerializeField]
    GameObject globalNightVission;
    [SerializeField]
    GameObject playerNightVission;

    public int nightVissionLevel;

    private void Start()
    {
        Invoke("reloadCameras", 2.0f);
    }

    private void FixedUpdate()
    {
        if (player.useNightVission && player.energy > 0)
        {
            if (firstPersonCamera.enabled)
            {
                globalNightVission.SetActive(false);
                playerNightVission.SetActive(true);
            }
            else
            {
                globalNightVission.SetActive(true);
                playerNightVission.SetActive(false);
            }
        }
        else
        {
            globalNightVission.SetActive(false);
            playerNightVission.SetActive(false);
        }
    }

    public void changeUseNightVission()
    {
        if (player.useNightVission)
        {
            player.turnOffAllItemsWithout(0);
        }
        else
        {
            // TODO: set player.energyDecrease from JSON
            if (player.gameItems["nightVission"])
            {
                player.energyDecrease = (float)(player.energyMaxDecreaseGloves * Math.Pow(0.4, nightVissionLevel - 1));
                nightVisionGloves.SetActive(true);
                if (player.energy > 0) night.SetActive(false);
                player.turnOffAllItemsWithout(2);
                player.useItems = true;
                player.useNightVission = true;
            }
        }
    }

    public void changeCamera()
    {
        if (firstPersonCamera.enabled)
        {
            // show main camera
            firstPersonCamera.enabled = false;
            overheadCamera.enabled = true;

            moveControler.firstPerson = false;
            rotateControler.firstPerson = false;

            moveControler.transform.rotation = moveControler.startRotation;
        }
        else
        {
            // show first person camera
            firstPersonCamera.enabled = true;
            overheadCamera.enabled = false;

            moveControler.firstPerson = true;
            rotateControler.firstPerson = true;

            rotateControler.transform.rotation = rotateControler.startRotation;
        }
    }

    public void reloadCameras()
    {
        player.useNightVission = false;
        moveControler = player.moveControler;
        rotateControler = player.rotateControler;
    }
}
