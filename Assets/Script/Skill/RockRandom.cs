using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockRandom : MonoBehaviour
{
    void Start()
    {
        transform.localScale = transform.localScale * Random.Range(0.9f, 1.1f);
        transform.eulerAngles = new Vector3(Random.Range(-180f, 180f), Random.Range(-180f, 180f), Random.Range(-180f, 180f));
    }

    void Update()
    {
        
    }
}
