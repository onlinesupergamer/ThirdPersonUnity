using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


/* Check This code For Optimization
* When Possible



*/
public class PlayerMovement : MonoBehaviour
{
    

    CharacterController characterController; // Gets controller


    public float jumpSpeed = 5f; // Jump velocity

    public float speed = 7f; // Player movement speed
    public float rotationSpeed = 720f; // player rotation speed
    public float jumpButtonGracePeriod = 0.2f; // How forgiving the jumping is; ie, slightly too late off a ledge

    public float gravityAmount = 15f; // How much gravity to be applied

    [SerializeField]
    Transform cameraTransform; // Gets main Camera Automatically; MAKE SURE THE PLAYER CAMERA IS TAGGED AS MAIN CAMERA

    float ySpeed;
    float originalStepOffset;
    float? lastGroundedTime;
    float? jumpButtonPressedTime;

    private void Start()
    {

        characterController = GetComponent<CharacterController>();
        originalStepOffset = characterController.stepOffset;

        cameraTransform = Camera.main.transform; // Should Automatically be set if it's done correctly

    }

    private void Update()
    {
        float horizontalInput = Input.GetAxis("Horizontal");    // Gets the input value
        float verticalInput = Input.GetAxis("Vertical");

        Vector3 movementDirection = new Vector3(horizontalInput, 0, verticalInput); // Creates the movement value
        float magnitude = Mathf.Clamp01(movementDirection.magnitude) * speed; // Clamps the movement value

        movementDirection.Normalize();

        movementDirection = Quaternion.AngleAxis(cameraTransform.eulerAngles.y, Vector3.up) * movementDirection; // Makes the movement value relative to player camera

        ySpeed += gravityAmount * Time.deltaTime; // Applies gravity to player at a rate


        ////////////////////////////////////////////////////////////////////////////////

        //All code here is for jump/coyote jump time

        if (characterController.isGrounded)
        {
            lastGroundedTime = Time.time;
        }

        if (Input.GetButtonDown("Jump"))
        {
            jumpButtonPressedTime = Time.time;
        }

        if (Time.time - lastGroundedTime <= jumpButtonGracePeriod)
        {
            characterController.stepOffset = originalStepOffset;
            ySpeed = -0.5f;

            if (Time.time - jumpButtonPressedTime <= jumpButtonGracePeriod)
            {
                ySpeed = jumpSpeed;
                jumpButtonPressedTime = null;
                lastGroundedTime = null;
            }
        }
        else
        {
            characterController.stepOffset = 0;
        }


        ////////////////////////////////////////////////////////////////////////////////

        Vector3 Velocity = movementDirection * magnitude;
        Velocity.y = ySpeed; // Adds gravity to movement calculation

        characterController.Move(Velocity * Time.deltaTime); // Actually moves the player

        if (movementDirection != Vector3.zero) // This part just calculates player rotation
        {
            Quaternion toRotation = Quaternion.LookRotation(movementDirection, Vector3.up);

            transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, rotationSpeed * Time.deltaTime); // This does the actual rotation
        }
    }

    private void OnApplicationFocus(bool focus)
    {
        // Quality of life thing, locks the mouse when the application is in focus

        if (focus)
        {
            Cursor.lockState = CursorLockMode.Locked;
        }
        else
        {
            Cursor.lockState = CursorLockMode.None;
        }
    }

}
