using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void LoadGame(int level)
    {
        GlobalState.Scene = 1 + level; //Start loading the requested level
        SceneManager.LoadScene("Loading Scene");
    }
}
