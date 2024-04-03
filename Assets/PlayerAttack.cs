using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    //�C���X�y�N�^�[�ōU���͂��`
    public int playerDamage;
    //�I�u�W�F�N�g�ƐڐG�����u�ԂɌĂяo�����
    void OnTriggerEnter(Collider other)
    {
        //���������I�u�W�F�N�g��IDamageAble���Ăяo��
        IDamagAble IDamagAble = other.gameObject.GetComponent<IDamagAble>();
        EnemyScript enemyScript = other.gameObject.GetComponent<EnemyScript>();
        BossScript bossScript = other.gameObject.GetComponent<BossScript>();
        //�����������肪Enemy�̏ꍇ
        if (other.CompareTag("Enemy"))
        {
            IDamagAble.AddDamage(playerDamage);
            //�_���[�W�\���@��
            enemyScript.ChangeColToR();
        }
        if (other.CompareTag("Boss"))
        {
            IDamagAble.AddDamage(playerDamage);
            //�_���[�W�\���@��
            bossScript.ChangeColToR();
        }
    }
}