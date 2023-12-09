using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    private Vector3 spawnPos;
    public GameObject enemy;
    public GameObject currentEnemy;
    // Start is called before the first frame update
    public void Spawn()
    {
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }
        currentEnemy = Instantiate(enemy, transform, false);
        currentEnemy.transform.rotation = Quaternion.Euler(new Vector3(0f,Random.Range(0,359),0f));   
    }
}
