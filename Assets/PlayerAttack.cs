using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
 
public class PlayerAttack : MonoBehaviour
{
    //インスペクターで攻撃力を定義
    public int playerDamage;
    //オブジェクトと接触した瞬間に呼び出される
    void OnTriggerEnter(Collider other)
    {
        //当たったオブジェクトのIDamageAbleを呼び出す
        IDamagAble IDamagAble =other.gameObject.GetComponent<IDamagAble>();
        EnemyScript enemyScript = other.gameObject.GetComponent<EnemyScript>();

        //当たった相手がEnemyの場合
        if (other.CompareTag("Enemy")|| other.CompareTag("Boss"))
        {
            IDamagAble.AddDamage(playerDamage);
        }
    }
}