using Com.Neko.ThreeDGameProjecct;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.XR;

public class Sliding : MonoBehaviour
{
    [Header("≈‹º∆")]
    public Transform orientation;
    public Transform playerObj;
    private Rigidbody playerRig;
    private ForceMotionNew playerMoveScript;
    public BoxCollider playerCollider;
    public BoxCollider playerCrouchCollider;

    [Header("∑∆¶Ê")]
    public float maxSlideTime;
    public float slideForce;
    private float slideTimer;

    public float slideYScale;
    private float startYScale;

    [Header("Input")]
    public KeyCode slideKey = KeyCode.LeftControl;
    public KeyCode slideKey2 = KeyCode.C;
    private float h_Input, v_Input;
    private PlayerStates playerStates;


    //private bool sliding = false;
    void Start()
    {
        playerStates = PlayerManager.instance.player.GetComponent<PlayerStates>();
        playerRig = GetComponent<Rigidbody>();
        playerMoveScript = GetComponent<ForceMotionNew>();

        startYScale = playerObj.localScale.y;
    }

    // Update is called once per frame
    void Update()
    {
        h_Input = Input.GetAxisRaw("Horizontal");
        v_Input = Input.GetAxisRaw("Vertical");

        if((Input.GetKeyDown(slideKey) || Input.GetKeyDown(slideKey2)) && !playerStates.isAiming && Cursor.lockState == CursorLockMode.Locked)
        {
            StartSlide();
        }
        if((Input.GetKeyUp(slideKey) || Input.GetKeyUp(slideKey2)) && playerMoveScript.isSliding && Cursor.lockState == CursorLockMode.Locked)
        {
            StopSlide();
        }
    }

    private void FixedUpdate()
    {
        if (playerMoveScript.isSliding)
            SlidingMovement();    
    }
    private void StartSlide()
    {
        playerMoveScript.isSliding = true;
        //playerCollider.enabled = false;
        playerObj.localScale = new Vector3 (playerObj.localScale.x, slideYScale, playerObj.localScale.z);
        //playerCrouchCollider.enabled = true;
        playerRig.AddForce(Vector3.down * 5f, ForceMode.Impulse);

        slideTimer = maxSlideTime;

    }
    private void SlidingMovement()
    {
        Vector3 inputDirection = orientation.forward * v_Input + orientation.right * h_Input;

        // sliding normal
        if(!playerMoveScript.OnSlope() || playerRig.velocity.y > -0.1f)
        {
            playerRig.AddForce(inputDirection.normalized * slideForce, ForceMode.Force);
            slideTimer -= Time.deltaTime;
        }
        else
        {
            playerRig.AddForce(playerMoveScript.GetSlopeMoveDirection(inputDirection) * slideForce, ForceMode.Force);
        }


        if (slideTimer < 0)
        {
            StopSlide();
        }
    }
    private void StopSlide()
    {
        playerMoveScript.isSliding = false;
        //playerCrouchCollider.enabled = false;
        transform.position = new Vector3(transform.position.x, transform.position.y + 0.75f, transform.position.z);
        playerObj.localScale = new Vector3(playerObj.localScale.x, startYScale, playerObj.localScale.z);
        //playerCollider.enabled = true;
    }
}
