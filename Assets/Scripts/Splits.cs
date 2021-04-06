using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Splits : MonoBehaviour
{
    private float time;
    private TMPro.TextMeshProUGUI textController;
    // Start is called before the first frame update
    void Start()
    {
        textController = gameObject.transform.GetChild(0).gameObject.GetComponent<TMPro.TextMeshProUGUI>();
        time = 0;
    }

    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime;
        textController.text = time.ToString("F2");
    }
}
