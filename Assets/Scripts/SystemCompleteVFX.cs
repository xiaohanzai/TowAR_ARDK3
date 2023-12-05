using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SystemCompleteVFX : MonoBehaviour
{
    // Reference to the Particle System component
    public ParticleSystem particleSystem;

    private Storage storage;

    private void Start()
    {
        // Attempt to find the Storage component in the scene
        storage = FindObjectOfType<Storage>();

        // Check if the Storage component was found
        if (storage != null)
        {
            // Subscribe to the event when the success flag is triggered
            storage.OnItemStorageCountChanged += HandleSuccessFlag;
        }
        else
        {
            Debug.Log("Storage component not found in the scene.");
        }
    }

    private void OnDestroy()
    {
        // Unsubscribe from the event to prevent memory leaks
        if (storage != null)
        {
            storage.OnItemStorageCountChanged -= HandleSuccessFlag;
        }
    }

    private void HandleSuccessFlag(object sender, EventArgs e)
    {
        // Check if the success flag has been triggered
        if (storage.HasTriggeredSuccessFlag())
        {
            // Run particle system
            if (particleSystem != null)
            {
                particleSystem.Play();
            }
        }
    }
}
