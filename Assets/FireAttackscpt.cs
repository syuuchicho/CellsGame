using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireAttackscpt : MonoBehaviour
{
    //�C���X�y�N�^�[�ōU���͂��`
    public int Damage;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void OnTriggerEnter(Collider other)
    {
        //���������I�u�W�F�N�g��IDamageAble���Ăяo��
        IDamagAble IDamagAble = other.gameObject.GetComponent<IDamagAble>();
        PlayerController playerController = other.gameObject.GetComponent<PlayerController>();
        //�����������肪Player�̏ꍇ
        if (other.CompareTag("Player"))
        {
            playerController.ChangeColToR();
            IDamagAble.AddDamage(Damage);
        }
    }

}
