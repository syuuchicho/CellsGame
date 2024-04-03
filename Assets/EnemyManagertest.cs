using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManagertest : MonoBehaviour
{
    public GameObject Enemy;

    public Transform EnemyPos;
    // Start is called before the first frame update
    void Start()
    {
        Instantiate(Enemy,EnemyPos.position,Quaternion.identity);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
