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
        //player���U���͈͂ɂ�����
        if (other.CompareTag("Player"))
        {
            animator.SetBool("isLock", true);
            //�X�^����Ԃɓ�������
            if (isStun == true)
            {
                //���b�N�I��������
                animator.SetBool("isLock", false);
            }
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
