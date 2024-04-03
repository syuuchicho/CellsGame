using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireAttackscpt : MonoBehaviour
{
    //インスペクターで攻撃力を定義
    public int Damage;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void OnTriggerEnter(Collider other)
    {
        //当たったオブジェクトのIDamageAbleを呼び出す
        IDamagAble IDamagAble = other.gameObject.GetComponent<IDamagAble>();
        PlayerController playerController = other.gameObject.GetComponent<PlayerController>();
        //当たった相手がPlayerの場合
        if (other.CompareTag("Player"))
        {
            playerController.ChangeColToR();
            IDamagAble.AddDamage(Damage);
        }
    }

}
