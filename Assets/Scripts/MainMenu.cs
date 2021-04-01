using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void LoadGame()
    {
        GlobalState.Scene = 1;
        SceneManager.LoadScene("Loading Scene");
    }
}
