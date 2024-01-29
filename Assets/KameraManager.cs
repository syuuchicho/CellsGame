using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KameraManager : MonoBehaviour
{
    public GameObject target;   //�Ǐ]����Ώۂ����߂�
    Vector3 pos;                //�J�����̏����ʒu���L��
    // Start is called before the first frame update
    void Start()
    {
        pos = Camera.main.gameObject.transform.position;//�J�����̏����ʒu
    }

    // Update is called once per frame
    void Update()
    {
        //���������Ǐ]
        pos.x = target.transform.position.x;
        //�c���Ǐ]
        pos.y = target.transform.position.y;
        pos.z = -15;//���s��
        Camera.main.gameObject.transform.position = pos;
    }
}
