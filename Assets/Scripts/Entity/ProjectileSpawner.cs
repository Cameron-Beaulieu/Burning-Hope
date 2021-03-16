using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileSpawner : MonoBehaviour
{
    public float spawnTime = 3f; //time between spawning
    private float timeLastSpawn;
    public GameObject projectile;
    public Vector2 spawnDir; //direction to spawn projectiles in
    public float spawnSpeed; //speed of projectiles spawning

    // Start is called before the first frame update
    void Start()
    {
        timeLastSpawn = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        //Update the spawning timer
        timeLastSpawn += Time.deltaTime;

        //Check if the projectile needs to spawn
        if (timeLastSpawn > spawnTime)
        {
            // Spawn it, give it the spawner location, and give it speed and direction
            GameObject newProj = Instantiate(projectile);
            newProj.transform.position = this.transform.position;
            Projectile projScript = newProj.GetComponent<Projectile>();
            projScript.direction = spawnDir;
            projScript.speed = spawnSpeed;

            // Reset the timer
            timeLastSpawn = 0f;
        }
    }
}
