using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Adventurer : Entity
{
    public float dashCooldown = 0.2f;
    public float dashSpeed = 15;
    public int maxJumps = 1;
    public int jumps = 1;
    private float dashTimer;
    public float wallSlideSpeed = 0.2f;
    public Vector2 wallJumpVelocity = new Vector2(8, 10);
    public bool wallSliding;
    private bool dashing;
    private bool fastFalling;
    public GameObject torchPrefab;
    public float memoryLength;
    private Vector2 lastCheckpoint;
    private Animator anim;
    private SpriteRenderer sprite;
    private AudioManager audio;
    // Start is called before the first frame update
    protected override void Start()
    {
        lastCheckpoint = this.transform.position; //set the spawn to the current location
        anim = GetComponent<Animator>();
        sprite = GetComponent<SpriteRenderer>();
        audio = GetComponent<AudioManager>();
        base.Start();
    }

    // Update is called once per frame
    protected override void Update()
    {
        // Check for obstacles
        if (collisionController.death)
        {
            collisionController.death = false; //reset the flag
            Die();
        }

        // Check for checkpoints
        if (collisionController.checkpoint != Vector2.zero)
        {
            lastCheckpoint = collisionController.checkpoint;
            collisionController.checkpoint = Vector2.zero;
        }

        // Go into wall sliding state when coming into contact with a wall, or jumping against a wall from the ground
        if (((!collisionController.collisions.down && collisionController.collisions.left && !collisionController.prevCollisions.left) || (!collisionController.collisions.down && collisionController.collisions.right && !collisionController.prevCollisions.right)) || ((!collisionController.collisions.down && collisionController.collisions.left && collisionController.prevCollisions.down) || (!collisionController.collisions.down && collisionController.collisions.right && collisionController.prevCollisions.down)))
        {
            if (LayerMask.LayerToName(collisionController.collisions.left.layer) == "Wall Jump" || LayerMask.LayerToName(collisionController.collisions.right.layer) == "Wall Jump")
            {
                wallSliding = true;
                jumps = maxJumps;
            }
        }

        // Set wallSliding to false on airbourne
        if (!collisionController.collisions.left && !collisionController.collisions.right || collisionController.collisions.down)
        {
            wallSliding = false;
        }

        // Restore jumps on landing
        if (collisionController.collisions.down && !collisionController.prevCollisions.down)
        {
            jumps = maxJumps;
            audio.Play("land");
        }

        // Decrease number of jumps for a walk-off
        if (jumps == maxJumps && !collisionController.collisions.down && collisionController.prevCollisions.down)
        {
            jumps--;
        }

        anim.SetBool("wallSliding", wallSliding);

        // Dashing cooldown countdown
        if (dashing)
        {
            dashTimer -= Time.deltaTime;
            if (dashTimer < 0)
            {
                dashing = false;
            }
        }
        
        // Calculate y velocity
        velocity.y += gravity * Time.deltaTime;

        // Set y velocity in special cases
        if (wallSliding)
        {
            if (fastFalling)
            {
                velocity.y = wallSlideSpeed * -2f;
            }
            else if (velocity.y < -1 * wallSlideSpeed)
            {
                velocity.y = -1 * wallSlideSpeed;
            }
        }
        else
        {
            if (fastFalling && !collisionController.collisions.down && velocity.y < 0)
            {
                velocity.y = maxJumpVelocity * -1.5f;
                anim.SetTrigger("fastFall");
            }
            else if (velocity.y < -1 * maxJumpVelocity)
            {
                velocity.y = -1 * maxJumpVelocity;
            }
        }

        anim.SetFloat("velocity.y", velocity.y);

        // Calculate x velocity
        float targetVelocityX = movementInput.x * moveSpeed;
        velocity.x = Mathf.SmoothDamp(velocity.x, targetVelocityX, ref velocityXSmoothing, (collisionController.collisions.down ? dashing ? groundFriction * 2 : groundFriction : dashing ? airFriction * 2 : airFriction));

        anim.SetFloat("velocity.x", Mathf.Abs(Mathf.Round(velocity.x)));

        // Move the entity
        collisionController.Move(velocity * Time.deltaTime);

        anim.SetBool("grounded", collisionController.collisions.down);

        sprite.flipX = collisionController.collisions.facing == 1 ? false : true;

        // Set velocity to 0 on landing or ceiling
        if (collisionController.collisions.top || collisionController.collisions.down) {
            fastFalling = false;
            anim.SetBool("fastFalling", fastFalling);
            velocity.y = 0f;
        }
    }
    public override void OnJumpInputDown()
    {
        if (wallSliding)
        {
            velocity.x = collisionController.collisions.facing * -1 * wallJumpVelocity.x;
            velocity.y = wallJumpVelocity.y;
            jumps--;
            anim.SetTrigger("jump");
            audio.Play("jump");
            fastFalling = false;
            anim.SetBool("fastFalling", fastFalling);
        }
        else if (jumps > 0)
        {
            jumps--;
            velocity.y = maxJumpVelocity;
            anim.SetTrigger("jump");
            audio.Play("jump");
            fastFalling = false;
            anim.SetBool("fastFalling", fastFalling);
        }
    }
    public override void OnJumpInputUp()
    {
        if (velocity.y > minJumpVelocity)
        {
            velocity.y = minJumpVelocity;
        }
    }

    //NOTE: Is called 3 times on a mouse click (I think once for initial press, once for hold, and once on release)
    public override void OnActionInputDown() 
    {
        Debug.Log("Dash action registered.");
        if (!dashing && dashTimer <= 0 && !wallSliding)
        {
            Debug.Log("Dashing.");
            dashing = true;
            if (movementInput.x != 0)
            {
                collisionController.collisions.facing = (int)Mathf.Sign(movementInput.x);
            }
            velocity.x = collisionController.collisions.facing * dashSpeed;
            dashTimer = dashCooldown;
            audio.Play("dash");
        }
    } 
    public override void OnActionInputUp() {}
    public override void OnFallInputDown()
    {
        fastFalling = true;
        anim.SetBool("fastFalling", fastFalling);
    }

    public override void OnFallInputUp()
    {
        if (wallSliding)
        {
            fastFalling = false;
            anim.SetBool("fastFalling", fastFalling);
        }
    }

    /*
     * DESC: "Throws" a torch. Creates a copy of the torchPrefab, and launches it the direction the player is facing.
     */
    public override void ThrowTorch()
    {
        if (torches > 0)
        {
            GameObject torch = Instantiate(torchPrefab, this.transform.position, Quaternion.identity);

            //Find the direction from the player to the mouse
            Vector3 mouseDir = UnityEngine.Camera.main.ScreenToWorldPoint(new Vector3(Mouse.current.position.ReadValue().x, Mouse.current.position.ReadValue().y, 0)) - this.transform.position;
            Vector2 mouseDir2D = new Vector2(mouseDir.x, mouseDir.y);
            mouseDir2D = mouseDir2D.normalized;

            //Give the torch a force in the direction of the mouse (from the player), and decrease the torch counter
            torch.GetComponent<Rigidbody2D>().AddForce(mouseDir2D * torchThrowForce);
            torch.GetComponent<Torch>().memoryLength = memoryLength;
            torches--;
            audio.Play("throw");
        }
    }

    /*
     * Kill the player, respawns at the last checkpoint.
     */
    public void Die()
    {
        Debug.Log("You died.");
        audio.Play("hurt");
        //Reset position
        this.transform.position = lastCheckpoint;
    }
}
