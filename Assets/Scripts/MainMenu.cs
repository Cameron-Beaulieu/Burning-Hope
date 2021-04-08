using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void LoadGame()
    {
        GlobalState.Time = 0f;
        GlobalState.LoadScene(10); //Load story intro
    }

    public void LoadLevel(int i)
    {
        GlobalState.LoadLevel(i);
    }

    public void NextLevel()
    {
        GlobalState.LoadLevel(GlobalState.Scene / 2 + 1);
    }

    public void LoadScene(int i)
    {
        GlobalState.LoadScene(i);
    }

    public void NextScene()
    {
        GlobalState.LoadScene(GlobalState.Scene + 1);
    }
}
