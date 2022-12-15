using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwimDetector : MonoBehaviour
{
    public GameObject noteNeedSwimSkill;
    private PlayerStates playerStates;
    public BoxCollider waterCollider;
    private void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.tag == "Player")
        {
            if(!playerStates.swimAble)
                noteNeedSwimSkill.SetActive(true);
            if (playerStates.swimAble)
                waterCollider.enabled = false;
        }
    }
    private void OnTriggerExit(Collider collider)
    {
        if (collider.gameObject.tag == "Player" && !playerStates.swimAble)
        {
            noteNeedSwimSkill.SetActive(false);
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        playerStates = PlayerManager.instance.player.GetComponent<PlayerStates>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
