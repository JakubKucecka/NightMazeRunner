using UnityEngine;

/// <summary>
/// script zastresuje kontrolu nad kostrou
/// </summary>
public class Skeleton : MonoBehaviour
{
    public Game game;
    private AudioSource boneSound;

    /// <summary>
    /// nacita sa zvuk prechudu ponad kostru
    /// </summary>
    private void Start()
    {
        boneSound = GetComponent<AudioSource>();
    }

    /// <summary>
    /// ak nastane kolizia kostry s bezcom aktivujeme duchovy chodzu ku kostre
    /// </summary>
    /// <param name="other"></param>
    void OnTriggerEnter(Collider other)
    {
        if (!game.showMenu && other.name == "Player")
        {
            boneSound.Play();
            game.ghost.bonePosition = transform.position;
            game.ghost.goForBone = true;
            game.ghost.GetDir();
        }
    }
}
