using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class groundCollider : MonoBehaviour
{
    private PlayerController controller;
    public string gameObjectTagName;
    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponentInParent<PlayerController>();
    }

    private void OnTriggerEnter(Collider other)
    {
        gameObjectTagName = other.gameObject.tag;
        if (gameObjectTagName == "Ground")
        {
            controller.isGround = true;
        }
    }

    //private void OnTriggerStay(Collider other)
    //{
    //    gameObjectTagName = other.gameObject.tag;
    //    if (gameObjectTagName == "Ground")
    //    {
    //        controller.isGround = true;
    //    }
    //}

    private void OnTriggerExit(Collider other)
    {
        gameObjectTagName = other.gameObject.tag;
        if (gameObjectTagName == "Ground")
        {
            controller.isGround = false;
        }
    }
}
