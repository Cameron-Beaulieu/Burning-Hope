using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void LoadGame()
    {
        GlobalState.Time = 0f;
        GlobalState.LoadLevel(1);
    }

    public void LoadTutorial()
    {
        GlobalState.Time = 0f;
        GlobalState.LoadScene(8);
    }

    public void NextLevel()
    {
        GlobalState.LoadLevel(GlobalState.Scene / 2 + 1);
    }
}
