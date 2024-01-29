using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KameraManager : MonoBehaviour
{
    public GameObject target;   //追従する対象を決める
    Vector3 pos;                //カメラの初期位置を記憶
    // Start is called before the first frame update
    void Start()
    {
        pos = Camera.main.gameObject.transform.position;//カメラの初期位置
    }

    // Update is called once per frame
    void Update()
    {
        //横軸だけ追従
        pos.x = target.transform.position.x;
        //縦軸追従
        pos.y = target.transform.position.y;
        pos.z = -15;//奥行き
        Camera.main.gameObject.transform.position = pos;
    }
}
