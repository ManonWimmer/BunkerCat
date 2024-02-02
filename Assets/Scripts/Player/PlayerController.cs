using Newtonsoft.Json.Bson;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
#if UNITY_EDITOR
using UnityEditor.Experimental.GraphView;
using UnityEditor.Tilemaps;
#endif
using UnityEngine.EventSystems;
using UnityEngine.Tilemaps;
using UnityEngine;


public class PlayerController : MonoBehaviour
{
    // ----- VARIABLES ----- //
    [Header("Movements :")]
    public bool canMove;
    public float movementSpeed = 10.0f;
    public float turnTimerSet = 0.1f;
    private bool isFacingRight = true;
    private bool isWalking;
    private int facingDirection = 1; // 1 = droite, -1 = gauche
    private bool canFlip;
    private float movementInputDirection;
    private float turnTimer;
    public bool playerInCrate = false; // Public car modifié par la crate qui bloque ses mouvements
    public bool UIOpen = false;

    [Header("Knockback : ")]
    public float knockBackLength, knockBackForce; // Combien de temps le knockback va durer et sa puissance
    private float knockBackCounter; // Compteur lié à la longévité du knockback
    private bool knockback = false;

    [Header("Ground check : ")]
    public bool isGrounded;
    public float groundCheckRadius;
    public Transform groundCheck;
    
    [Header("Ledge check : ")]
    public Transform ledgeCheck;
    public float ledgeCheckDistance = 1f;
    public bool ledgeDetected;
    private bool isTouchingLedge;

    [Header("Wall check : ")]
    public float wallCheckDistance;
    public Transform wallCheck;
    public LayerMask whatIsGround;
    public LayerMask whatIsGroundClimb;
    private bool isTouchingWall;

    [Header("Jump : ")]
    public int amountOfJumps = 1; // Nombre de sauts en +, donc la double saut max
    public float jumpForce = 16.0f;
    public float movementForceInAir;
    public float airDragMultiplier = 0.95f;
    public float variableJumpHeightMultiplier = 0.5f;
    public float jumpTimerSet = 0.15f;
    private bool canNormalJump;
    private float jumpTimer;
    private int amountOfJumpsLeft; // Nombre de sauts restants
    private bool isAttemptingToJump;
    private bool checkJumpMultiplier;

    [Header("Wall jump : ")]
    public float wallHopForce;
    public float wallJumpForce;
    public float wallJumpTimerSet = 0.5f;
    public Vector2 wallHopDirection;
    public Vector2 wallJumpDirection;
    private bool canWallJump;
    private float wallJumpTimer;
    private int lastWallJumpDirection;
    private bool hasWallJumped;

    [Header("Wall slide : ")]
    public float wallSlideSpeed;
    private bool isWallSliding;

    [Header("Wall grab : ")]
    private bool isWallGrabbing;
    private bool wallGrabbed = false;

    [Header("Climbing : ")]
    public float ledgeClimbXOffset1 = 0f;
    public float ledgeClimbYOffset1 = 0f;
    public float ledgeClimbXOffset2 = 0f;
    public float ledgeClimbYOffset2 = 0f;
    private bool canClimbLedge = false;
    private Vector2 ledgePosBot;
    private Vector2 ledgePos1;
    private Vector2 ledgePos2;

    [Header("Attacking : ")]
    public bool isAttacking;

    [Header("Sneaking : ")]
    public bool isSneaking = false;

    [Header("End Game : ")]
    public bool gameFinished = false;

    // Dash :
    /*
    private bool canDash = true;
    private bool isDashing = false;
    [SerializeField]
    private float dashingPower = 24f;
    private float dashingTime = 0.48f;
    private float dashingCooldown = 1f;
    [SerializeField]
    private TrailRenderer tr;*/

    // Autres :
    private Rigidbody2D rb;
    private Animator anim;
    // ----- VARIABLES ----- //
    void Start()
    {
        rb = GetComponent<Rigidbody2D>(); // On récupère le rigidbody du joueur
        anim = GetComponent<Animator>(); // On récupère l'animator
        amountOfJumpsLeft = amountOfJumps;
        wallHopDirection.Normalize(); // Normalize = longueur de 1
        wallJumpDirection.Normalize();
        anim.SetBool("menu", false); // On est plus dans le menu mais dans le jeu, on set bool au cas où
    }

