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
        //player‚ªUŒ‚”ÍˆÍ‚É“ü‚Á‚½‚ç
        if (other.CompareTag("Player"))
        {
            animator.SetBool("isLock", true);
        }
    }

    void OnTriggerExit(Collider other)
    {
        //player‚ªUŒ‚”ÍˆÍ‚©‚ço‚½‚ç
        if (other.CompareTag("Player"))
        {
            animator.SetBool("isLock", false);
        }
    }
}
