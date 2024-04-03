using System.Collections;
using System.Collections.Generic;
using Unity.Rendering.HybridV2;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;

public class EnemyScript : MonoBehaviour, IDamagAble
{
    public Rigidbody rb;
    public Transform player;

    private Animator animator;
    private NavMeshAgent agent;
    //索敵範囲
    private SphereCollider searchArea;
    public GameManager gameManager;

    public Renderer _renderer;
    public Renderer _renderer1;
    private Material _materialBox;
    private Material _materialBox1;
    //private int fireCount = 0;

    //倒したときのエフェクト
    public ParticleSystem breakEffect;

    //左手のコライダー
    public Collider handCollider;

    //インスペクターでHpを定義
    public int EnemyHp;
    public float gravity = -0.2f;
    public bool isStun = false;
    public bool isAttack = false;
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
        _materialBox1 = new Material(_renderer1.material);
    }

    // Update is called once per frame
    void Update()
    {
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

        //元の色に回復
        _renderer.material.color = Color.Lerp(_renderer.material.color, _materialBox.color, Time.deltaTime * 10);
        _renderer1.material.color = Color.Lerp(_renderer1.material.color, _materialBox1.color, Time.deltaTime * 10);

        //静止でない　かつ　攻撃中でなければ
        if (agent.velocity.sqrMagnitude != 0 && isAttack == false)
        {
            //プレイヤーの座標によって向きを変える
            TransToPlayer();
        }

        if (EnemyHp <= 0)
        {
            //Hpが0以下のときに消滅
            Death();
        }

        //値渡し スタン
        animator.SetBool("isStun", isStun);
        //値取得　
        // animator.GetBool()
        //スタン
        if (isStun)
        {
            StopAgent();
            stunCount--;
        }
        //スタン終了
        if (stunCount <= 0)
        {
            stunCount = 120;
            isStun = false;
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
        if (_materialBox1 != null)
        {
            Destroy(_materialBox1);
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

    public void GetStun()
    {
        isStun = true;
    }
    public void ChangeColToR()
    {
        _renderer.material.color = Color.red;
        _renderer1.material.color = Color.red;
    }
    //---------------アニメーションイベント用関数----------------//
    public void GetHit()
    {
        animator.SetBool("getHit", true);
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
}
