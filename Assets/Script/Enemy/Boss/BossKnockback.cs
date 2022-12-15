using Com.Neko.ThreeDGameProjecct;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossKnockback : MonoBehaviour
{
    private PlayerStates playerStates;
    private Rigidbody playerRig;
    private ForceMotionNew forceMotion;
    private void Start()
    {
        playerStates = PlayerManager.instance.player.GetComponent<PlayerStates>();
        playerRig = PlayerManager.instance.player.GetComponent<Rigidbody>();
        forceMotion = PlayerManager.instance.player.GetComponent<ForceMotionNew>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            forceMotion.speedControlAble = false;
            playerRig.AddForce(((playerRig.transform.position + Vector3.down * .5f) - transform.position).normalized * 80f, ForceMode.Impulse);
            Invoke(nameof(speedControlAbleTrue), 1f);
        }
        if (other.gameObject.CompareTag("WaterCeiling"))
        {
            WallStatus wallStatus =  other.gameObject.GetComponent<WallStatus>();
            wallStatus.CollisionDeactivate();
        }
    }
    void speedControlAbleTrue()
    {
        forceMotion.speedControlAble = true;
    }
}
