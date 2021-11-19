using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraHandler : MonoBehaviour
{
    public GameObject playerGO;
    public Camera firstPersonCamera;
    public Camera overheadCamera;

    public GameObject night;
    public GameObject globalNightVission;
    public GameObject playerNightVission;
    public GameObject spotLight;

    Player player;
    PlayerMove moveControler;
    PlayerRotation rotateControler;

    private void Start()
    {
        Invoke("reloadCameras", 2.0f);
    }

    private void FixedUpdate()
    {
        if (player == null ) player = playerGO.GetComponent<Player>();

        if (player.energy <= 0)
        {
            player.useNightVission = false;
        }

        if (player.useNightVission)
        {
            night.SetActive(false);
            player.turnOffAllItemsWithout(2);

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
            night.SetActive(true);
            globalNightVission.SetActive(false);
            playerNightVission.SetActive(false);
        }
    }

    public void changeUseGlasses()
    {
        if (player.useNightVission)
        {
            player.turnOffAllItemsWithout(0);
        }
        else
        {
            player.turnOffAllItemsWithout(2);
            player.useItems = true;
            player.useNightVission = true;
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

            //var newRoattaion = moveControler.transform.rotation;
            moveControler.transform.rotation = moveControler.startRotation;
            //rotateControler.transform.rotation = newRoattaion;
            spotLight.transform.Rotate(0, -270, 0);
        }
        else
        {
            // show first person camera
            firstPersonCamera.enabled = true;
            overheadCamera.enabled = false;

            moveControler.firstPerson = true;
            rotateControler.firstPerson = true;

            rotateControler.transform.Rotate(0, 90, 0);
            var newRoattaion = rotateControler.transform.rotation;
            rotateControler.transform.rotation = moveControler.startRotation;
            moveControler.transform.rotation = newRoattaion;
            player.GetComponentInChildren<Light>().transform.Rotate(0, 270, 0);
        }
    }

    public void reloadCameras()
    {
        player.useNightVission = false;
        moveControler = player.moveControler;
        rotateControler = player.rotateControler;
    }
}
