using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraHandler : MonoBehaviour
{
    public GameObject playerGO;
    public Camera firstPersonCamera;
    public Camera overheadCamera;

    public GameObject night;
    public GameObject nightVisionGloves;
    public GameObject globalNightVission;
    public GameObject playerNightVission;

    public int glovesLevel;

    Player player;
    PlayerMove moveControler;
    PlayerRotation rotateControler;

    private void Start()
    {
        Invoke("reloadCameras", 2.0f);
    }

    private void FixedUpdate()
    {
        if (player == null) player = playerGO.GetComponent<Player>();

        if (player.useNightVission)
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

    public void changeUseGlasses()
    {
        if (player.useNightVission)
        {
            player.turnOffAllItemsWithout(0);
        }
        else
        {
            // TODO: set player.energyDecrease from JSON
            if (player.gameItems["glasses"])
            {
                player.energyDecrease = (float)(player.energyDecreaseMax - (0.5 * glovesLevel));
                nightVisionGloves.SetActive(true);
                night.SetActive(false);
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

            rotateControler.transform.Rotate(0, -90, 0);
            moveControler.transform.rotation = moveControler.startRotation;
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
        }
    }

    public void reloadCameras()
    {
        player.useNightVission = false;
        moveControler = player.moveControler;
        rotateControler = player.rotateControler;
    }
}
