using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    //インスペクターで攻撃力を定義
    public int enemyDamage;
    public EnemyScript enemyScript;
    private void Start()
    {
        //enemyScript = GameObject.FindWithTag("Enemy").GetComponent<EnemyScript>();
    }
    // Start is called before the first frame update
    void OnTriggerEnter(Collider other)
    {
        //当たったオブジェクトのIDamageAbleを呼び出す
        IDamagAble IDamagAble = other.gameObject.GetComponent<IDamagAble>();
        PlayerController playerController = other.gameObject.GetComponent<PlayerController>();

        //当たった相手がPlayerの場合
        if (other.CompareTag("Player"))
        {
            //パリイ中
            if (playerController.isParry)
            {
                //パリイされた処理
                enemyScript.GetStun();
            }
            IDamagAble.AddDamage(enemyDamage);
            //enemyScript.StopAttack();
            Debug.Log("敵の攻撃が当たった！");
        }
    }
}
