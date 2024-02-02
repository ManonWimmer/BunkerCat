using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallCollision : MonoBehaviour
{
    // ----- VARIABLES ----- // 
    public LayerMask groundLayer;
    public bool grounded;
    public bool onWall;
    public bool onRightWall;
    public bool onLeftWall;
    public float collisionRadius;
    public Vector2 rightOffset;
    public Vector2 leftOffset;
    public int side;

    private PlayerController playerController;
    // ----- VARIABLES ----- //
    private void Awake()
    {
        playerController = GameObject.Find("Player").GetComponent<PlayerController>();
    }
    private void Update()
    {
        grounded = playerController.isGrounded;
        onWall = Physics2D.OverlapCircle((Vector2)transform.position + rightOffset, collisionRadius, groundLayer) || Physics2D.OverlapCircle((Vector2)transform.position + leftOffset, collisionRadius, groundLayer);
        onRightWall = Physics2D.OverlapCircle((Vector2)transform.position + rightOffset, collisionRadius, groundLayer);
        onLeftWall = Physics2D.OverlapCircle((Vector2)transform.position + leftOffset, collisionRadius, groundLayer);
        side = onRightWall ? 1 : -1;

    }


}
