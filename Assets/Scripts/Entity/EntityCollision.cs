﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityCollision : MonoBehaviour
{
    [HideInInspector]
    public CollisionData collisions;
    [HideInInspector]
    public CollisionData prevCollisions;
    public LayerMask collisionMask;
    Bounds bounds;
    int horizontalRays;
    float horizontalRaySpacing;
    int verticalRays;
    float verticalRaySpacing;
    float skinWidth = .2f;
    BoxCollider2D coll;

    // Start is called before the first frame update
    void Start()
    {
       // Get collider
       coll = GetComponent<BoxCollider2D>();
       bounds = coll.bounds;
       horizontalRays = Mathf.RoundToInt(bounds.size.y / 0.1f);
       verticalRays = Mathf.RoundToInt(bounds.size.x / 0.1f);
       horizontalRaySpacing = bounds.size.y / (horizontalRays);
       verticalRaySpacing = bounds.size.x / (verticalRays);
    }

    // Update is called once per frame
    public void Move(Vector2 amount)
    {
        // Update position
        bounds = coll.bounds;
        bounds.Expand(skinWidth * -2);
        prevCollisions = collisions;
        collisions.Reset();

        if (amount.x != 0)
        {
            collisions.facing = (int)Mathf.Sign(amount.x);
        }

        CheckHorizontalCollisions(ref amount);

        if (amount.y != 0)
        {
            CheckVerticalCollisions(ref amount);
        }

        //Debug.Log(amount.x);

        transform.Translate(amount);
    }

    private void CheckHorizontalCollisions(ref Vector2 amount)
    {
        float directionX = Mathf.Sign(amount.x);
        float rayLength = Mathf.Abs(amount.x) + skinWidth;

        for (int i = 0; i < horizontalRays; i++) {
            Vector2 rayOrigin;
            if (Mathf.Sign(amount.y) == 1)
            {
                rayOrigin = directionX == -1 ? new Vector2(bounds.min.x, bounds.max.y + skinWidth - 0.01f) : new Vector2(bounds.max.x, bounds.max.y + skinWidth - 0.01f);
                rayOrigin += Vector2.down * (i * horizontalRaySpacing);
            }
            else
            {
                rayOrigin = directionX == -1 ? new Vector2(bounds.min.x, bounds.min.y - skinWidth + 0.01f) : new Vector2(bounds.max.x, bounds.min.y - skinWidth + 0.01f);
                rayOrigin += Vector2.up * (i * horizontalRaySpacing);
            }

            RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.right * directionX, rayLength, collisionMask);

            Debug.DrawRay(rayOrigin, Vector2.right * directionX * rayLength, Color.red);
            if (hit)
            {
                if (hit.distance == 0 || hit.collider.tag == "Through")
                {
                    continue;
                }

                //Debug.Log("bump");

                amount.x = (hit.distance - skinWidth) * directionX;
                rayLength = hit.distance;
                collisions.left = new CollisionSide(directionX == -1, hit.collider.gameObject.layer);
                collisions.right = new CollisionSide(directionX == 1, hit.collider.gameObject.layer);
            }
        }
    }

    private void CheckVerticalCollisions(ref Vector2 amount)
    {
        float directionY = Mathf.Sign(amount.y);
        float rayLength = Mathf.Abs(amount.y) + skinWidth;

        for (int i = 0; i < verticalRays; i++)
        {
            Vector2 rayOrigin;
            if (Mathf.Sign(amount.x) == 1)
            {
                rayOrigin = directionY == -1 ? new Vector2(bounds.max.x + skinWidth - 0.01f, bounds.min.y) : new Vector2(bounds.max.x + skinWidth - 0.01f, bounds.max.y);
                rayOrigin += Vector2.left * (i * verticalRaySpacing + amount.x);
            }
            else
            {
                rayOrigin = directionY == -1 ? new Vector2(bounds.min.x - skinWidth + 0.01f, bounds.min.y) : new Vector2(bounds.min.x - skinWidth + 0.01f, bounds.max.y);
                rayOrigin += Vector2.right * (i * verticalRaySpacing + amount.x);
            }


            RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.up * directionY, rayLength, collisionMask);

            Debug.DrawRay(rayOrigin, Vector2.up * directionY * rayLength, Color.red);
            if (hit)
            {
                if (hit.collider.tag == "Through")
                {
                    continue;
                }

                amount.y = (hit.distance - skinWidth) * directionY;
                rayLength = hit.distance;
                collisions.top = new CollisionSide(directionY == 1, hit.collider.gameObject.layer);
                collisions.down = new CollisionSide(directionY == -1, hit.collider.gameObject.layer);
            }
        }
    }

    public struct CollisionData
    {
        public CollisionSide top, down, left, right;
        public int facing;

        public void Reset()
        {
            top = down = left = right = false;
        }
    }

    public struct CollisionSide
    {
        public int layer;
        public bool colliding;
        public CollisionSide(bool a, int l)
        {
            colliding = a;
            layer = l;
        }
        public static implicit operator CollisionSide(bool a) => new CollisionSide(a, -1);
        public static implicit operator bool(CollisionSide a) => a.colliding;
    }
}