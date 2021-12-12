using System;
using UnityEngine;

/// <summary>
/// script pre kontrolu zmeny kamier
/// </summary>
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

    [SerializeField]
    GameObject globalInfoCanvas;
    [SerializeField]
    GameObject playerInfoCanvas;

    public int nightVissionLevel;

    private void Start()
    {
        Invoke("reloadCameras", 2.0f);
    }

    /// <summary>
    /// v update sa iba kontroluje ci je zapnute nocne videnie alebo nie a podla toho zobrazuje potrebne canvas objekty
    /// </summary>
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

    /// <summary>
    /// pri prepinani na nocne videnie sa kontroluje ci hrac vlastni dany objekt
    /// nastavi ubytok energie a vypne ostatne objekty ako baterku a detektor
    /// </summary>
    public void changeUseNightVission()
    {
        if (player.useNightVission)
        {
            player.turnOffAllItemsWithout(0);
        }
        else
        {
            if (player.gameItems["nightVission"])
            {
                // ratanie ubytku energie funguje na odratany mocniny 0.4 podla levelu od maximalneho ubytku pre dany objekt
                player.energyDecrease = (float)(player.energyMaxDecreaseGloves * Math.Pow(0.4, nightVissionLevel - 1));
                nightVisionGloves.SetActive(true);
                if (player.energy > 0) night.SetActive(false);
                player.turnOffAllItemsWithout(2);
                player.useItems = true;
                player.useNightVission = true;
            }
        }
    }

    /// <summary>
    /// pri zmene kamery sa nastavia premenne firstPerson v controlleroch na otacanie a pohyb Player-a
    /// Tiez sa nastavia rotacie jednotlivych objektov
    /// </summary>
    public void changeCamera()
    {
        if (firstPersonCamera.enabled)
        {
            // show main camera
            globalInfoCanvas.SetActive(true);
            playerInfoCanvas.SetActive(false);

            firstPersonCamera.enabled = false;
            overheadCamera.enabled = true;

            moveControler.firstPerson = false;
            rotateControler.firstPerson = false;

            moveControler.transform.rotation = moveControler.startRotation;
        }
        else
        {
            // show first person camera
            globalInfoCanvas.SetActive(false);
            playerInfoCanvas.SetActive(true);

            firstPersonCamera.enabled = true;
            overheadCamera.enabled = false;

            moveControler.firstPerson = true;
            rotateControler.firstPerson = true;

            rotateControler.transform.rotation = rotateControler.startRotation;
        }
    }

    /// <summary>
    /// pri restarte camier sa opat nastavy tma a obnovia sa controllery na otacanie a pohyb
    /// </summary>
    public void reloadCameras()
    {
        player.useNightVission = false;
        moveControler = player.moveControler;
        rotateControler = player.rotateControler;
        if (firstPersonCamera.enabled)
        {
            globalInfoCanvas.SetActive(false);
            playerInfoCanvas.SetActive(true);
        }
        else
        {
            globalInfoCanvas.SetActive(true);
            playerInfoCanvas.SetActive(false);
        }
    }
}
