using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAttack : MonoBehaviour
{
    //インスペクターで攻撃力を定義
    public int enemyDamage;
    public BossScript bossScript;
    private void Start()
    {
        //enemyDamage=bossScript.Damage;
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
                bossScript.GetStun();
            }
            else
            {
                playerController.ChangeColToR();
            }
            IDamagAble.AddDamage(enemyDamage);
            Debug.Log("ボスの攻撃が当たった！");
        }
    }
}