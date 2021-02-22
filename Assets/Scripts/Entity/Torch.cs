using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Torch : MonoBehaviour
{
    public static float torchMaxLifetime = 5f; //duration of light in sec
    private float torchTimeAlive = 0f;
    private float intensity;
    private UnityEngine.Experimental.Rendering.Universal.Light2D light2D;
    // Start is called before the first frame update
    void Start()
    {
        light2D = this.GetComponent<UnityEngine.Experimental.Rendering.Universal.Light2D>();
        intensity = light2D.intensity;
    }

    // Update is called once per frame
    void Update()
    {
        //Update the time the torch has been alive, and decrease it's intensity
        torchTimeAlive += Time.deltaTime;
        light2D.intensity = intensity * (1f - (torchTimeAlive / torchMaxLifetime));

        //Check if the torch needs to disappear
        if (torchTimeAlive > torchMaxLifetime)
        {
            Destroy(this.gameObject);
        }
    }
}
