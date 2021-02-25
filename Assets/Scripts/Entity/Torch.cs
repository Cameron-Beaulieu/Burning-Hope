using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Torch : MonoBehaviour
{
    public float torchMaxLifetime = 5f; //duration of light in sec
    private float torchTimeAlive = 0f;
    private float intensity;
    public GameObject memoryPrefab;
    [HideInInspector]
    public float memoryLength;
    private Vector2 prevPosition;
    private UnityEngine.Experimental.Rendering.Universal.Light2D light2D;
    // Start is called before the first frame update
    void Start()
    {
        light2D = this.GetComponent<UnityEngine.Experimental.Rendering.Universal.Light2D>();
        intensity = light2D.intensity;
        prevPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        //Update the time the torch has been alive, and decrease it's intensity
        torchTimeAlive += Time.deltaTime;
        light2D.intensity = intensity * (1f - (torchTimeAlive / torchMaxLifetime));

        if (memoryPrefab)
        {
            if (Vector2.Distance(prevPosition, transform.position) > 1)
            {
                prevPosition = transform.position;
                GameObject memory = Instantiate(memoryPrefab, transform.position, Quaternion.identity);
                memory.GetComponent<Torch>().torchMaxLifetime = memoryLength;
            }
        }

        //Check if the torch needs to disappear
        if (torchTimeAlive > torchMaxLifetime)
        {
            Destroy(this.gameObject);
        }
    }
}
