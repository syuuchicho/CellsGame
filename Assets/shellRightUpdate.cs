using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class shellRightUpdate : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        ////�G�t�F�N�g�̃N���[���𐶐�
        //ParticleSystem p1 = Instantiate(particle) as ParticleSystem;
        ////�N���[�����Đ�
        //p1.Play();
        ////�G�t�F�N�g�̍��W�͓G�I�u�W�F�̍��W
        //p1.transform.position = gameObject.transform.position;
        
    }

    // Update is called once per frame
    void Update()
    {
        //�e�̃��[���h���W
        Vector3 pos = transform.position;

        //�E�ɔ��
        pos.x += 0.3f;
        //�e�̈ړ�
        transform.position = new Vector3(pos.x, pos.y, pos.z);

        //��莞�Ԍo��������ł���
        Destroy(this.gameObject, 3.0f); 
    }
}
