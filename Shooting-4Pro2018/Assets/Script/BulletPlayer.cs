using UnityEngine;
using System.Collections;

public class BulletPlayer : MonoBehaviour
{

    public GameObject explosionPrefab;

    // 攻撃力
    public int power = 1;

    void Update()
    {
        transform.Translate(0, 0.2f, 0);

        if (transform.position.y > 5)
        {
            Destroy(gameObject);
        }
    }

    void OnTriggerEnter2D(Collider2D coll)
    {
        switch (coll.tag)
        {
            case "Enemy":
                // 衝突したときにスコアを更新する
                GameObject.Find("Canvas").GetComponent<UIController>().AddScore();

                // 爆発エフェクトを生成する	
                GameObject effect = Instantiate(explosionPrefab, transform.position, Quaternion.identity) as GameObject;
                Destroy(effect, 1.0f);

                //Destroy(coll.gameObject);
                //Destroy(gameObject);
                break;

            case "EnemyBullet":

                Destroy(coll.gameObject);
                Destroy(gameObject);
                break;

        }
           
    }
}