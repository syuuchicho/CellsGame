using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class shellLeftUpdate : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //�e�̃��[���h���W
        Vector3 pos = transform.position;

        //���ɔ��
        pos.x -= 0.3f;
        //�e�̈ړ�
        transform.position = new Vector3(pos.x, pos.y, pos.z);

        //��莞�Ԍo��������ł���
        Destroy(this.gameObject, 3.0f);

    }
}
