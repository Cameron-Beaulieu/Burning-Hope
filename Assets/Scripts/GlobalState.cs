using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class GlobalState
{
    public static int Scene { get; set; }
    public static void LoadScene(int scene)
    {
        GlobalState.Scene = scene; //Start loading the requested level
        SceneManager.LoadScene("Loading Scene");
    }
    public static void LoadLevel(int level)
    {
        LoadScene(1 + level);
    }
}
