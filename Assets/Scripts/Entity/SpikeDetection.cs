using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeDetection : MonoBehaviour
{
    private Adventurer adventurerScript;

    // Start is called before the first frame update
    void Start()
    {
        adventurerScript = gameObject.transform.parent.GetComponent<Adventurer>();
    }

    /* 
     * Handles collisions w/ spikes. Triggered when a 2D collider
     * hits another 2D collider.
     */
    public void OnTriggerEnter2D(Collider2D collider)
    {
        Debug.Log(collider.GetType() + " " + collider.gameObject.layer);
        // Check for obstacles
        if (collider.gameObject.layer == LayerMask.NameToLayer("Spikes"))
        {
            adventurerScript.Die();
        }
    }
}
