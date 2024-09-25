using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FindCanvasCamera : MonoBehaviour
{
    Canvas mCanvas;
    void Start()
    {
        mCanvas = GetComponent<Canvas>();
        SceneManager.activeSceneChanged += (s1, s2) =>
        {
            FindCamera();
        };
    }

    private void FindCamera()
    {
        if (mCanvas != null && mCanvas.worldCamera == null)
        {
            mCanvas.worldCamera = Camera.main;
                        
        }
    }

    // Update is called once per frame
    void Update()
    {
        FindCamera();
    }
}