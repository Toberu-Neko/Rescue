using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using TMPro;
using Unity.Collections;
using UnityEditor;
using UnityEngine;

namespace Com.Neko.ThreeDGameProjecct
{
    public class ForceMotionNew : MonoBehaviour
    {
        #region Variables
        private float moveSpeed;//原為orgSpeed
        [Header("玩家速度")]
        public float walkSpeed;
        public float sprintSpeed;
        public float groundDrag;
        public float slideSpeed;
        public float jumpPadForce;
        [Range(0f, 0.999f)]
        public float airMultiplier;
        public float speedIncreaseMultiplier;
        public float slopeIncreaseMultiplier;
        [HideInInspector] public bool speedControlAble;
        //public float playerHeight;

        private float desiredMoveSpeed;
        private float lastDesiredMoveSpeed;

        [Header("跳躍")]
        public float jumpForce;
        public float jumpCooldown;
        bool readyToJump;
        [Header("蹲下")]
        public float crouchSpeed;
        public float crouchSuckToGorundMutiplier;
        public float crouchYScale;
        private float startYScale;

        [Header("滑行最小需求角度")]
        public float slideMinAngle;


        [Header("Keybind")]
        public KeyCode jumpKey = KeyCode.Space;
        public KeyCode sprintKey = KeyCode.LeftShift;
        //public KeyCode crouchKey = KeyCode.C;
        //public KeyCode keepCrouchKey = KeyCode.LeftControl;

        [Header("附加物件")]
        public Transform playerTransform;
        public LayerMask ground;
        public LayerMask jumpPad;
        public Transform frontGroundDetector, backGroundDetector;
        public BoxCollider playerCollider;
        //public BoxCollider playerCrouchCollider;
        //public Camera normalCam;
        public Transform orientation;
        public GameObject noteDoubleJumpAble;
        public Animator playerAnimator;


        [Header("斜坡")]
        private RaycastHit slopeHit;
        public float slopeMaxAngle;
        private bool exitingSlope = false;
        public bool isSliding;

        public MovementState state;
        private float hMove, vMove;
        private float defultFOV;
        Vector3 movementDirection;
        private PlayerStates playerStates;
        //private float adjustedSpeed;
        private Rigidbody rig;

        [HideInInspector]public float standingTimer;
        private bool doubleJumpAble;
        bool stand1Started;
        [HideInInspector] public bool isGrounded;
        bool moveable;
        //bool jump, jumped = false;

        #endregion
        #region Monobehaviour Callbacks
        void Start()
        {
            speedControlAble = true;
            stand1Started = false;
            standingTimer = 0;
            playerStates = PlayerManager.instance.player.GetComponent<PlayerStates>();
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            moveable = true;
            //crouchKey = KeyCode.C;
            //defultFOV = normalCam.fieldOfView;
            //Camera.main.enabled = false;
            rig = GetComponent<Rigidbody>();
            //playerCollider = GetComponent<BoxCollider>();
            rig.freezeRotation = true;

            readyToJump = true;
            doubleJumpAble = false;

            startYScale = playerTransform.localScale.y;
        }
        private void Update()
        {

            /*
            //Controls
            bool sprint = Input.GetKey(KeyCode.RightShift) || Input.GetKey(KeyCode.LeftShift);
            bool jump = Input.GetKeyDown(KeyCode.Space);
            */
            //States
            isGrounded = Physics.Raycast(frontGroundDetector.position, Vector3.down, 0.5f, ground) || Physics.Raycast(backGroundDetector.position, Vector3.down, 0.5f, ground);//Raycast(偵測目標位置, 偵測方向, 偵測離主角距離，小於為真, layerMask)
            //bool isJumping = jump && isGrounded;
            //bool isSprinting = sprint && vMove > 0 && !isJumping && isGrounded;

            if (moveable)
            {
                PlayerInput();
                SpeedControl();
                StateHandler();
            }

            if (state == MovementState.sliding || state == MovementState.crouching)
            {
                playerStates.isCrouching = true;
            }
            else
            {
                playerStates.isCrouching = false;
            }
            if (Cursor.lockState == CursorLockMode.None)
            {
                moveable = false;
                readyToJump = false;
            }
            else if(Cursor.lockState == CursorLockMode.Locked)
            {
                moveable = true;
                readyToJump = true;
            }
            //Animator
            if(state == MovementState.walking)
            {
                playerAnimator.SetBool("isWalking", true);
                playerAnimator.SetBool("isRunning", false);
                playerAnimator.SetBool("Stand1", false);

                stand1Started = false;
            }
            else if(state != MovementState.walking)
            {
                playerAnimator.SetBool("isWalking", false);
            }

            if(state == MovementState.sprinting)
            {
                playerAnimator.SetBool("isRunning", true);
                playerAnimator.SetBool("isWalking", false);
                playerAnimator.SetBool("Stand1", false);

                stand1Started = false;
            }
            else if (state != MovementState.sprinting)
            {
                playerAnimator.SetBool("isRunning", false);
            }


            if (state == MovementState.standing)
            {
                standingTimer += Time.deltaTime;
            }
            if(state != MovementState.standing || playerStates.isAiming)
            {
                standingTimer = 0;
            }
            if(standingTimer >= 5f && !stand1Started)
            {
                stand1Started = true;
                playerAnimator.SetBool("Stand1", true);
                StartCoroutine(Stand1Animation());
            }
            if (playerAnimator.GetCurrentAnimatorStateInfo(0).IsName("Base Layer_CatJump"))
            {
                playerAnimator.SetBool("Jump", false);
            }
            if (rig.velocity.y<0 && isGrounded)
            {
                playerAnimator.SetBool("Jump", false);
            }

            //Drag
            if (isGrounded)
            {
                rig.drag = groundDrag;
            }
            else
                rig.drag = 0;

            /*if (doubleJumpAble)
            {
                noteDoubleJumpAble.SetActive(true);
            }
            if (!doubleJumpAble)
            {
                noteDoubleJumpAble.SetActive(false);
            }*/
        }

