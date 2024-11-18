using UnityEngine;
using System.Collections;

public class PortalTransition2D : PortalTransition
{
    public override void OnFocus()
    {
        Debug.Log("2D focus");
    }

    public override void OnDefocus()
    {
        Debug.Log("2D defocus");
    }
}
