using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadingScene : MonoBehaviour
{
    private float textUpdateRate = 0.5f;
    private float textTimer;
    private string[] text = {
        "Loading",
        "Loading.",
        "Loading..",
        "Loading..."
    };

    private int currentText;

    // Start is called before the first frame update
    void Start()
    {
        gameObject.transform.GetChild(1).gameObject.GetComponent<TMPro.TextMeshProUGUI>().text = text[0];
        currentText = 0;
        textTimer = 0f;
        StartCoroutine(LoadAsync());
    }

    // Update is called once per frame
    void Update()
    {
        textTimer += Time.deltaTime;
        if (textTimer > textUpdateRate)
        {
            textTimer = 0f;
            currentText = currentText + 1;

            if (currentText == text.Length)
            {
                currentText = 0;
            }

            gameObject.transform.GetChild(1).gameObject.GetComponent<TMPro.TextMeshProUGUI>().text = text[currentText];
        }
    }

    IEnumerator LoadAsync()
    {
        AsyncOperation loadScene = SceneManager.LoadSceneAsync(GlobalState.Scene);
        yield return new WaitForEndOfFrame();
    }
}
