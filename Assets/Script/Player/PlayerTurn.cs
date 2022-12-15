using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTurn : MonoBehaviour
{
    [Header("References")]
    public Transform orientation;
    public Transform player;
    public Transform playerObj;
    public Transform basicCamLookat;
    public Transform basicCamLookatOrignPosition;
    public Transform inCombatLookat;
    public Rigidbody rb;

    //private Vector3 basicCamLookatStartPosition;

    public float rotationSpeed;


    [Header("附加物件")]
    public GameObject basicCam;
    //public GameObject combatCam;
    public GameObject topDownCam;

    private PlayerStates playerStates;
    private Transform mainCamTransform;
    private CinemachineVirtualCamera basicCamCinemachineComponent;
    //Sensitivity 
    [Header("Cinemachine")]
    public Cinemachine.AxisState xAxis;
    public Cinemachine.AxisState yAxis;

    
    Animator animator;
    int isAimingPram = Animator.StringToHash("isAiming");
    bool moveable;

    public CameraStyle cameraStyle;
    public enum CameraStyle
    {
        Basic,
        InCombat,
        TopDown
    }
    void Start()
    {
        playerStates = PlayerManager.instance.player.GetComponent<PlayerStates>();
        basicCamCinemachineComponent = basicCam.GetComponent<CinemachineVirtualCamera>();
        animator = GetComponent<Animator>();
        //var basicCamBody = basicCamCinemachineComponent.GetCinemachineComponent<CinemachineOrbitalTransposer>();

        mainCamTransform = PlayerManager.instance.mainCamera.transform;
        
        moveable = true;
        CameraStyleChanger(CameraStyle.Basic);

    }

    void Update()
    {
        xAxis.Update(Time.deltaTime);
        yAxis.Update(Time.deltaTime);

        basicCamLookat.eulerAngles = new Vector3(yAxis.Value, xAxis.Value, 0);
        if (Input.GetKeyUp(KeyCode.Mouse1))
        {
            CameraStyleChanger(CameraStyle.Basic);
            cameraStyle = CameraStyle.Basic;
            playerStates.isAiming = false;
            animator.SetBool(isAimingPram, playerStates.isAiming);
        }
        if (Input.GetKeyDown(KeyCode.Mouse1) && !playerStates.isCrouching && Cursor.lockState == CursorLockMode.Locked)
        {
            CameraStyleChanger(CameraStyle.InCombat);
            cameraStyle = CameraStyle.InCombat;
            playerStates.isAiming = true;
            animator.SetBool(isAimingPram, playerStates.isAiming);
        }
        if (Input.GetKeyDown(KeyCode.Alpha9))
        {
            CameraStyleChanger(CameraStyle.TopDown);
            cameraStyle = CameraStyle.TopDown;
            playerStates.isAiming = false;
        }

        if (Cursor.lockState == CursorLockMode.None)
        {
            moveable = false;
        }
        else if (Cursor.lockState == CursorLockMode.Locked)
        {
            moveable = true;
        }


        Vector3 vierDirection = player.position - new Vector3(mainCamTransform.position.x, player.position.y, mainCamTransform.position.z);
        //Vector3 basicCamLookDirection = player.position - new Vector3(thirdPersonCamBasic.transform.position.x,player.position.y, thirdPersonCamBasic.transform.position.z); 
        orientation.forward = vierDirection.normalized;

        if(cameraStyle == CameraStyle.Basic || cameraStyle == CameraStyle.TopDown)
        {
            //垂直。輸入S=-1, 輸入W=1
            float vertical = Input.GetAxis("Vertical");
            //水平。輸入A=-1, 輸入D=1
            float horizontal = Input.GetAxis("Horizontal");
            Vector3 inputDr = orientation.forward * vertical + orientation.right * horizontal;

            if (inputDr != Vector3.zero && moveable)
            {
                playerObj.forward = Vector3.Slerp(playerObj.forward, inputDr.normalized, rotationSpeed * Time.deltaTime);
            }
        }
        else if(cameraStyle == CameraStyle.InCombat)
        {
            basicCamLookat.position = inCombatLookat.position;

            Vector3 combatViewDirection = inCombatLookat.position - new Vector3(mainCamTransform.position.x, inCombatLookat.position.y, mainCamTransform.position.z);

            //playerObj.forward = vierDirection.normalized;

            orientation.forward = combatViewDirection.normalized;
            playerObj.forward = combatViewDirection.normalized;
        }
    }
    void CameraStyleChanger(CameraStyle newStyle)
    {
        basicCam.SetActive(false);
        //combatCam.SetActive(false);
        topDownCam.SetActive(false);

        if(newStyle == CameraStyle.Basic)
        {
            basicCam.SetActive(true);
            basicCamLookat.position = basicCamLookatOrignPosition.position;
            yAxis.m_MaxValue = 90;
        }
        if (newStyle == CameraStyle.InCombat)
        {
            basicCam.SetActive(true);
            yAxis.m_MaxValue = 60;
            
        }
        if (newStyle == CameraStyle.TopDown) topDownCam.SetActive(true);
    }
/*    IEnumerator ChangeBasicCamLook()
    {

    }*/
}
