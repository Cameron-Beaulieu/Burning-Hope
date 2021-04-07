using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class GlobalState
{
    public static float Time { get; set; }
    public static float CaveTime { get; set; }
    public static float MineTime { get; set; }
    public static float CultTime { get; set; }
    public static int Scene { get; set; }
    public static void LoadScene(int scene)
    {
        GlobalState.Scene = scene; //Start loading the requested level
        SceneManager.LoadScene("Loading Scene");
    }
    public static void LoadLevel(int level)
    {
        LoadScene(2 * level); //Level 1 is scene 2, level 2 is scene 4, etc.
    }
}
