using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour{
    public int Hp = 100;
    public float power = 200;
    public float gravity = -0.2f;
    public float speed = 1;
    public int jumpNum = 2;
    public Rigidbody rb;
    public Animator animator;

    
    //private bool isJump = false;
    //private bool isGround = false;
    //右手のコライダー
    private Collider handCollider;
    
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
        //右手のコライダーを取得
        handCollider = GameObject.Find("mixamorig:RightHand").GetComponent<SphereCollider>();
    }

    // Update is called once per frame
    void Update()
    {
        move["up"] = Input.GetKey(KeyCode.W);
        move["down"] = Input.GetKey(KeyCode.S);
        move["right"] = Input.GetKey(KeyCode.D);
        move["left"] = Input.GetKey(KeyCode.A);

        //ジャンプ処理
        if (Input.GetKeyDown(KeyCode.Space))//スペースを押した瞬間
        {
            if (jumpNum > 0)
            {
                jumpNum--;
                rb.velocity = Vector3.zero;
                rb.angularVelocity = Vector3.zero;
                rb.AddForce(new Vector3(0, 1.0f * power, 0));
            }
        }
        //重力
        rb.AddForce(new Vector3(0, gravity, 0));
        //攻撃　左クリック
        if (Input.GetMouseButtonDown(0) == true&& animator.GetBool("attack_flag")==false)
        {
            animator.SetBool("attack_flag",true);
        }
       
    }
    public void ColliderOn()
    {
        //攻撃の効果音を鳴らす(実装予定)

        handCollider.enabled = true;
    }

    public void ColliderOFF()
    {
        handCollider.enabled = false;
        animator.SetBool("attack_flag", false);
    }

    private void FixedUpdate()
    {
        // 入力がなければ0にする
        rb.angularVelocity = Vector3.zero;

        if (move["left"] == false && move["right"] == false)
        {
            animator.SetBool("running_flag", false);
        }
        //攻撃アニメーション中でない
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Punching_right") == false)
        {//移動処理
            if (move["up"])
            {
                rb.transform.position += (new Vector3(0, 0, 0.1f * speed));
            }

            if (move["left"])
            {
                rb.transform.position += (new Vector3(-0.1f *speed, 0, 0));

                //左向き
                rb.transform.localScale = new Vector3(1, 1, -1);
                animator.SetBool("running_flag", true);
            }

            if (move["down"])
            {
                rb.transform.position += (new Vector3(0, 0, -0.1f * speed));
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

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            //ジャンプリセット
            jumpNum = 2;
        }
    }
}

