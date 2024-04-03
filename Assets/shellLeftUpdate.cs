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
        //弾のワールド座標
        pos = transform.position;
        //元の座標より少し上
        pos.y += y;
    }

    // Update is called once per frame
    void Update()
    {
        //左に飛ぶ
        pos.x -= 0.3f;
        //弾の移動
        transform.position = new Vector3(pos.x, pos.y, pos.z);

        //一定時間経ったら消滅する
        Destroy(this.gameObject, 3.0f);

    }
}
