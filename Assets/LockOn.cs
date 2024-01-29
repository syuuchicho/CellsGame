using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockOn : MonoBehaviour
{
    private Animator animator;
    // Start is called before the first frame update
    void Start()
    {
        animator = this.gameObject.GetComponent<Animator>();
    }
    void OnTriggerStay(Collider other)
    {
        //player���U���͈͂ɓ�������
        if (other.CompareTag("Player"))
        {
            animator.SetBool("isLock", true);
        }
    }

    void OnTriggerExit(Collider other)
    {
        //player���U���͈͂���o����
        if (other.CompareTag("Player"))
        {
            animator.SetBool("isLock", false);
        }
    }
}
