using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAttack : MonoBehaviour
{
    //�C���X�y�N�^�[�ōU���͂��`
    public int enemyDamage;
    public BossScript bossScript;
    private void Start()
    {
        //enemyDamage=bossScript.Damage;
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
                bossScript.GetStun();
            }
            else
            {
                playerController.ChangeColToR();
            }
            IDamagAble.AddDamage(enemyDamage);
            Debug.Log("�{�X�̍U�������������I");
        }
    }
}