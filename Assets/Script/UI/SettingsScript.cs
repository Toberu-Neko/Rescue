using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class SettingsScript : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI xNumber, yNumber;
    [SerializeField] private Slider xSlider, ySlider;
    //[SerializeField] private Button applyButton;
    private PlayerTurn playerTurn;
    float orgXSens, orgYSens;
    void Start()
    {
        playerTurn = PlayerManager.instance.player.GetComponent<PlayerTurn>();
        xSlider.value = 1;
        ySlider.value = 1;
        orgXSens = playerTurn.xAxis.m_MaxSpeed;
        orgYSens = playerTurn.yAxis.m_MaxSpeed;
    }


    void Update()
    {
        xNumber.text = "" + (xSlider.value - xSlider.value % .01f);
        yNumber.text = "" + (ySlider.value - ySlider.value % .01f);
    } 
    public void Apply()
    {
        playerTurn.xAxis.m_MaxSpeed = orgXSens * (xSlider.value - xSlider.value % .01f);
        playerTurn.yAxis.m_MaxSpeed = orgYSens * (ySlider.value - ySlider.value % .01f);
    }
}
