using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoteManager : MonoBehaviour
{
    private Transform cam;

    private void Start()
    {
        GameObject _cam = GameObject.FindGameObjectWithTag("MainCamera");
        cam = _cam.GetComponent<Transform>();
    }
    void LateUpdate()
    {
        transform.LookAt(transform.position + cam.forward);
    }
}
