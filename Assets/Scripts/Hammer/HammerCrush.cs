using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HammerCrush : MonoBehaviour
{
    [SerializeField] private float destructionFactor = 0.5f;
    private Hammer hammer; // Reference to parent
    private Collider col;

    // Start is called before the first frame update
    void Start()
    {
        hammer = GetComponentInParent<Hammer>();
        col = GetComponent<Collider>();
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
            else if (other.tag == "Tile")
            {
                if (Vector2.Distance(new Vector2(transform.position.x, transform.position.z), new Vector2(other.transform.position.x, other.transform.position.z)) < (col.bounds.extents.x + destructionFactor))
                    other.gameObject.GetComponent<Tile>().Kill();
            }
        }
    }
}
