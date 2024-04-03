using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockOn : MonoBehaviour
{
    private Animator animator;
    private bool isStun;
    // Start is called before the first frame update
    void Start()
    {
        animator = this.gameObject.GetComponent<Animator>();
    }

    void Update()
    {
        isStun = animator.GetBool("isStun");
    }

    void OnTriggerStay(Collider other)
    {
        //playerが攻撃範囲にいたら
        if (other.CompareTag("Player"))
        {
            animator.SetBool("isLock", true);
            //スタン状態に入ったら
            if (isStun == true)
            {
                //ロックオンを解除
                animator.SetBool("isLock", false);
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        //playerが攻撃範囲から出たら
        if (other.CompareTag("Player"))
        {
            animator.SetBool("isLock", false);
        }
    }
}
