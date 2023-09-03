using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
 
public class PlayerAttack : MonoBehaviour
{

    //オブジェクトと接触した瞬間に呼び出される
    void OnTriggerEnter(Collider other)
    {
        IDamagAble IDamagAble =other.gameObject.GetComponent<IDamagAble>();
        //当たった相手がEnemyの場合
        if (other.CompareTag("Enemy"))
        {
            int playerDamage = 30;
            //other.gameObject.GetComponent<PlayerController>().AddDamage(playerDamage);
            //other.gameObject.GetComponent<EnemyScript>().Damage(playerDamage);
            IDamagAble.AddDamage(playerDamage);
        }
    }
}