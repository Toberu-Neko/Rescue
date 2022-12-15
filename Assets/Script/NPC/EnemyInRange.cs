using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyInRange : MonoBehaviour
{
    [SerializeField] private LayerMask enemiesLayer;
    [SerializeField] private Transform enemyDetector;
    [SerializeField] private float detectorRange;
    public bool enemyInRange;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Physics.CheckSphere(enemyDetector.position, detectorRange, enemiesLayer))
        {
            enemyInRange = true;
        }
        else
            enemyInRange = false;
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(enemyDetector.position, detectorRange);
    }
}
