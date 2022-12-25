using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpPad : MonoBehaviour
{
    private Rigidbody playerRig;
    [SerializeField] float upForce;
    [SerializeField] float renderDistance;
    private GameObject particle;
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            playerRig.velocity += new Vector3(0, upForce, 0);
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        particle = transform.Find("Trampoline Flood").gameObject;
        playerRig = PlayerManager.instance.player.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Vector3.Distance(playerRig.transform.position, transform.position) < renderDistance)
        {
            particle.SetActive(true);
        }
        else
        {
            particle.SetActive(false);
        }
    }
}
