using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "新能力", menuName = "newSkill")]
public class Skill : ScriptableObject
{
    public int skillSlot;
    [Header("基本數值")]
    public int damage;
    [Tooltip("每fireRate秒可以射出一發能力")]
    public float fireRate;
    [Tooltip("每射出一發能力cooldown-fireRate，cooldown <= 0 就能力過熱")]
    public float cooldown;
    [Tooltip("能力過熱後，經過overHeatCD開始回復冷卻")]
    public float overHeatCD;
    [Tooltip("停止發射能力後，經過startCountDownTime開始回復冷卻")]
    public float startCountDownTime;

    [Header("次要能力")]
    public bool isTemporarySkill;
    public bool waterTemporary;

    [Header("投擲設定")]
    public float maxThrowForce;
    public float minThrowForce;
    public float maxRightThrowForce;
    public float minRightThrowForce;
    public float throwUpwardForce;

    [Header("雜項設定")]
    public float dieTime;
    public float aimDetectRange;

    public GameObject hitParticle;
}
