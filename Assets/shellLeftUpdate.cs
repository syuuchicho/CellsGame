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
        //弾のワールド座標
        Vector3 pos = transform.position;

        //左に飛ぶ
        pos.x -= 0.3f;
        //弾の移動
        transform.position = new Vector3(pos.x, pos.y, pos.z);

        //一定時間経ったら消滅する
        Destroy(this.gameObject, 3.0f);

    }
}
