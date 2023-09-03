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
        //animator�R���|�[�l���g���擾              
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        //�d��
        rb.AddForce(new Vector3(0, gravity, 0));

        //Hp��0�ȉ��̂Ƃ��ɏ���
        Death();
    }

    private void Death()
    {
        if (EnemyHp <= 0) 
        {
            Destroy(this.gameObject);
            //��ʂ��
            FlushController.instance.BlueScreen();
        }
    }

    void IDamagAble.AddDamage(int damage)
    {
        //�N���e�B�J�������ꍇ�A���ʉ�
        EnemyHp -= damage;
        Debug.Log("�U�������������IHp-"+damage);
        Invoke("ResetMaterialColor", 0.3f);
        //��ʂ����F
        FlushController.instance.YellowScreen();
    }
}
