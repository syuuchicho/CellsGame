using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class shellRightUpdate : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        ////エフェクトのクローンを生成
        //ParticleSystem p1 = Instantiate(particle) as ParticleSystem;
        ////クローンを再生
        //p1.Play();
        ////エフェクトの座標は敵オブジェの座標
        //p1.transform.position = gameObject.transform.position;
        
    }

    // Update is called once per frame
    void Update()
    {
        //弾のワールド座標
        Vector3 pos = transform.position;

        //右に飛ぶ
        pos.x += 0.3f;
        //弾の移動
        transform.position = new Vector3(pos.x, pos.y, pos.z);

        //一定時間経ったら消滅する
        Destroy(this.gameObject, 3.0f); 
    }
}
