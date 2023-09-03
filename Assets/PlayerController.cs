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
    //�E��̃R���C�_�[
    private Collider handCollider;
    
    // �����^�̕ϐ���
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
        //1920x1080�E�C���h�E
        Screen.SetResolution(1920, 1080, false);
        //60FPS�Œ�
        Application.targetFrameRate = 60;
        //rigidBody���擾
        rb = GetComponent<Rigidbody>();
        //animator�R���|�[�l���g���擾              
        animator = GetComponent<Animator>();
        //�E��̃R���C�_�[���擾
        handCollider = GameObject.Find("mixamorig:RightHand").GetComponent<SphereCollider>();
    }

    // Update is called once per frame
    void Update()
    {
        move["up"] = Input.GetKey(KeyCode.W);
        move["down"] = Input.GetKey(KeyCode.S);
        move["right"] = Input.GetKey(KeyCode.D);
        move["left"] = Input.GetKey(KeyCode.A);

        //�W�����v����
        if (Input.GetKeyDown(KeyCode.Space))//�X�y�[�X���������u��
        {
            if (jumpNum > 0)
            {
                jumpNum--;
                rb.velocity = Vector3.zero;
                rb.angularVelocity = Vector3.zero;
                rb.AddForce(new Vector3(0, 1.0f * power, 0));
            }
        }
        //�d��
        rb.AddForce(new Vector3(0, gravity, 0));
        //�U���@���N���b�N
        if (Input.GetMouseButtonDown(0) == true&& animator.GetBool("attack_flag")==false)
        {
            animator.SetBool("attack_flag",true);
        }
       
    }
    public void ColliderOn()
    {
        //�U���̌��ʉ���炷(�����\��)

        handCollider.enabled = true;
    }

    public void ColliderOFF()
    {
        handCollider.enabled = false;
        animator.SetBool("attack_flag", false);
    }

    private void FixedUpdate()
    {
        // ���͂��Ȃ����0�ɂ���
        rb.angularVelocity = Vector3.zero;

        if (move["left"] == false && move["right"] == false)
        {
            animator.SetBool("running_flag", false);
        }
        //�U���A�j���[�V�������łȂ�
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Punching_right") == false)
        {//�ړ�����
            if (move["up"])
            {
                rb.transform.position += (new Vector3(0, 0, 0.1f * speed));
            }

            if (move["left"])
            {
                rb.transform.position += (new Vector3(-0.1f *speed, 0, 0));

                //������
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

                //�E����
                rb.transform.localScale = new Vector3(1, 1, 1);
                animator.SetBool("running_flag", true);
            }
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            //�W�����v���Z�b�g
            jumpNum = 2;
        }
    }
}

