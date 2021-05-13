using UnityEngine;
using System.Collections;
using FayvitCam;

public static class SetHeroCamera
{
    public static void Set(Transform transform)
    {
        CameraAplicator.cam.FocusForDirectionalCam(transform, .1f, 3);
        CameraAplicator.cam.Cdir.VarVerticalHeightPoint = .7f;
    }
}
