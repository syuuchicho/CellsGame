using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    //�C���X�y�N�^�[�ōU���͂��`
    public int enemyDamage;
    public EnemyScript enemyScript;
    private void Start()
    {
        //enemyScript = GameObject.FindWithTag("Enemy").GetComponent<EnemyScript>();
    }
    // Start is called before the first frame update
    void OnTriggerEnter(Collider other)
    {
        //���������I�u�W�F�N�g��IDamageAble���Ăяo��
        IDamagAble IDamagAble = other.gameObject.GetComponent<IDamagAble>();
        PlayerController playerController = other.gameObject.GetComponent<PlayerController>();

        //�����������肪Player�̏ꍇ
        if (other.CompareTag("Player"))
        {
            //�p���C��
            if (playerController.isParry)
            {
                //�p���C���ꂽ����
                enemyScript.GetStun();
            }
            IDamagAble.AddDamage(enemyDamage);
            //enemyScript.StopAttack();
            Debug.Log("�G�̍U�������������I");
        }
    }
}
