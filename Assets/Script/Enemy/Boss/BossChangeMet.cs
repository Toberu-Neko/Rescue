using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossChangeMet : MonoBehaviour
{
    [SerializeField]
    private Material normalMaterial;

    [SerializeField]
    private Material attackedMaterial;

    private Renderer currentRenderer;
    void Start()
    {
        currentRenderer = GetComponent<Renderer>();
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.O))
        {
            currentRenderer.sharedMaterial = attackedMaterial;
        }
    }
}
