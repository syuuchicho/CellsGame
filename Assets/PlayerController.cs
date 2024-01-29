using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SocialPlatforms;

public class PlayerController : MonoBehaviour, IDamagAble
{
    public int Hp = 100;
    public const int maxHp = 100;
    public float power = 230;
    public float gravity = -0.1f;
    public float speed = 1;
    public bool isDoubleJump = false;
    public Rigidbody rb;
    public Animator animator;
    public Slider slider;
    public float velocity;

    public bool isGround = true;
    //ローリング中
    public bool isRolling = false;
    //パリイ中
    public bool isParry = false;
    //パリイフラグ
    public bool parryFlag = false;
    //ブロック中
    public bool isBlock = false;
    //攻撃アニメーション中
    private bool isAtk = false;
    //右手のコライダー
    private Collider handCollider;

    private CapsuleCollider capsuleCollider;
    //ローリングクールタイム
    private int rollingCD = 0;
    //パリィカウント
    private int parryCount = 26;
    //パリイ判定時間
    private int parryTime = 25;
    //ブロッククールタイム
    private int blockCD = 0;

    public Vector3 origin; // 原点
    public Vector3 direction; // Y軸方向を表すベクトル

    Ray ray;
    RaycastHit hit;

    // 辞書型の変数を
    Dictionary<string, bool> move = new Dictionary<string, bool>
    {
        {"up", false },
        {"down", false },
        {"right", false },
        {"left", false },
        {"jump",false },
    };

    // Start is called before the first frame update
    void Start()
    {
        //1920x1080ウインドウ
        Screen.SetResolution(1920, 1080, false);
        //60FPS固定
        Application.targetFrameRate = 60;
        //rigidBodyを取得
        rb = GetComponent<Rigidbody>();
        //animatorコンポーネントを取得              
        animator = GetComponent<Animator>();
        //カプセルコライダーを取得
        capsuleCollider = GetComponent<CapsuleCollider>();
        //右手のコライダーを取得
        handCollider = GameObject.Find("Player:RightHand").GetComponent<SphereCollider>();
    }

    // Update is called once per frame
    void Update()
    {
        move["up"] = Input.GetKey(KeyCode.W);
        move["down"] = Input.GetKey(KeyCode.S);
        move["right"] = Input.GetKey(KeyCode.D);
        move["left"] = Input.GetKey(KeyCode.A);

        origin = new Vector3(transform.position.x, transform.position.y + 0.1f, transform.position.z); // 原点
        //Vector3 direction = new Vector3(transform.position.x, transform.position.y - 0.1f, transform.position.z);
        ray = new Ray(origin, transform.up * -1); // Rayを下方向に生成;
        isGround = Physics.Raycast(ray, 0.12f); // もしRayを投射して何らかに衝突したら

        //レイ可視化(sceneでのみ)
        Debug.DrawRay(ray.origin, ray.direction, Color.red, 1.0f, false);

        //ジャンプアニメーション
        animator.SetBool("isGround", isGround);
        //重力
        if (isGround)//地面に立っている
        {
            gravity = -0.2f;
            rb.velocity = Vector3.zero;
            rb.useGravity = false;
        }
        else
        {
            rb.useGravity = true;
            if (gravity >= -10)
            { gravity += -0.5f; }
            rb.AddForce(new Vector3(0, gravity, 0));
        }

        //ジャンプ処理
        Jumping();

        //速度測定
        velocity = rb.velocity.y;
        animator.SetFloat("velocity", velocity);

        //値渡し
        animator.SetBool("isDoubleJump", isDoubleJump);

        //攻撃　左クリック
        if (Input.GetMouseButtonDown(0) == true && animator.GetBool("attack_flag") == false)
        {
            animator.SetBool("attack_flag", true);
        }

        //ローリングアクション
        Rolling();

        //ブロック処理
        Block();

        //Hpが0以下のときに消滅
        Death();

    }

