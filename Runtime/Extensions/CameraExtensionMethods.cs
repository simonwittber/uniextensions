using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public static class CameraExtensionMethods
{

    /// <summary>
    /// Smoothly change a camera's FOV from current to target FOV over T seconds.
    /// </summary>
    /// <param name="camera">
    /// A <see cref="Camera"/>
    /// </param>
    /// <param name="FOV">
    /// A <see cref="System.Single"/> The target FOV
    /// </param>
    /// <param name="T">
    /// A <see cref="System.Single"/> The time in seconds taken to reach the target.
    /// </param>
    public static void Zoom(this Camera camera, float FOV, float T)
    {
        var startFOV = camera.fieldOfView;
        UniExtender.Instance.StartCoroutine(UniExtender.Step(T, delegate (float P)
        {
            camera.fieldOfView = Mathf.Lerp(startFOV, FOV, P);
        }));
    }

}
