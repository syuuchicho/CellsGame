using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class shellLeftUpdate : MonoBehaviour
{
    // Start is called before the first frame update
    private float y = 0.3f;
    private float posY;
    private Vector3 pos;
    void Start()
    {
        //�e�̃��[���h���W
        pos = transform.position;
        //���̍��W��菭����
        pos.y += y;
    }

    // Update is called once per frame
    void Update()
    {
        //���ɔ��
        pos.x -= 0.3f;
        //�e�̈ړ�
        transform.position = new Vector3(pos.x, pos.y, pos.z);

        //��莞�Ԍo��������ł���
        Destroy(this.gameObject, 3.0f);

    }
}