        void FixedUpdate()
        {
            if(moveable)
            Movement();

        }
        #endregion
        #region States
        public enum MovementState
        {
            standing,
            walking,
            sprinting,
            sliding,
            crouching,
            air
        }
        private void StateHandler()
        {
            if (isSliding && !playerStates.isAiming)
            {
                if (OnSlope() && rig.velocity.y < 0.2f)
                {
                    state = MovementState.sliding;
                    
                    desiredMoveSpeed = slideSpeed;
                }
                else
                {
                    state = MovementState.crouching;
                    desiredMoveSpeed = crouchSpeed;
                }
            }
          /*  else if (Input.GetKey(crouchKey) && !playerStates.isAiming)
            {
                state = MovementState.crouching;
                desiredMoveSpeed = crouchSpeed;
            }*/
            else if (isGrounded && Input.GetKey(sprintKey) && !playerStates.isAiming)
            {
                state = MovementState.sprinting;
                desiredMoveSpeed = sprintSpeed;
            }
            else if(hMove == 0 && vMove == 0 && isGrounded)
            {
                state = MovementState.standing;
            }
            else if(isGrounded && Cursor.lockState == CursorLockMode.Locked)
            {
                state = MovementState.walking;
                StopAllCoroutines();
                desiredMoveSpeed = walkSpeed;
            }
            else
            {
                state = MovementState.air;
            }
            //檢查是否立即變更desiredMoveSpeed
            if (Mathf.Abs(desiredMoveSpeed - lastDesiredMoveSpeed) > sprintSpeed - walkSpeed && moveSpeed != 0) 
            {
                StopAllCoroutines();
                StartCoroutine(SmoothlyLerpMoveSpeed());

            }
            else
            {
                moveSpeed = desiredMoveSpeed;
            }
            lastDesiredMoveSpeed = desiredMoveSpeed;

        }
        #endregion

        private void OnCollisionStay(Collision collision)
        {
            if (state == MovementState.air && !collision.gameObject.CompareTag("JumpPad"))
            {
                //Debug.Log("In Wall.");
                rig.velocity = new Vector3(rig.velocity.x, -7f, rig.velocity.z);
            }
        }
        private void PlayerInput()
        {
            hMove = Input.GetAxisRaw("Horizontal");//水平A+1, D-1
            vMove = Input.GetAxisRaw("Vertical");//垂直W+1, S=1

            JumpPad();
            if (Input.GetKeyDown(jumpKey) && isGrounded && readyToJump)
            {
                //Jump
                readyToJump = false;
                playerAnimator.SetBool("Jump", true);
                Jump();
                #region Invoke用法（註解）
                /*
                public void Invoke(string methodName, float time);
                    -Invoke ( 委派的funtion,幾秒後開始調用 )

                public void InvokeRepeating(string methodName, float time, float repeatRate);
                    -InvokeRepeating ( 委派的funtion, 幾秒後開始調用, 開始調用後每幾秒再調用 ) 

                public bool IsInvoking(string methodName);
                    -IsInvoking ( 委派的funtion ) 判斷是否正在調用中
                */
                #endregion
                Invoke(nameof(resetJump), jumpCooldown);//Invoke(nameof(A), b) A=Function, b=幾秒後執行;
            }
            if(Input.GetKeyDown(jumpKey) && doubleJumpAble)
            {
                DoubleJump();
            }

            if(doubleJumpAble && isGrounded)
            {
                doubleJumpAble = false;
            }
        }
        private void Movement()
        {

            movementDirection = orientation.forward * vMove + orientation.right * hMove;

            if (OnSlope() && !exitingSlope)
            {
                rig.AddForce(GetSlopeMoveDirection(movementDirection) * moveSpeed * 20f, ForceMode.Force);

                if (rig.velocity.y > 0)
                {
                    rig.AddForce(Vector3.down * 80f, ForceMode.Force);
                }
                //Debug.Log("onSlope");
            }
            else if (isGrounded)
                rig.AddForce(movementDirection * moveSpeed * 10f, ForceMode.Force);
            else if (!isGrounded)
                rig.AddForce(movementDirection * moveSpeed * 10f * airMultiplier, ForceMode.Force);

            rig.useGravity = !OnSlope();

        }
        private void Jump()
        {
            exitingSlope = true;
            //reset t velocity
            rig.velocity = new Vector3(rig.velocity.x, 0f, rig.velocity.z);

            rig.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);//Vector3.up 不考慮rotation, transform.up考慮
        }
        private void DoubleJump()
        {
            //exitingSlope = true;
            //reset t velocity
            rig.velocity = new Vector3(rig.velocity.x, 0f, rig.velocity.z);
            rig.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);//Vector3.up 不考慮rotation, transform.up考慮