    void Update() // Chaque frame
    {
        if (!gameFinished) 
        {
            CheckKnockback();
            CheckInput();
            CheckMovementDirection();
            UpdateAnimations();
            CheckIfCanJump();
            CheckIfWallSliding();
            CheckJump();
            CheckLedgeClimb();
            CheckWallGrab();
        }
    // Si le jeu est fini => écran de fin -> on ne doit pas pouvoir bouger ! 
    }

    private void FixedUpdate() // 50x par seconde
    {
        if (!gameFinished)
        {
            ApplyMovement();
            CheckSurroundings();
        } 
    }

    public void ChangeUIOPen(bool boolean)
    {
        UIOpen = boolean;
    }

    private void CheckKnockback()
    {
        if (knockBackCounter <= 0) // Pas de KnockBack, alors on peut se déplacer et sauter
        {
            canMove = true;
            knockback = false;
        }
        else if (knockBackCounter > 0) // KnockBack donc pas de déplacement ni de saut possible
        {
            canMove = false;
            knockback = true;

            knockBackCounter -= Time.deltaTime; // On diminue de 1 le compteur par seconde
            anim.SetTrigger("hurt");

            // Pousser le joueur en fonction de quel côté le joueur fait face :
            if (isFacingRight)
            {  // Joueur face à la droite, !sr.flipX : sr.flipX == false
                rb.velocity = new Vector2(-knockBackForce, rb.velocity.y); // KnockBack vers la gauche
            }
            else // Joueur face à la gauche
            {
                rb.velocity = new Vector2(knockBackForce, rb.velocity.y); // KnockBack vers la droite
            }
        }
    }

    // Knockback : 
    public void KnockBack()
    {

        knockBackCounter = knockBackLength; // Initialisation du compteur
        Debug.Log("knockback " + knockBackCounter);

        // On change la vitesse car sinon c'est la dernière enregistrée avant le else (x = 0 car on ne bouge pas et on fait un petit saut en y avec knockBackForce):
        rb.velocity = new Vector2(0f, knockBackForce);

        // Animation hurt :
        anim.SetTrigger("hurt");

        CheckKnockback();
    }

    private void CheckIfWallSliding()
    {
        if (isTouchingWall && rb.velocity.y < 0 && !isGrounded && !knockback) // Joueur en l'air, qui touche le mur
        {
            isWallSliding = true;
        }
        else
        {
            isWallSliding = false;
        }
    }

