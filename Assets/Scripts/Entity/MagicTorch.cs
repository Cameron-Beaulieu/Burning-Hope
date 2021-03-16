using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MagicTorch : MonoBehaviour
{
    public float moveSpeed = 1f;
    private Vector3 lastPos;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // FixedUpdate is called at a consistent rate
    void FixedUpdate()
    {
        // Find the direction from the torch to the mouse
        Vector3 mouseDir = UnityEngine.Camera.main.ScreenToWorldPoint(new Vector3(Mouse.current.position.ReadValue().x, Mouse.current.position.ReadValue().y, 0)) - this.transform.position;
        Vector2 mouseDir2D = new Vector2(mouseDir.x, mouseDir.y);
        mouseDir2D = mouseDir2D.normalized;

        // Move the torch towards the mouse position
        this.GetComponent<Rigidbody2D>().velocity = (Vector3)(mouseDir2D * moveSpeed);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("Collision recorded.");
    }
}
