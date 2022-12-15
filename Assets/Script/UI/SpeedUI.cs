using Com.Neko.ThreeDGameProjecct;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpeedUI : MonoBehaviour
{
    public Rigidbody player;
    public ForceMotionNew playerFM;
    public Text states;
    public Text speedUI;

    void Update()
    {
        speedUI.text = "Speed = " + player.velocity.magnitude.ToString("0.00");
        states.text = playerFM.state.ToString();
    }
}
