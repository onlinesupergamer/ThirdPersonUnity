using Unity.VisualScripting;
using UnityEngine;


/* Check This code For Optimization
* When Possible

THIS IS A DEV REFACTOR FOR THE PLAYER MOVEMENT
THERE MAY BE BUGS HERE, DO NOT USE FOR REAL PLAYER YET

*/
public class DEV_PlayerMovement : MonoBehaviour
{
    Vector3 moveDirection;
    Transform cameraObject;
    //PlayerAnimator playerAnimator;

    Animator animator;
    public Vector2 movementInput;
    Rigidbody rb;
    
    public float movementSpeed = 5;

    float snappedHorizontal;
    float snappedvertical;

    public bool bCannotMove = true;
    public bool bIsGrounded;
    public float inAirTimer;
    public float leapVelocity;
    public float fallingSpeed;
    public LayerMask groundLayer;

    public float currentGravity = 10;



    public float gravityIntensity = -15;
    public float jumpHeight = 3;

    public bool isJumping;

    public float rayHeightOffset = -0.95f;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        cameraObject = Camera.main.transform;
        //playerAnimator = GetComponent<PlayerAnimator>();
        animator = GetComponent<Animator>();
    }


    private void FixedUpdate()
    {
        HandleFalling();
        HandleAnimation();
        HandleMovement();
        HandleRotation();
        HandleJump();

        

        //playerAnimator.AnimatorUpdate();
    }

    public void HandleMovement()
    {
        if (isJumping)      // Check to refactor these if statements
            return;

        if (bCannotMove)
            return;

        // Create Var for movement strings
        movementInput[0] = Input.GetAxisRaw("Horizontal");
        movementInput[1] = Input.GetAxisRaw("Vertical");


        moveDirection = cameraObject.forward * movementInput[1];
        moveDirection = moveDirection + cameraObject.right * movementInput[0];
        moveDirection.Normalize();

        moveDirection.y = 0;
        moveDirection = moveDirection * movementSpeed;

        Vector3 movementVelocity = moveDirection;

        rb.velocity = movementVelocity;


    }

    public void HandleRotation()
    {

        if (isJumping)
            return;

        Vector3 targetDirection = Vector3.zero;

        targetDirection = cameraObject.forward * movementInput[1];
        targetDirection = targetDirection + cameraObject.right * movementInput[0];
        targetDirection.Normalize();
        targetDirection.y = 0;

        if (targetDirection == Vector3.zero)
            targetDirection = transform.forward;

        Quaternion targetRotation = Quaternion.LookRotation(targetDirection);
        Quaternion playerRotation = Quaternion.Slerp(transform.rotation, targetRotation, 10 * Time.deltaTime);


        transform.rotation = playerRotation;
    }

    void HandleFalling() 
    {
        RaycastHit hit;
        Vector3 rayStart = transform.position;
        rayStart.y = rayStart.y + rayHeightOffset;

        if (!bIsGrounded && !isJumping)
        {


            inAirTimer = inAirTimer + Time.deltaTime;
            rb.AddForce(transform.forward * leapVelocity);
            rb.AddForce(-Vector3.up * fallingSpeed * inAirTimer);
        }

        if (Physics.SphereCast(rayStart, 0.2f, -Vector3.up, out hit, groundLayer))
        {
            if (!bIsGrounded && !bCannotMove)
            {

            }
            inAirTimer = 0;
            bIsGrounded = true;
        }
        else 
        {
            bIsGrounded = false;
        }


    }


    void HandleAnimation()
    {
        movementInput[1] = Mathf.Abs(movementInput[1]);
        movementInput[0] = Mathf.Abs(movementInput[0]);

        CalculateRotationSnapping();

        //animator.SetFloat("Vertical", Mathf.Clamp01(snappedvertical), 0.1f, Time.deltaTime);
        //animator.SetFloat("Horizontal", Mathf.Clamp01(snappedHorizontal), 0.1f, Time.deltaTime);


    }

    void CalculateRotationSnapping()
    {



        if (movementInput[0] > 0 && movementInput[0] < 0.55f)
        {
            snappedHorizontal = 0.5f;
        }
        else if (movementInput[0] > 0.55f)
        {
            snappedHorizontal = 1;
        }
        else if (movementInput[0] < 0 && movementInput[0] > -0.55f)
        {
            snappedHorizontal = -0.5f;
        }
        else if (movementInput[0] < -0.55f)
        {
            snappedHorizontal = -1;
        }
        else
        {
            snappedHorizontal = 0;
        }

        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        if (movementInput[1] > 0 && movementInput[1] < 0.55f)
        {
            snappedvertical = 0.5f;
        }
        else if (movementInput[1] > 0.55f)
        {
            snappedvertical = 1;
        }
        else if (movementInput[1] < 0 && movementInput[1] > -0.55f)
        {
            snappedvertical = -0.5f;
        }
        else if (movementInput[1] < -0.55f)
        {
            snappedvertical = -1;
        }
        else
        {
            snappedvertical = 0;
        }
    }

    void HandleJump() 
    {
        if (Input.GetButtonDown("Jump"))
        {


            if (isJumping)
            {
                isJumping = false;
                print("Is Already Jumping");

            }
            else if (bIsGrounded) 
            {
                isJumping = true;

                float jumpVelocity = Mathf.Sqrt(-2 * gravityIntensity * jumpHeight);
                Vector3 playerVelocity = moveDirection;
                playerVelocity.y = jumpVelocity;

                rb.velocity = playerVelocity;
                isJumping = true;

            }

        }

        
    }



}





