using Com.Neko.ThreeDGameProjecct;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossKnockback : MonoBehaviour
{
    private PlayerStates playerStates;
    private Rigidbody playerRig;
    private ForceMotionNew forceMotion;
    [SerializeField] private int damage;
    private GameObject redUI;

    public bool dealDamage;
    bool playerInRange;
    private void Start()
    {
        dealDamage = false;
        redUI = UIManager.instance.UI.transform.Find("HUD/Red").gameObject;
        playerStates = PlayerManager.instance.player.GetComponent<PlayerStates>();
        playerRig = PlayerManager.instance.player.GetComponent<Rigidbody>();
        forceMotion = PlayerManager.instance.player.GetComponent<ForceMotionNew>();
    }
    private void Update()
    {
        if(dealDamage && playerInRange)
        {
            dealDamage = false;
            playerInRange = false;
            forceMotion.speedControlAble = false;
            redUI.SetActive(false);
            playerRig.AddForce(((playerRig.transform.position + Vector3.down * .5f) - transform.position).normalized * 80f, ForceMode.Impulse);
            playerStates.TakeDamage(damage);
            Invoke(nameof(SpeedControlAbleTrue), 0.8f);
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            playerInRange = true;
            redUI.SetActive(true);
        }
        if (other.gameObject.CompareTag("WaterCeiling"))
        {
            WallStatus wallStatus =  other.gameObject.GetComponent<WallStatus>();
            wallStatus.CollisionDeactivate();
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            playerInRange = false;
            redUI.SetActive(false);
        }
    }
    void SpeedControlAbleTrue()
    {
        forceMotion.speedControlAble = true;
    }
}
