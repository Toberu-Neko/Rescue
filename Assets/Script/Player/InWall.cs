using Com.Neko.ThreeDGameProjecct;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InWall : MonoBehaviour
{
    private ForceMotionNew forceMotion;
    private Rigidbody rig;
    void Start()
    {
        forceMotion = PlayerManager.instance.player.GetComponent<ForceMotionNew>();
        rig = PlayerManager.instance.player.GetComponent<Rigidbody>();
    }

    private void OnCollisionStay(Collision collision)
    {
        if(collision.gameObject.GetComponent<JumpPad>() != null)
        {
            Debug.Log("OnJumpPad");
        }
        if (forceMotion.state == ForceMotionNew.MovementState.air && collision.gameObject.GetComponent<JumpPad>() == null)
        {
            Debug.Log("In Wall.");
            rig.velocity = new Vector3(rig.velocity.x, -5f, rig.velocity.z);
        }
    }
}
