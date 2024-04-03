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
    //���G�͈�
    private SphereCollider searchArea;
    public GameManager gameManager;

    public Renderer _renderer;
    public Renderer _renderer1;
    private Material _materialBox;
    private Material _materialBox1;
    //private int fireCount = 0;

    //�|�����Ƃ��̃G�t�F�N�g
    public ParticleSystem breakEffect;

    //����̃R���C�_�[
    public Collider handCollider;

    //�C���X�y�N�^�[��Hp���`
    public int EnemyHp;
    public float gravity = -0.2f;
    public bool isStun = false;
    public bool isAttack = false;
    public int stunCount = 120;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        //animator�R���|�[�l���g���擾
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();

        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        //���̐F���R�s�[
        _materialBox = new Material(_renderer.material);
        _materialBox1 = new Material(_renderer1.material);
    }

    // Update is called once per frame
    void Update()
    {
        //�d��
        rb.AddForce(new Vector3(0, gravity, 0));
        //AI
        agent.SetDestination(player.position);
        //��]�X�V���Ȃ�
        agent.updateRotation = false;
        //NavmeshAgent�̃X�s�[�h��Animator�ɓo�^
        animator.SetFloat("Speed", agent.velocity.sqrMagnitude);
        //�ڕW�Ƃ̋�����Animator�ɓo�^
        animator.SetFloat("Distance", agent.remainingDistance);

        //���̐F�ɉ�
        _renderer.material.color = Color.Lerp(_renderer.material.color, _materialBox.color, Time.deltaTime * 10);
        _renderer1.material.color = Color.Lerp(_renderer1.material.color, _materialBox1.color, Time.deltaTime * 10);

        //�Î~�łȂ��@���@�U�����łȂ����
        if (agent.velocity.sqrMagnitude != 0 && isAttack == false)
        {
            //�v���C���[�̍��W�ɂ���Č�����ς���
            TransToPlayer();
        }

        if (EnemyHp <= 0)
        {
            //Hp��0�ȉ��̂Ƃ��ɏ���
            Death();
        }

        //�l�n�� �X�^��
        animator.SetBool("isStun", isStun);
        //�l�擾�@
        // animator.GetBool()
        //�X�^��
        if (isStun)
        {
            StopAgent();
            stunCount--;
        }
        //�X�^���I��
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
        //�p�[�e�B�N���𔭐�
        GenerateEffect();
        animator.SetBool("isDead", true);
        Destroy(this.gameObject);
        //��ʂ��
        FlushController.instance.BlueScreen();
        if (tag == "Boss")
        {
            //���o�����Ă���
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

    //�G�t�F�N�g�𐶐�����
    void GenerateEffect()
    {
        //�G�t�F�N�g�̃N���[���𐶐�
        ParticleSystem p1 = Instantiate(breakEffect);
        //�N���[�����Đ�
        p1.Play();
        //�G�t�F�N�g�̍��W�͓G�I�u�W�F�̍��W
        p1.transform.position = gameObject.transform.position;
        //�N���[���Đ��I���㎩���폜���܂�
    }

    public void AddDamage(int damage)
    {
        GetHit();
        if (isStun)
        {
            EnemyHp -= damage * 2;
            //��ʂ����F
            FlushController.instance.YellowScreen();
            //�N���e�B�J�������ꍇ�A���ʉ�
        }
        else
        {
            EnemyHp -= damage;
        }
        Debug.Log("�U�������������IHp-" + damage);
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
    //---------------�A�j���[�V�����C�x���g�p�֐�----------------//
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
        //NavmeshAgent���ꎞ��~
        agent.isStopped = true;
    }
    public void StartAgent()
    {
        if (isStun == false)
        {
            //NavmeshAgent���J�n
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
    //�v���C���[�̕��Ɍ�����ς���
    public void TransToPlayer()
    {
        //�v���C���[�̍��W�ɂ���Č�����ς���
        if (player.position.x > rb.position.x)
        {
            //�E����
            transform.eulerAngles = new Vector3(0, 90, 0);
        }
        else if (player.position.x <= rb.position.x)
        {
            //������
            transform.eulerAngles = new Vector3(0, -90, 0);
        }
    }
}
