using UnityEngine;
using System.Collections;

public class PortalTransition3D : PortalTransition
{
    public override void OnFocus()
    {
        Debug.Log("3D focus");
    }

    public override void OnDefocus()
    {
        Debug.Log("3D defocus");
    }
}
