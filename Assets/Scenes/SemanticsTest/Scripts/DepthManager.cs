using System;
using System.Collections;
using Niantic.Lightship.AR.Utilities;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class DepthManager : MonoBehaviour
{
    public static DepthManager instance;

    [SerializeField] AROcclusionManager _occlusionManager = null;

    XRCpuImage? depthimage;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    private void Update()
    {
        UpdateDepthImage();
    }

    void UpdateDepthImage()
    {
        if (!_occlusionManager.subsystem.running)
        {
            return;
        }

        if (_occlusionManager.TryAcquireEnvironmentDepthCpuImage(out var image))
        {
            depthimage?.Dispose();
            depthimage = image;
        }
    }

    public Vector3 GetWorldPosition(float x, float y)
    {
        if (depthimage.HasValue)
        {
            // Sample eye depth
            var uv = new Vector2(x / Screen.width, y / Screen.height);
            Matrix4x4 displayMat = Matrix4x4.identity;
            var eyeDepth = depthimage.Value.Sample<float>(uv, displayMat);

            // Get world position
            var worldPosition =
                Camera.main.ScreenToWorldPoint(new Vector3(x, y, eyeDepth));
            return worldPosition;
        }
        return Vector3.zero;
    }
}