            doubleJumpAble = false;
        }
        private void resetJump()
        {
            playerAnimator.SetBool("Jump", false);
            readyToJump = true;

            exitingSlope = false;
        }
        
        private void SpeedControl()
        {
            if (!speedControlAble)
                return;
            Vector3 flatVel = new Vector3(rig.velocity.x, 0f, rig.velocity.z);

            if (OnSlope() && !exitingSlope)
            {
                if(rig.velocity.magnitude > moveSpeed)
                {
                    rig.velocity = rig.velocity.normalized * moveSpeed;
                    //Debug.Log(rig.velocity.magnitude);
                }
            }
            else
            {
                //limit velocity if needed
                if (flatVel.magnitude > moveSpeed)
                {
                    Vector3 limitedVel = flatVel.normalized * moveSpeed;
                    rig.velocity = new Vector3(limitedVel.x, rig.velocity.y, limitedVel.z);
                }
            }

        }

        public bool OnSlope()
        {
            //isGrounded = Physics.Raycast(groundDetector.position, Vector3.down, 0.3f, ground);
            if (Physics.Raycast(frontGroundDetector.position, Vector3.down, out slopeHit, 0.3f)|| Physics.Raycast(backGroundDetector.position, Vector3.down, out slopeHit, 0.3f))
            {
                float angle = Vector3.Angle(Vector3.up, slopeHit.normal);
                //Debug.Log(angle);
                if(angle > slideMinAngle)
                return angle < slopeMaxAngle && angle != 0;
                else
                    return false;
            }
            return false;
        }

        public Vector3 GetSlopeMoveDirection(Vector3 direction)
        {
            return Vector3.ProjectOnPlane(direction, slopeHit.normal).normalized;
        }

        private IEnumerator SmoothlyLerpMoveSpeed()
        {
            float t_time = 0;
            float t_difference = Mathf.Abs(desiredMoveSpeed - moveSpeed);
            float startValue = moveSpeed;

            while (t_time < t_difference)
            {
                moveSpeed = Mathf.Lerp(startValue, desiredMoveSpeed, t_time / t_difference);
                if (OnSlope() && (hMove != 0 && vMove != 0))
                {
                    float slopeAngle = Vector3.Angle(Vector3.up, slopeHit.normal);
                    float slopeAngleIncrease = 1 + (slopeAngle / 90f);

                    t_time += Time.deltaTime * speedIncreaseMultiplier * slopeIncreaseMultiplier * slopeAngleIncrease;
                }
                else if (hMove == 0 && vMove == 0 && t_time > 0)
                {
                    t_time -= Time.deltaTime * speedIncreaseMultiplier;
                }
                else
                    t_time += Time.deltaTime * speedIncreaseMultiplier;
                lastDesiredMoveSpeed = desiredMoveSpeed;
                //moveSpeed = desiredMoveSpeed;
                yield return null;
            }

            moveSpeed = desiredMoveSpeed;
            
        }
        private IEnumerator LateJumpPadForce()
        {
            yield return null;
            rig.AddForce(Vector3.up * jumpPadForce, ForceMode.Impulse);
        }
        private IEnumerator Stand1Animation()
        {
            yield return new WaitForSeconds(1.5f);
            standingTimer = 0;
            playerAnimator.SetBool("Stand1", false);
            stand1Started = false;
            yield return null;
        }
        private void JumpPad()
        {
            bool isOnJumpPad = Physics.Raycast(frontGroundDetector.position, Vector3.down, 0.2f, jumpPad) || Physics.Raycast(backGroundDetector.position, Vector3.down, 0.2f, jumpPad);

            if (isOnJumpPad)
            {
                //rig.velocity = new Vector3(rig.velocity.x, 0, rig.velocity.z);
                //playerCollider.enabled = false;
                rig.velocity = (Vector3.up * jumpPadForce);
                //rig.AddForce(Vector3.up * jumpPadForce, ForceMode.Impulse) ;
                //StartCoroutine(LateJumpPadForce());
                doubleJumpAble = true;
            }
            else
            {
                //playerCollider.enabled = true;
            }
        }
    }
}

