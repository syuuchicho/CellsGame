using System.Collections;
using System.Collections.Generic;
using Unity.Rendering.HybridV2;
using UnityEngine;
using UnityEngine.UIElements;


public class EnemyScript : MonoBehaviour, IDamagAble
{
    public float gravity = -0.2f;

    public int EnemyHp = 60;
    public Rigidbody rb;
    public Material material;
    public Material material1;
    private Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        //animatorコンポーネントを取得              
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        //重力
        rb.AddForce(new Vector3(0, gravity, 0));

        //Hpが0以下のときに消滅
        Death();
    }

    private void Death()
    {
        if (EnemyHp <= 0) 
        {
            Destroy(this.gameObject);
            //画面を青く
            FlushController.instance.BlueScreen();
        }
    }

    void IDamagAble.AddDamage(int damage)
    {
        //クリティカルした場合、効果音
        EnemyHp -= damage;
        Debug.Log("攻撃が当たった！Hp-"+damage);
        Invoke("ResetMaterialColor", 0.3f);
        //画面を黄色
        FlushController.instance.YellowScreen();
    }
}
