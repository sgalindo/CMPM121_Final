using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HammerCrush : MonoBehaviour
{
    private Hammer hammer; // Reference to parent

    // Start is called before the first frame update
    void Start()
    {
        hammer = GetComponentInParent<Hammer>();
    }

    private void OnTriggerEnter(Collider other)
    {
        // If the hammer hasn't hit the ground yet, check for collisions with player.
        if (!hammer.GetHammerDown())
        {
            // If collider is a player, kill them
            PlayerMovement playerScript = other.gameObject.GetComponent<PlayerMovement>();
            if (playerScript != null)
            {
                playerScript.Kill();
            }
        }
    }
}
