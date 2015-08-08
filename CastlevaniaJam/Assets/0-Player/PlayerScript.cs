using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerScript : MonoBehaviour
{
    #region Variables

    private Rigidbody2D rigBody2D;
    public float maxHorizontalSpeedGrounded = 1.5f;
    public float maxHorizontalSpeedAir = 1.5f;
    public float maxFallingSpeed = -5f;
    public float movementForce = 20f;
    public float jumpVerticalForce = 1000f;
    public float jumpHorizontalForce = 200f;
    public float _gravityScale = 3f;
    public float forceToAdd;
    
    private bool isGrounded;
    private bool isCrouching;
    private bool whipAttackTrigger;
    private bool canMove;
    private bool canFlip;
    private bool canJump;
    private bool jump;
    public Animator animatorController;

    Transform activePlatform;
    Vector3 activeLocalPlatformPoint;
    Vector3 activeGlobalPlatformPoint;
    Vector3 lastPlatformVelocity;
    Vector3 _MoveDistance;
    
    private BoxCollider2D ColliderRef;
    public LayerMask CollideWith;

    #endregion

    #region Raycasting Settings
        #region Ground check Rays Settings
        
        public int verticalRaysAmount = 3;
        public float verticalLenght = 0.4f;
        public float horizontalOffset = -0.01f;
        public float verticalOffset = 0.3f;

        #endregion
        

        #region Platform check Rays Settings

        public float PR_lenght = 1;
        public float PR_verticalOffset = 0;

        #endregion
    #endregion

    #region AnimatorHashes

    int hsIdle = Animator.StringToHash("Base Layer.Idle");
    int hsWalking = Animator.StringToHash("Base Layer.Walking");
    int hsCrouch_Down = Animator.StringToHash("Base Layer.Crouch_Down");
    int hsCrouch_Up = Animator.StringToHash("Base Layer.Crouch_Up");
    int hsWhipAttack_Standing = Animator.StringToHash("Base Layer.WhipAttack_Standing");
    int hsJump_Up = Animator.StringToHash("Base Layer.Jump_Up");
    int hsFalling = Animator.StringToHash("Base Layer.Falling");

    #endregion

    #region Hitboxes

    public List<PolygonCollider2D> Whip_Hitboxes;

    #endregion

    public Vector2 Velocity;

    void Start () 
    {
        rigBody2D = this.GetComponent<Rigidbody2D>();
        ColliderRef = GetComponent<BoxCollider2D>();
        activePlatform = null;
    }

    void LateUpdate()
    {
        isGrounded = CheckForGroundedState();
        SetSpriteDirection();
        /*if (isGrounded)
        {*/
            HandlePlatforms();
        /*}*/        
        CheckAnimationState();
        HandlePlayerInput();
        RoundSpeed();        

        if (activePlatform != null)
        {
            activeGlobalPlatformPoint = transform.position;
            activeLocalPlatformPoint = activePlatform.transform.InverseTransformPoint(transform.position);
        }

        SetAnimations();
    }
	
    private void SetSpriteDirection()
    {
        //if (rigBody2D.velocity.x > 0.01f)
        //    this.GetComponentsInChildren<Transform>()[1].localScale = new Vector3(1, 1, 1);
        //if (rigBody2D.velocity.x < -0.01f)
        //    this.GetComponentsInChildren<Transform>()[1].localScale = new Vector3(-1, 1, 1);
        if (Input.GetKey(KeyCode.D) && canFlip)
            this.GetComponentsInChildren<Transform>()[1].localScale = new Vector3(1, 1, 1);
        if (Input.GetKey(KeyCode.A) && canFlip)
            this.GetComponentsInChildren<Transform>()[1].localScale = new Vector3(-1, 1, 1);
    }

    public void CheckAnimationState()
    {        
        canMove = true;
        canFlip = true;
        canJump = true;
        int state = animatorController.GetCurrentAnimatorStateInfo(0).fullPathHash;

        if (state == hsCrouch_Down || state == hsCrouch_Up || state == hsWhipAttack_Standing)
            canMove = false;

        if (state == hsWhipAttack_Standing || state == hsJump_Up || state == hsFalling)
        {
            canMove = false;
            canFlip = false;
            canJump = false;
        }
    }

    public void HandlePlayerInput()
    {        

        if(canMove && Input.GetKey(KeyCode.D))
        {
            rigBody2D.AddForce(this.transform.right * movementForce);
        }

        if(canMove && Input.GetKey(KeyCode.A))
        {
            rigBody2D.AddForce(-this.transform.right * movementForce);
        }

        // Arreglar los saltos, no toma bien la fuerza
        if(Input.GetKeyDown(KeyCode.Space) && isGrounded && canJump)
        {               
            if (Input.GetKey(KeyCode.D))
            {
                rigBody2D.AddForce(new Vector2(jumpHorizontalForce, jumpVerticalForce));
            }
            else if (Input.GetKey(KeyCode.A))
            {
                rigBody2D.AddForce(new Vector2(-jumpHorizontalForce, jumpVerticalForce));
            }
            else
            {
                rigBody2D.AddForce(this.transform.up * jumpVerticalForce);
            }
        }

        isCrouching = (isGrounded && Input.GetKey(KeyCode.S));

        #region Attacks

        if(Input.GetKeyDown(KeyCode.C))
        {
            whipAttackTrigger = true;
        }

        #endregion
    }

    public bool CheckForGroundedState()
    {
        Vector2[] rays = new Vector2[verticalRaysAmount];
        Vector3 center = transform.TransformPoint(ColliderRef.offset);
        float colliderWidth = transform.localScale.x * ColliderRef.size.x - horizontalOffset * 2;
        float colliderLeft = center.x - colliderWidth/2;
        float colliderHeight = transform.localScale.y * ColliderRef.size.y;
        float colliderBottom = (center.y - colliderHeight / 2) + verticalOffset;
        float spaceWithinRays = colliderWidth / (rays.Length - 1);
        
        for (int i = 0; i < rays.Length; i++)
        {
            rays[i] = new Vector2((colliderLeft + i * spaceWithinRays), colliderBottom);
        }

        foreach(Vector2 ray in rays)
        {
            Debug.DrawRay(ray, new Vector3(0f, -verticalLenght, 0f), Color.white);
            if (Physics2D.Raycast(ray, -this.transform.up, verticalLenght, CollideWith))
            {                
                return true;
            }
        }
        return false;
    }

    public void RoundSpeed()
    {
        Velocity = rigBody2D.velocity;
        if (isGrounded)
        {            
            if (Mathf.Abs(Velocity.x) > maxHorizontalSpeedGrounded)
            {
                rigBody2D.velocity = new Vector2(Mathf.Sign(Velocity.x) * maxHorizontalSpeedGrounded, Velocity.y);
            }
        }
        else
        {
            if (Mathf.Abs(Velocity.x) > maxHorizontalSpeedAir)
            {
                rigBody2D.velocity = new Vector2(Mathf.Sign(Velocity.x) * maxHorizontalSpeedAir, Velocity.y);
            }
        }

        if (Velocity.y < maxFallingSpeed) rigBody2D.gravityScale = 0;
        else
            rigBody2D.gravityScale = _gravityScale;
    }

    private void HandlePlatforms()
    {
        if (activePlatform != null)
        {
            var newGlobalPlatformPoint = activePlatform.transform.TransformPoint(activeLocalPlatformPoint);
            var moveDistance = newGlobalPlatformPoint - activeGlobalPlatformPoint;
            _MoveDistance = moveDistance;
            if (moveDistance != Vector3.zero)
                transform.Translate(moveDistance, Space.World);

            lastPlatformVelocity = (newGlobalPlatformPoint - activeGlobalPlatformPoint) / Time.deltaTime;
        }
        else
        {
            lastPlatformVelocity = Vector3.zero;
        }

        activePlatform = null;

        Vector3 center = transform.TransformPoint(ColliderRef.offset);
        float colliderHeight = transform.localScale.y * ColliderRef.size.y;
        float colliderBottom = (center.y - colliderHeight / 2) + PR_verticalOffset;
        Vector2 rayVector = new Vector2(center.x, colliderBottom + 0.2f);

        var raycastHit = Physics2D.Raycast(rayVector, -Vector2.up, PR_lenght, CollideWith);
        Debug.DrawRay(rayVector, -Vector3.up * PR_lenght, Color.red);
        if(raycastHit)
        {            
            activePlatform = raycastHit.collider.gameObject.transform;
        }

    }

    public void SetAnimations()
    {
        animatorController.SetFloat("velocityX", Mathf.Abs(rigBody2D.velocity.normalized.x));
        animatorController.SetFloat("velocityY", rigBody2D.velocity.y);
        animatorController.SetBool("whipAttack", whipAttackTrigger); whipAttackTrigger = false;
        animatorController.SetBool("isGrounded", isGrounded);
        animatorController.SetBool("isCrouching", isCrouching);       
    }

    public void EnableFlip()
    {
        canFlip = true;
    }
    
    public void setForce(string force)
    {
        float resul;
        if (float.TryParse(force, out resul))
            forceToAdd = resul;
    }

    public void applyForce(string dir)
    {
        switch(dir)
        {
            case "up":
                rigBody2D.AddForce(Vector2.up * forceToAdd);
                break;

            case "down":
                rigBody2D.AddForce(-Vector2.up * forceToAdd);
                break;

            case "left":
                rigBody2D.AddForce(Vector2.left * forceToAdd);
                break;

            case "right":
                rigBody2D.AddForce(-Vector2.left * forceToAdd);
                break;
        }
    }
}
