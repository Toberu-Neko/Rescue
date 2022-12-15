using UnityEngine;

public class SkillManager : MonoBehaviour
{
    #region Singleton
    public static SkillManager instance;

    private void Awake()
    {
        instance = this;
    }

    #endregion
    [Header("臨時能力")]
    public GameObject waterTemporary;

    [Header("次要能力")]
    public GameObject noSkill;
    public GameObject testWater;
    public GameObject fire;
    public GameObject debug;

}
