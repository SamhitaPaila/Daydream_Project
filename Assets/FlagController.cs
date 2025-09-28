using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlagController : MonoBehaviour
{
    public Vector3 playerResetPosition = Vector3.zero; // Set this in Inspector or default to (0,0,0)

    private void OnTriggerEnter2D(Collider2D other)
    {   
        if (other.CompareTag("Player"))
        {
            // Reset player position
            other.transform.position = playerResetPosition;
            // Optionally reset velocity if Rigidbody exists
            Rigidbody rb = other.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.velocity = Vector3.zero;
                rb.angularVelocity = Vector3.zero;
            }
            // Add one to each item
            UIController ui = FindObjectOfType<UIController>();
            if (ui != null)
            {
                ui.AddOneToEachItem();
            }
        }
    }
}
