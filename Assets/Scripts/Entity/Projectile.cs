using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    private float maxLifetime = 30f; //duration of projectile in sec
    private float timeAlive;
    public float speed; //speed of the projectile
    public Vector2 direction; //unit vector of the direction the projectile is heading
    public bool destroyed;
    public bool despawned;

    // Start is called before the first frame update
    void Start()
    {
        timeAlive = 0f;
        speed = 2f;
        despawned = false;
        destroyed = false;
    }

    // Update is called once per frame
    void Update()
    {
        //Update the time the projectile has been alive
        timeAlive += Time.deltaTime;

        //Check if the projectile needs to despawn
        if (timeAlive > maxLifetime)
        {
            despawned = true;
            Destroy(this.gameObject);
        }

        //Move the projectile
        this.gameObject.transform.position += (Vector3)(direction * speed * Time.deltaTime);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Solid"))
        {
            Debug.Log("Projectile collision.");
            destroyed = true;
            //TODO implement destruction animation
            Destroy(this.gameObject);
        }
    }
}
