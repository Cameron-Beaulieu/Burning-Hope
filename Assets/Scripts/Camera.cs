using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera : MonoBehaviour
{
    public Entity target;
    public float smoothing = 0.1f;
    public float lookAheadOffset = 0f;
    private Vector2 cameraVelocity;

    // Update is called once per frame
    void Update()
    {
        float offset = target.collisionController.collisions.facing == 1 ? lookAheadOffset : lookAheadOffset * -1;
        Vector2 pos = Vector2.Lerp(new Vector2(transform.position.x, transform.position.y), new Vector2(target.transform.position.x + offset, target.transform.position.y), smoothing); 
        transform.position = new Vector3(pos.x, pos.y, transform.position.z);
    }
}