    private void CheckSurroundings()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, whatIsGround); // Cercle raycast au niveau des pieds

        isTouchingWall = Physics2D.Raycast(wallCheck.position, transform.right, wallCheckDistance, whatIsGroundClimb); // Raycast au niveau du corps

        isTouchingLedge = Physics2D.Raycast(ledgeCheck.position, transform.forward, ledgeCheckDistance, whatIsGroundClimb); // Raycast en haut de la tete

        if (isTouchingWall && !isTouchingLedge && !ledgeDetected) // Il touche un mur et il n'y a rien au niveau de sa tete donc ledge detected
        {
            ledgeDetected = true;
            ledgePosBot = wallCheck.position;
        }
    }

    private void CheckIfCanJump()
    {
        if (isGrounded && rb.velocity.y <= 0.01f) // Au sol
        {
            amountOfJumpsLeft = amountOfJumps; // Il a tous ses sauts restants (1)
        }

        if (isTouchingWall && !knockback) // Si il touche le mur
        {
            checkJumpMultiplier = false;
            canWallJump = true;
        }

        if (amountOfJumpsLeft <= 0) // Plus de sauts restants
        {
            canNormalJump = false;
        }
        else
        {
            canNormalJump = true;
        }

    }

    private void CheckMovementDirection()
    {
        if (isFacingRight && movementInputDirection < 0) // Face à la droite mais va à gauche
        {
            Flip();
        }
        else if (!isFacingRight && movementInputDirection > 0) // Face à la gauche mais va à droite
        {
            Flip();
        }

        if (rb.velocity.x != 0 && !knockback) // Le joueur se déplace
        {
            isWalking = true;
        }
        else
        {
            isWalking = false;
        }
    }

    private void UpdateAnimations() // On set toutes les valeurs de l'animator
    {
        anim.SetFloat("moveSpeed", Mathf.Abs(rb.velocity.x));
        anim.SetBool("isWalking", isWalking);
        anim.SetBool("isGrounded", isGrounded);
        anim.SetFloat("yVelocity", rb.velocity.y);
        anim.SetBool("isWallSliding", isWallSliding);
        anim.SetBool("sneaking", isSneaking);
        anim.SetBool("isWallGrabbing", wallGrabbed);
    }

    private void CheckInput() 
    {
        if (canMove && !knockback)
        {
            movementInputDirection = InputManager.GetInstance().GetMoveDirection().x; 

            bool jumpPressed = InputManager.GetInstance().GetJumpPressed();
            if (jumpPressed)
            {
                if (isGrounded || (amountOfJumpsLeft > 0 && !isTouchingWall)) // Au sol, touche pas de mur et peut sauter -> il saute
                {
                    NormalJump();
                }
                else
                {
                    jumpTimer = jumpTimerSet; 
                    isAttemptingToJump = true; 
                }
            }

            if (isWalking && isTouchingWall) // Se déplace et touche un mur
            {
                if (!isGrounded && movementInputDirection != facingDirection) // Touche le mur en l'air, input dans la direction inverse au joueur
                {
                    canMove = false;
                    canFlip = false;

                    turnTimer = turnTimerSet;
                }
            }

            if (checkJumpMultiplier && !jumpPressed) 
            {
                checkJumpMultiplier = false;
                rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * variableJumpHeightMultiplier);
            }

            /*
            bool dashingPressed = InputManager.GetInstance().GetDashingPressed();
            if (dashingPressed && canDash)
            {
                StartCoroutine(Dash());
                //Debug.Log(isDashing);
            }*/


            bool attackPressed = InputManager.GetInstance().GetAttackPressedAnim();
            //if (Input.GetButtonDown("Jump")) // Si input saut
            if (attackPressed && !isAttacking) // !isAttacking pour pas attaquer quand l'animation est déjà en cours
            {
                isAttacking = true;
                anim.SetTrigger("attacking");
                //Debug.Log("attack");
                StartCoroutine(WaitAnimationAttackEnd(0.75f)); // On attend la fin de l'animation pour mettre isAttacking a false
            }

            bool sneakingPressed = InputManager.GetInstance().GetSneakingPressed();
            if (sneakingPressed && isGrounded)
            {
                isSneaking = true;
                //Debug.Log("is sneaking" + isSneaking);
                anim.SetBool("sneaking", isSneaking);
            }
            else
            {
                isSneaking = false;
            }

            if (DialogueManager.GetInstance().dialogueIsPlaying)
            {
                canMove = false; // Peut pas bouger quand le dialogue est en train de jouer
            }
        }

        if (!canMove &&  !knockback) 
        {
            
            rb.velocity = new Vector2(0, 0);  // On l'arrete si il est en train de se déplacer

            turnTimer -= Time.deltaTime;

            if (turnTimer <= 0)
            {
                canMove = true;
                canFlip = true;
            }
        }
    }
    
    private void CheckWallGrab()
    {
        isWallGrabbing = InputManager.GetInstance().GetClimbingPressed();
        if (!ledgeDetected && isTouchingWall && !isGrounded && isWallGrabbing && !playerInCrate && !UIOpen && !knockback) // Input grab + mur à sa tete et mur à son corps
        {
            //Debug.Log("grab");
            rb.gravityScale = 0; // Pas de gravité, il ne tombe pas;

            if (!wallGrabbed)
            {
                Vector2 moveDirection = new Vector2(movementInputDirection, 0); // Il arrete de monter en y quand il s'accroche au mur
                //Debug.Log(moveDirection.y);
                //StartCoroutine(WaitGrabbed(0.1f));
                wallGrabbed = true;
            }
            else
            {
                Vector2 moveDirection = InputManager.GetInstance().GetMoveDirection();
                anim.SetFloat("moveSpeed", Mathf.Abs(rb.velocity.x));
                float speedModifier = moveDirection.y > 0 ? 0.35f : 1; // On change sa vitesse en fonction de si il descend ou monte le mur (monte doucement et descend vite)
                //Debug.Log(moveDirection.y);
                rb.velocity = new Vector2(rb.velocity.x, movementSpeed * moveDirection.y * speedModifier); // On ralentit la vitesse en y quand le joueur grimpe
                anim.SetFloat("verticalAxis", moveDirection.y); 
            }
            //rb.velocity = new Vector2(moveSpeed * Input.GetAxisRaw("Horizontal"), rb.velocity.y); // Déplacement gauche - droite

        }
        else
        {
            wallGrabbed = false;
            rb.gravityScale = 5; // Retour à la gravité de base
        }


        if (isTouchingWall && !isGrounded && !canClimbLedge) // Touche le mur, en l'air et mur au niveau de sa tete sans input grab
        {
            
            //Debug.Log("wall + not grounded");
            if (!wallGrabbed)
            {
                // Debug.Log("+ not wallgrab");
                //Debug.Log("slide");
                isWallSliding = true; // Il glisse sur le mur (il le grab pas)

            }
        }
        //Debug.Log(wallGrabbed);
    }

    private void CheckJump()
    {
        if (jumpTimer > 0 && !knockback) // Il saute
        {
            // WallJump
            if (!isGrounded && isTouchingWall && movementInputDirection != 0) // Il bouge, en l'air et touche un mur
            {
                Debug.Log("wall jump");
                WallJump(); // Saut depuis le mur
            }
            else if (isGrounded)
            {
                NormalJump(); // Saut depuis le sol
            }
        }

        if (isAttemptingToJump) 
        {
            jumpTimer -= Time.deltaTime;
        }

        if (wallJumpTimer > 0) // Il saute depuis le mur
        {
            if (hasWallJumped && movementInputDirection == -lastWallJumpDirection) // Il a sauté mais il change de direction
            {
                rb.velocity = new Vector2(rb.velocity.x, 0.0f); // y à 0
                hasWallJumped = false;
            }
            else if (wallJumpTimer <= 0) // Il ne saute plus
            {
                hasWallJumped = false;
            }
            else
            {
                wallJumpTimer -= Time.deltaTime;
            }
        }
    }


    private void NormalJump()
    {
        if (canNormalJump && !playerInCrate && !UIOpen && !knockback )
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce); // Saut
            amountOfJumpsLeft--;
            jumpTimer = 0;
            isAttemptingToJump = false;
            checkJumpMultiplier = true;
        }
    }

    private void WallJump()
    {
        if (canWallJump && !playerInCrate && !UIOpen && !knockback)
        {
            //Debug.Log("ici");
            rb.velocity = new Vector2(rb.velocity.x, 0.0f); // Y 0 donc il ne bouge plus sur le mur
            isWallSliding = false;
            amountOfJumpsLeft = amountOfJumps;
            amountOfJumpsLeft--; 

            Vector2 forceToAdd = new Vector2(wallJumpForce * wallJumpDirection.x * -facingDirection, wallJumpForce * wallJumpDirection.y);
            Debug.Log(wallJumpForce * wallJumpDirection.x * -facingDirection + " " + wallJumpForce * wallJumpDirection.y); 
            rb.AddForce(forceToAdd, ForceMode2D.Impulse); // Saut Impulse pour que ce soit brusque / instantané
            jumpTimer = 0;
            isAttemptingToJump = false;
            checkJumpMultiplier = true;
            turnTimer = 0;
            canFlip = true;
            hasWallJumped = true;
            wallJumpTimer = wallJumpTimerSet;
            lastWallJumpDirection = -facingDirection;
        }
    }

    private void ApplyMovement()
    {

        if (!isGrounded && !isWallSliding && movementInputDirection == 0 && !playerInCrate && !UIOpen && !knockback && !canClimbLedge) // Dans les airs
        {
            canFlip = true;
            rb.velocity = new Vector2(rb.velocity.x * airDragMultiplier, rb.velocity.y); // Air drag multiplier
        }
        else if (canMove && !playerInCrate && !UIOpen && !knockback && !canClimbLedge) // Au sol
        {
            canFlip = true;
            rb.velocity = new Vector2(movementSpeed * movementInputDirection, rb.velocity.y); // Déplacement gauche droite
        }


        if (isWallSliding && !playerInCrate && !UIOpen && !knockback && !canClimbLedge) // Wall slide
        {
            if (rb.velocity.y < -wallSlideSpeed) // il descend trop vite
            {
                rb.velocity = new Vector2(rb.velocity.x, -wallSlideSpeed); // Chute lente
            }
        }

        if (!UIOpen)
        {
            Cursor.visible = false;
        }
    }

    private void Flip()
    {
        if (!isWallSliding && canFlip && !canClimbLedge && !playerInCrate && !UIOpen && !knockback) // Changement de cote le joueur
        {
            facingDirection *= -1;
            isFacingRight = !isFacingRight;
            transform.Rotate(0.0f, 180.0f, 0.0f);
        }
    }

    private void CheckLedgeClimb()
    {
        if (ledgeDetected && !canClimbLedge && !playerInCrate && !UIOpen && !knockback) // Rien au niveau de la tete mais ne grimpe pas encore
        {
            Debug.Log("climb");
            canFlip = false;
            canClimbLedge = true;
            isWallGrabbing = false;

            // On cale bien la position de début et de fin de climb avec les offsets
            if (isFacingRight)
            {
                ledgePos1 = new Vector2(Mathf.Floor(ledgePosBot.x + ledgeCheckDistance) - ledgeClimbXOffset1, Mathf.Floor(ledgePosBot.y) + ledgeClimbYOffset1);
                ledgePos2 = new Vector2(Mathf.Floor(ledgePosBot.x + ledgeCheckDistance) + ledgeClimbXOffset2, Mathf.Floor(ledgePosBot.y) + ledgeClimbYOffset2);
            }
            else
            {
                ledgePos1 = new Vector2(Mathf.Ceil(ledgePosBot.x - ledgeCheckDistance) + ledgeClimbXOffset1, Mathf.Floor(ledgePosBot.y) + ledgeClimbYOffset1);
                ledgePos2 = new Vector2(Mathf.Ceil(ledgePosBot.x - ledgeCheckDistance) - ledgeClimbXOffset2, Mathf.Floor(ledgePosBot.y) + ledgeClimbYOffset2);
            }

            canMove = false;
            canFlip = false;

            anim.SetBool("canClimbLedge", canClimbLedge); // Grimpe
        }

        if (canClimbLedge && !knockback) // Grimpe
        {
            transform.position = ledgePos1; // On déplace le joueur à la position du début d'anim
        }
    }

    public void FinishLedgeClimb() // Variables après climb -> appelé à la fin de l'anim ledgeClimb
    {
        Debug.Log("finish ledge climb");
        canClimbLedge = false;
        transform.position = ledgePos2; // Position de fin d'anim
        canMove = true;
        canFlip = true;
        ledgeDetected = false;
        anim.SetBool("canClimbLedge", canClimbLedge);
    }

    /*
    IEnumerator Dash()
    {
        if(!playerInCrate && !UIOpen && !knockback) 
        {
            canDash = false;
            isDashing = true;
            rb.velocity = new Vector2(0, 0);
            anim.SetBool("dashing", isDashing);
            float originalGravity = rb.gravityScale;
            rb.gravityScale = 0;

            if (isFacingRight)
            {
                rb.velocity = new Vector2(dashingPower, 0f);
                Debug.Log(rb.velocity.x);
            }
            else
            {
                rb.velocity = new Vector2(-dashingPower, 0f);
                Debug.Log(rb.velocity.x);
            }


            tr.emitting = true;
            yield return new WaitForSeconds(dashingTime);
            tr.emitting = false;
            rb.gravityScale = originalGravity;
            isDashing = false;
            anim.SetBool("dashing", isDashing);
            yield return new WaitForSeconds(dashingCooldown);
            canDash = true;
            rb.velocity = new Vector2(0, 0);
        }
    }*/

    IEnumerator WaitAnimationAttackEnd(float animationDuration)
    {
        //yield return new WaitWhile(() => anim.GetCurrentAnimatorStateInfo(0).IsName(animName));
        yield return new WaitForSeconds(animationDuration);
        isAttacking = false;
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);

        Gizmos.DrawLine(wallCheck.position, new Vector3(wallCheck.position.x + wallCheckDistance, wallCheck.position.y, wallCheck.position.z));
    }
#endif
}

