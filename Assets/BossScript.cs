using System.Collections;
using System.Collections.Generic;
using Unity.Rendering.HybridV2;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;

public class BossScript : MonoBehaviour, IDamagAble
{
    public Rigidbody rb;
    public Transform player;

    private Animator animator;
    private NavMeshAgent agent;

    public GameManager gameManager;

    public Renderer _renderer;
    private Material _materialBox;

    public int fireCD = 0;
    public GameObject leftShell;
    public GameObject rightShell;

    //倒したときのエフェクト
    public ParticleSystem breakEffect;

    //左手のコライダー
    public Collider handCollider;

    //インスペクターでHpを定義
    public int EnemyHp;
    public float gravity = -0.2f;
    public bool isStun = false;
    public bool isAttack = false;
    public bool fireAttack = false;
    public int stunCount = 120;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        //animatorコンポーネントを取得
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();

        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        //元の色をコピー
        _materialBox = new Material(_renderer.material);
    }

    // Update is called once per frame
    void Update()
    {
        FireAttack();
        //重力
        rb.AddForce(new Vector3(0, gravity, 0));
        //AI
        agent.SetDestination(player.position);
        //回転更新しない
        agent.updateRotation = false;
        //NavmeshAgentのスピードをAnimatorに登録
        animator.SetFloat("Speed", agent.velocity.sqrMagnitude);
        //目標との距離をAnimatorに登録
        animator.SetFloat("Distance", agent.remainingDistance);

        //ダメージを受けた後
        //元の色に回復
        _renderer.material.color = Color.Lerp(_renderer.material.color, _materialBox.color, Time.deltaTime * 10);

        //静止でない　かつ　攻撃中でなければ
        if (agent.velocity.sqrMagnitude != 0 && isAttack == false && isStun == false)
        {
            //プレイヤーの座標によって向きを変える
            TransToPlayer();
        }

        if (EnemyHp <= 0)
        {
            //Hpが0以下のときに消滅
            Death();
        }

        //値渡し
        //スタン
        animator.SetBool("isStun", isStun);
        //ファイアアタック
        animator.SetBool("fireAttack", fireAttack);

        //スタン
        if (isStun)
        {
            StopAgent();
            stunCount--;
        }
        //スタン終了
        if (stunCount <= 0)
        {
            isStun = false;
            stunCount = 120;
            TransToPlayer();
            StartAgent();
        }
    }

    private void Death()
    {
        //パーティクルを発生
        GenerateEffect();
        animator.SetBool("isDead", true);
        Destroy(this.gameObject);
        //画面を青く
        FlushController.instance.BlueScreen();
        if (tag == "Boss")
        {
            //演出を入れても可
            gameManager.ChangeScene("GameClear");
        }
    }
    private void OnDestroy()
    {
        if (_materialBox != null)
        {
            Destroy(_materialBox);
        }
    }

    public void FireAttack()
    {
        if (fireAttack == false)
        {
            fireCD++;
        }
        if (fireCD >= 120 && agent.remainingDistance >= 2)
        {
            fireAttack = true;
            fireCD = 0;
        }
    }

    //エフェクトを生成する
    void GenerateEffect()
    {
        //エフェクトのクローンを生成
        ParticleSystem p1 = Instantiate(breakEffect);
        //クローンを再生
        p1.Play();
        //エフェクトの座標は敵オブジェの座標
        p1.transform.position = gameObject.transform.position;
        //クローン再生終了後自動削除します
    }

    public void AddDamage(int damage)
    {
        GetHit();
        if (isStun)
        {
            EnemyHp -= damage * 2;
            //画面を黄色
            FlushController.instance.YellowScreen();
            //クリティカルした場合、効果音
        }
        else
        {
            EnemyHp -= damage;
        }
        Debug.Log("攻撃が当たった！Hp-" + damage);
    }


    //---------------アニメーションイベント用関数----------------//
    public void GetStun()
    {
        isStun = true;
    }
    public void ChangeColToR()
    {
        _renderer.material.color = Color.red;
    }
    public void GetHit()
    {
        animator.SetBool("getHit", true);
        //動きを止める
        StopAgent();
    }
    public void ResetHit()
    {
        animator.SetBool("getHit", false);
        StartAgent();
        TransToPlayer();
    }

    public void ColliderOn()
    {
        handCollider.enabled = true;
    }

    public void ColliderOff()
    {
        handCollider.enabled = false;
    }

    public void StopAgent()
    {
        //NavmeshAgentを一時停止
        agent.isStopped = true;
    }
    public void StartAgent()
    {
        if (isStun == false)
        {
            //NavmeshAgentを開始
            agent.isStopped = false;
        }
    }
    public void StartAttack()
    {
        isAttack = true;
    }
    public void StopAttack()
    {
        isAttack = false;
    }
    //プレイヤーの方に向きを変える
    public void TransToPlayer()
    {
        //プレイヤーの座標によって向きを変える
        if (player.position.x > rb.position.x)
        {
            //右向き
            transform.eulerAngles = new Vector3(0, 90, 0);
        }
        else if (player.position.x <= rb.position.x)
        {
            //左向き
            transform.eulerAngles = new Vector3(0, -90, 0);
        }
    }
    public void InstantiateFire()
    {
        Instantiate(leftShell, transform.position, Quaternion.identity);
        Instantiate(rightShell, transform.position, Quaternion.identity);
    }
    public void ResetFireAttack()
    {
        fireAttack = false;
    }
}
