using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
 
public class PlayerAttack : MonoBehaviour
{

    //�I�u�W�F�N�g�ƐڐG�����u�ԂɌĂяo�����
    void OnTriggerEnter(Collider other)
    {
        IDamagAble IDamagAble =other.gameObject.GetComponent<IDamagAble>();
        //�����������肪Enemy�̏ꍇ
        if (other.CompareTag("Enemy"))
        {
            int playerDamage = 30;
            //other.gameObject.GetComponent<PlayerController>().AddDamage(playerDamage);
            //other.gameObject.GetComponent<EnemyScript>().Damage(playerDamage);
            IDamagAble.AddDamage(playerDamage);
        }
    }
}