    private void Jumping()
    {
        if (isGround)
        {
            isDoubleJump = false;
            if (Input.GetKeyDown(KeyCode.Space))//スペースを押した瞬間
            {
                //スペースキーを押す状況を渡す
                animator.SetTrigger("SpaceTri");

                //回避状態から抜ける
                ResetIsRolling();
                rb.velocity = Vector3.zero;
                //地面から離れる
                rb.AddForce(new Vector3(0, 1.0f * power, 0), ForceMode.Impulse);
            }
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.Space) && isDoubleJump == false)
            {
                //スペースキーを押す状況を渡す
                animator.SetTrigger("SpaceTri");

                //回避状態から抜ける
                ResetIsRolling();
                rb.velocity = Vector3.zero;
                //地面から離れる
                rb.AddForce(new Vector3(0, 1.0f * power, 0), ForceMode.Impulse);
                //二段ジャンプ中
                isDoubleJump = true;
            }
        }
    }

    private void Rolling()
    {
        //ローリングクールダウンがゼロなら
        if (rollingCD == 0)
        {
            if (Input.GetKeyDown(KeyCode.F) == true/* && isGround*/)
            {
                //ジャンプする力を消す
                rb.velocity = Vector3.zero;
                animator.SetBool("isRolling", true);
                //ブロックをキャンセル
                isBlock = false;
                //クールタイムを設ける
                rollingCD = 50;
            }
        }
        else
        {
            rollingCD--;
        }
        //ローリングできるかどうかを見る
        isRolling = animator.GetBool("isRolling");
        //ローリング処理
        if (isRolling && isAtk == false)
        {
            //ローリングアクション中,当たり判定を消す
            //プレイヤーの向く方向に進む
            rb.transform.position += gameObject.transform.localScale.z * new Vector3(0.11f * speed, 0, 0);
            //空中なら
            if (isGround == false)
            {
                //下方向に進む
                rb.transform.position += new Vector3(0, -0.02f * speed, 0);
            }
        }
    }

    private void Block()
    {
        //値渡し
        animator.SetBool("isBlocking", isBlock);
        //クールダウン
        if (blockCD != 0)
        {
            blockCD--;
        }
        if (blockCD <= 0)
        {
            //右クリック押した瞬間
            if (Input.GetMouseButtonDown(1) == true)
            {
                //カウントダウン初期化
                parryCount = 0;
                //パリイカウントダウンFlagOn
                parryFlag = true;
                //ブロック
                isBlock = true;
            }
            if (isBlock)
            {
                //右クリックを押し続けている
                if (Input.GetMouseButton(1) == true)
                {
                    //ブロックし続ける
                    isBlock = true;
                }
                //右クリックを離した
                if (Input.GetMouseButtonUp(1) == true)
                {
                    //ブロック解除
                    isBlock = false;
                    //ブロッククールダウンに入る
                    blockCD = 20;
                    //ガードアニメーションをぬける(Exit)
                }
            }
        }
        //パリイ期間
        if (parryFlag)
        {
            parryCount++;
        }
        if (parryCount <= parryTime)
        {
            isParry = true;
        }
        else
        {
            isParry = false;
        }
    }

    private void Death()
    {
        //Hpが0以下　死亡
        if (Hp <= 0)
        {
            Destroy(this.gameObject);
        }
    }

    public void AddDamage(int damage)
    {
        //パリイ判定
        //パリイ
        if (isParry)
        {
            Hp -= 0;//ダメージを受けない
            isBlock = false;
            blockCD = 30;
            FlushController.instance.WhiteScreen();
        }
        //ブロック
        else if (isBlock)
        {
            Hp -= damage / 2;
            FlushController.instance.RedScreen();
        }
        //攻撃を受けた
        else
        {
            Hp -= damage;
            //画面を赤
            FlushController.instance.RedScreen();
            Debug.Log("Player get hit！");
        }
        slider.value = (float)Hp / (float)maxHp;
    }
    public void GetStun()
    { }

    private void FixedUpdate()
    {
        // 入力がなければ0にする
        rb.angularVelocity = Vector3.zero;

        if (move["left"] == false && move["right"] == false)
        {
            animator.SetBool("running_flag", false);
        }
        //攻撃アニメーション中でない
        if (isAtk == false && isRolling == false && isBlock == false)
        {//移動処理

            if (move["left"])
            {
                rb.transform.position += (new Vector3(-0.1f * speed, 0, 0));

                //左向き
                rb.transform.localScale = new Vector3(1, 1, -1);
                animator.SetBool("running_flag", true);
            }

            if (move["right"])
            {
                rb.transform.position += (new Vector3(0.1f * speed, 0, 0));

                //右向き
                rb.transform.localScale = new Vector3(1, 1, 1);
                animator.SetBool("running_flag", true);
            }
        }
    }

    //---------------アニメーションイベント用関数----------------//
    public void HandColliderOn()
    {
        handCollider.enabled = true;
    }
    public void HandColliderOff()
    {
        handCollider.enabled = false;
    }
    public void LayerDefault()
    {
        //Layer:Default
        gameObject.SetLayerRecursively(0);
    }
    public void LayerRolling()
    {
        //Layer:rolling
        gameObject.SetLayerRecursively(6);
    }
    public void AtkAnimaIn()
    {
        isAtk = true;
    }
    public void AtkAnimaOut()
    {
        isAtk = false;
    }
    public void ResetAttackFlag()
    {
        animator.SetBool("attack_flag", false);
    }
    public void ResetIsRolling()
    {
        animator.SetBool("isRolling", false);
    }

}

