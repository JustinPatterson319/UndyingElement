using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;

public class EssentialObjects : MonoBehaviour
{
    Scene currentScene;
    String sceneName;
    public bool gameOver = false;

    public void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }
    public void Update()
    {
        currentScene = SceneManager.GetActiveScene();
        sceneName = currentScene.name;
        if (sceneName == "Title Screen")
        {
            while (transform.childCount > 0)
            {
                DestroyImmediate(transform.GetChild(0).gameObject);
            }
        }
    }
}
