using UnityEngine;

/// <summary>
/// script zastresuje kotrolu nad objektom itemu
/// </summary>
public class Item : MonoBehaviour
{
    public Player player;
    public AudioSource pickUp;

    private float rotationSpeed = 50;

    /// <summary>
    /// nastavujeme jeho rotaciu v arene
    /// </summary>
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

    /// <summary>
    /// pri kolizii sa harcom item zmizne a pustime k tomu prisluchajuci zvuk
    /// </summary>
    /// <param name="other"></param>
    void OnTriggerEnter(Collider other)
    {
        if (player.gameIsStarted && other.name == "Person")
        {
            pickUp.Play();
            Destroy(gameObject);
        }
    }
}
