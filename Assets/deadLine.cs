using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class deadLine : MonoBehaviour
{
    public GameManager gameManager;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    private void OnTriggerEnter(Collider other)
    {
        gameManager.SceneReset();
    }
}
