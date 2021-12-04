using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSpawner : MonoBehaviour
{
    [SerializeField]
    Player player;

    private AudioSource pickUpSound;

    private void Start()
    {
        pickUpSound = GetComponent<AudioSource>();
    }

    // Start is called before the first frame update
    public void ReloadItems(Dictionary<GameObject, List<Vector3>> itemsPositions)
    {
        var items = GameObject.FindGameObjectsWithTag("Item");

        foreach (var i in items)
        {
            Destroy(i);
        }

        foreach (var item in itemsPositions)
        {
            foreach (var i in item.Value)
            {
                var newItem = Instantiate(item.Key, transform);
                if (newItem.GetComponent<Coin>() != null) newItem.GetComponent<Coin>().player = player;
                if (newItem.GetComponent<Battery>() != null) newItem.GetComponent<Battery>().player = player;
                if (newItem.GetComponent<NightVision>() != null) newItem.GetComponent<NightVision>().player = player;
                if (newItem.GetComponent<Detector>() != null) newItem.GetComponent<Detector>().player = player;
                if (newItem.GetComponent<MiniMap>() != null) newItem.GetComponent<MiniMap>().player = player;
                newItem.transform.position = i;
                newItem.GetComponent<Item>().pickUp = pickUpSound;
                newItem.GetComponent<Item>().player = player;
            }
        }
    }
}
