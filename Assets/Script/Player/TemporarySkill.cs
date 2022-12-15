using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TemporarySkill : MonoBehaviour
{
    private PlayerCombat playerCombat;
    private SkillState waterTSkill;
    private Image temporarySkillCircle;
    float inWaterRangeTimer;
    bool inWaterRange;
    public bool skillSelected;
    void Start()
    {
        playerCombat = GetComponent<PlayerCombat>();
        temporarySkillCircle = UIManager.instance.UI.transform.Find("HUD/WaterCDCircle").gameObject.GetComponent<Image>();
        waterTSkill = SkillManager.instance.waterTemporary.GetComponent<SkillState>();

        temporarySkillCircle.fillAmount = 0;
        temporarySkillCircle.color = Color.blue;
        temporarySkillCircle.enabled = false;
        skillSelected = false;

        inWaterRangeTimer = 0;
    }

    void Update()
    {
        if(temporarySkillCircle.fillAmount == 0)
        {
            skillSelected = false;
        }
        if (inWaterRange && inWaterRangeTimer < waterTSkill.skill.cooldown && !skillSelected && !playerCombat.throwColldownActivated) 
        {
            inWaterRangeTimer += Time.deltaTime;

            temporarySkillCircle.enabled = true;
            temporarySkillCircle.color = Color.blue;
            temporarySkillCircle.fillAmount = inWaterRangeTimer / waterTSkill.skill.cooldown;
        }
        if (!inWaterRange && inWaterRangeTimer > 0)
        {
            inWaterRangeTimer -= Time.deltaTime;

            temporarySkillCircle.enabled = true;
            temporarySkillCircle.color = Color.blue;
            temporarySkillCircle.fillAmount = inWaterRangeTimer / waterTSkill.skill.cooldown;
        }
        if(inWaterRangeTimer >= waterTSkill.skill.cooldown && !skillSelected)
        {
            skillSelected = true;
            playerCombat.SelectSkill(-1);
            inWaterRangeTimer = 0;
            Debug.Log("Changed to -1");
        }
        //Debug.Log(inWaterRangeTimer / waterTSkill.skill.cooldown);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("WaterActivateRegion"))
        {
            inWaterRange = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("WaterActivateRegion"))
        {
            inWaterRange = false;
        }
    }
}
