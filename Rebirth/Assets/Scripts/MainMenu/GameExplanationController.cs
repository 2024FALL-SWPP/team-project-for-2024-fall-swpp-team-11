using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameExplanationController : MonoBehaviour
{
    public GameObject canvas;

    void Start()
    {
        if (canvas != null)
        {
            canvas.SetActive(false);
        }
    }

    // void Update()
    // {
    //     if (Input.GetKeyDown(KeyCode.P))
    //     {
    //         ToggleCanvas();
    //     }
    // }

    public void ToggleCanvas()
    {
        if (canvas != null)
        {
            canvas.SetActive(!canvas.activeSelf);
        }
    }
}
