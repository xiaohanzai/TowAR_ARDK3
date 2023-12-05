using UnityEngine;
using System.Collections;

public class FadeInMeshRenderer : MonoBehaviour
{
    public float fadeInTime = 2f; // Adjust the fade-in time as needed
    private MeshRenderer meshRenderer;
    private float currentAlpha = 0f;

    void Start()
    {
        meshRenderer = GetComponent<MeshRenderer>();

        if (meshRenderer != null)
        {
            // Set the initial alpha to 0
            Color initialColor = meshRenderer.material.color;
            initialColor.a = 0f;
            meshRenderer.material.color = initialColor;

            // Start the fade-in process
            StartCoroutine(FadeIn());
        }
        else
        {
            Debug.LogError("MeshRenderer component not found on the GameObject.");
        }
    }

    IEnumerator FadeIn()
    {
        while (currentAlpha < 1f)
        {
            // Incrementally increase the alpha value over time
            currentAlpha += Time.deltaTime / fadeInTime;

            // Set the new alpha value
            Color newColor = meshRenderer.material.color;
            newColor.a = currentAlpha;
            meshRenderer.material.color = newColor;

            yield return null; // Wait for the next frame
        }
    }
}
