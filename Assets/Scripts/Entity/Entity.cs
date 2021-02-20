using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(EntityCollision))]
public abstract class Entity : MonoBehaviour
{
    public int maxHealth = 1;
    public int health = 1;
    public float maxJumpHeight = 2f;
    public float minJumpHeight = 1f;
    public float timeToJumpApex = .4f;
    [HideInInspector]
    public float minJumpVelocity;
    [HideInInspector]
    public float maxJumpVelocity;
    [HideInInspector]
    public float gravity;
    [HideInInspector]
    public float maxMoveSpeed = 20f;
    [HideInInspector]
    public float minMoveSpeed = 10f;
    public float groundFriction = .1f;
    public float airFriction = .2f;
    [HideInInspector]
    public float velocityXSmoothing = 0f;
    public float moveSpeed = 5;
    public Vector2 velocity;
    [HideInInspector]
    public EntityCollision collisionController;
    [HideInInspector]
    public Vector2 movementInput;
    // Start is called before the first frame update

    protected virtual void Start()
    {
        collisionController = GetComponent<EntityCollision>();
        gravity = -(2 * maxJumpHeight) / Mathf.Pow(timeToJumpApex, 2);
        maxJumpVelocity = Mathf.Abs(gravity) * timeToJumpApex;
        minJumpVelocity = Mathf.Sqrt(2 * Mathf.Abs(gravity) * minJumpHeight);
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        CalculateVelocity();
        collisionController.Move(velocity * Time.deltaTime);

        if (collisionController.collisions.top || collisionController.collisions.down) {
            velocity.y = 0f;
        }
    }

    public void CalculateVelocity() {
        float targetVelocityX = movementInput.x * moveSpeed;
        velocity.x = Mathf.SmoothDamp(velocity.x, targetVelocityX, ref velocityXSmoothing, (collisionController.collisions.down ? groundFriction : airFriction));
        velocity.y += gravity * Time.deltaTime;
    }

    public void SetMovementInput(Vector2 input)
    {
        movementInput = input;
    }

    public abstract void OnJumpInputDown();
    public abstract void OnJumpInputUp();
    public abstract void OnActionInputDown();

    public abstract void OnActionInputUp();
    public abstract void OnFallInputDown();
    public abstract void OnFallInputUp();
}
