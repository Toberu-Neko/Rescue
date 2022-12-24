using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class ClickButtonSound : MonoBehaviour
{
    private Button btn;
    private EventTrigger eventTrigger;
    // Start is called before the first frame update
    void Start()
    {
        if(GetComponent<EventTrigger>() == null)
        {
            gameObject.AddComponent<EventTrigger>();
        }
        eventTrigger = GetComponent<EventTrigger>();
        EventTrigger.Entry entry = new();
        entry.eventID = EventTriggerType.PointerEnter;
        entry.callback.AddListener((function) => { PointerEnter(); });
        eventTrigger.triggers.Add(entry);


        btn =GetComponent<Button>();
        btn.onClick.AddListener(Click);
    }
    void PointerEnter()
    {
        AudioManager.instance.Play("PointerEnter");
    }
    void Click()
    {
        AudioManager.instance.Play("Click");
    }
}
