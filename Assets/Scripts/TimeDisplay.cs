using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TimeDisplay : MonoBehaviour
{
    private TMPro.TextMeshProUGUI textController;
    // Start is called before the first frame update
    void Start()
    {
        textController = this.gameObject.GetComponent<TMPro.TextMeshProUGUI>();
        
        // Show the time of the current level or final display
        switch (SceneManager.GetActiveScene().buildIndex)
        {
            case 3:
                textController.text = "Level time: " + GlobalState.CaveTime.ToString("F2");
                break;
            case 5:
                textController.text = "Level time: " + GlobalState.MineTime.ToString("F2");
                break;
            case 7:
                if (textController.gameObject.name == "Cave Time")
                {
                    textController.text = "Level 1 time: " + GlobalState.CaveTime.ToString("F2");
                } else if (textController.gameObject.name == "Mine Time")
                {
                    textController.text = "Level 2 time: " + GlobalState.MineTime.ToString("F2");
                } else if (textController.gameObject.name == "Cult Time")
                {
                    textController.text = "Level 3 time: " + GlobalState.CultTime.ToString("F2");
                } else
                {
                    textController.text = "Total time: " + GlobalState.Time.ToString("F2");
                }
                break;
            default:
                textController.text = "Total time: " + GlobalState.Time.ToString("F2");
                break;
        }
    }
}
