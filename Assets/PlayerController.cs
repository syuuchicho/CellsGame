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
    //���[�����O��
    public bool isRolling = false;
    //�p���C��
    public bool isParry = false;
    //�p���C�t���O
    public bool parryFlag = false;
    //�u���b�N��
    public bool isBlock = false;
    //�U���A�j���[�V������
    private bool isAtk = false;
    //�E��̃R���C�_�[
    private Collider handCollider;

    private CapsuleCollider capsuleCollider;
    //���[�����O�N�[���^�C��
    private int rollingCD = 0;
    //�p���B�J�E���g
    private int parryCount = 26;
    //�p���C���莞��
    private int parryTime = 25;
    //�u���b�N�N�[���^�C��
    private int blockCD = 0;

    public Vector3 origin; // ���_
    public Vector3 direction; // Y��������\���x�N�g��

    Ray ray;
    RaycastHit hit;

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
        //�J�v�Z���R���C�_�[���擾
        capsuleCollider = GetComponent<CapsuleCollider>();
        //�E��̃R���C�_�[���擾
        handCollider = GameObject.Find("Player:RightHand").GetComponent<SphereCollider>();
    }

    // Update is called once per frame
    void Update()
    {
        move["up"] = Input.GetKey(KeyCode.W);
        move["down"] = Input.GetKey(KeyCode.S);
        move["right"] = Input.GetKey(KeyCode.D);
        move["left"] = Input.GetKey(KeyCode.A);

        origin = new Vector3(transform.position.x, transform.position.y + 0.1f, transform.position.z); // ���_
        //Vector3 direction = new Vector3(transform.position.x, transform.position.y - 0.1f, transform.position.z);
        ray = new Ray(origin, transform.up * -1); // Ray���������ɐ���;
        isGround = Physics.Raycast(ray, 0.12f); // ����Ray�𓊎˂��ĉ��炩�ɏՓ˂�����

        //���C����(scene�ł̂�)
        Debug.DrawRay(ray.origin, ray.direction, Color.red, 1.0f, false);

        //�W�����v�A�j���[�V����
        animator.SetBool("isGround", isGround);
        //�d��
        if (isGround)//�n�ʂɗ����Ă���
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

        //�W�����v����
        Jumping();

        //���x����
        velocity = rb.velocity.y;
        animator.SetFloat("velocity", velocity);

        //�l�n��
        animator.SetBool("isDoubleJump", isDoubleJump);

        //�U���@���N���b�N
        if (Input.GetMouseButtonDown(0) == true && animator.GetBool("attack_flag") == false)
        {
            animator.SetBool("attack_flag", true);
        }

        //���[�����O�A�N�V����
        Rolling();

        //�u���b�N����
        Block();

        //Hp��0�ȉ��̂Ƃ��ɏ���
        Death();

    }

    private void Jumping()
    {
        if (isGround)
        {
            isDoubleJump = false;
            if (Input.GetKeyDown(KeyCode.Space))//�X�y�[�X���������u��
            {
                //�X�y�[�X�L�[�������󋵂�n��
                animator.SetTrigger("SpaceTri");

                //�����Ԃ��甲����
                ResetIsRolling();
                rb.velocity = Vector3.zero;
                //�n�ʂ��痣���
                rb.AddForce(new Vector3(0, 1.0f * power, 0), ForceMode.Impulse);
            }
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.Space) && isDoubleJump == false)
            {
                //�X�y�[�X�L�[�������󋵂�n��
                animator.SetTrigger("SpaceTri");

                //�����Ԃ��甲����
                ResetIsRolling();
                rb.velocity = Vector3.zero;
                //�n�ʂ��痣���
                rb.AddForce(new Vector3(0, 1.0f * power, 0), ForceMode.Impulse);
                //��i�W�����v��
                isDoubleJump = true;
            }
        }
    }

    private void Rolling()
    {
        //���[�����O�N�[���_�E�����[���Ȃ�
        if (rollingCD == 0)
        {
            if (Input.GetKeyDown(KeyCode.F) == true/* && isGround*/)
            {
                //�W�����v����͂�����
                rb.velocity = Vector3.zero;
                animator.SetBool("isRolling", true);
                //�u���b�N���L�����Z��
                isBlock = false;
                //�N�[���^�C����݂���
                rollingCD = 50;
            }
        }
        else
        {
            rollingCD--;
        }
        //���[�����O�ł��邩�ǂ���������
        isRolling = animator.GetBool("isRolling");
        //���[�����O����
        if (isRolling && isAtk == false)
        {
            //���[�����O�A�N�V������,�����蔻�������
            //�v���C���[�̌��������ɐi��
            rb.transform.position += gameObject.transform.localScale.z * new Vector3(0.11f * speed, 0, 0);
            //�󒆂Ȃ�
            if (isGround == false)
            {
                //�������ɐi��
                rb.transform.position += new Vector3(0, -0.02f * speed, 0);
            }
        }
    }

    private void Block()
    {
        //�l�n��
        animator.SetBool("isBlocking", isBlock);
        //�N�[���_�E��
        if (blockCD != 0)
        {
            blockCD--;
        }
        if (blockCD <= 0)
        {
            //�E�N���b�N�������u��
            if (Input.GetMouseButtonDown(1) == true)
            {
                //�J�E���g�_�E��������
                parryCount = 0;
                //�p���C�J�E���g�_�E��FlagOn
                parryFlag = true;
                //�u���b�N
                isBlock = true;
            }
            if (isBlock)
            {
                //�E�N���b�N�����������Ă���
                if (Input.GetMouseButton(1) == true)
                {
                    //�u���b�N��������
                    isBlock = true;
                }
                //�E�N���b�N�𗣂���
                if (Input.GetMouseButtonUp(1) == true)
                {
                    //�u���b�N����
                    isBlock = false;
                    //�u���b�N�N�[���_�E���ɓ���
                    blockCD = 20;
                    //�K�[�h�A�j���[�V�������ʂ���(Exit)
                }
            }
        }
        //�p���C����
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
        //Hp��0�ȉ��@���S
        if (Hp <= 0)
        {
            Destroy(this.gameObject);
        }
    }

    public void AddDamage(int damage)
    {
        //�p���C����
        //�p���C
        if (isParry)
        {
            Hp -= 0;//�_���[�W���󂯂Ȃ�
            isBlock = false;
            blockCD = 30;
            FlushController.instance.WhiteScreen();
        }
        //�u���b�N
        else if (isBlock)
        {
            Hp -= damage / 2;
            FlushController.instance.RedScreen();
        }
        //�U�����󂯂�
        else
        {
            Hp -= damage;
            //��ʂ��
            FlushController.instance.RedScreen();
            Debug.Log("Player get hit�I");
        }
        slider.value = (float)Hp / (float)maxHp;
    }
    public void GetStun()
    { }

    private void FixedUpdate()
    {
        // ���͂��Ȃ����0�ɂ���
        rb.angularVelocity = Vector3.zero;

        if (move["left"] == false && move["right"] == false)
        {
            animator.SetBool("running_flag", false);
        }
        //�U���A�j���[�V�������łȂ�
        if (isAtk == false && isRolling == false && isBlock == false)
        {//�ړ�����

            if (move["left"])
            {
                rb.transform.position += (new Vector3(-0.1f * speed, 0, 0));

                //������
                rb.transform.localScale = new Vector3(1, 1, -1);
                animator.SetBool("running_flag", true);
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

    //---------------�A�j���[�V�����C�x���g�p�֐�----------------//
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

