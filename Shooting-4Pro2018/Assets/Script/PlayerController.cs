using UnityEngine;
using System.Collections;



public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private int SHOT_INTERVAL = 10;
    private int timeCount;
    public GameObject bulletPrefab;
    Spaceship spaceship;

    void Update()
    {
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            transform.Translate(-0.1f, 0, 0);
        }
        if (Input.GetKey(KeyCode.RightArrow))
        {
            transform.Translate(0.1f, 0, 0);
        }
        if (Input.GetKey(KeyCode.UpArrow))
        {
            transform.Translate(0, 0.1f, 0);
        }
        if (Input.GetKey(KeyCode.DownArrow))
        {
            transform.Translate(0, -0.1f, 0);
        }

        // 連射
        if (Input.GetKey(KeyCode.Space))
        {
            timeCount++;

            if (timeCount > SHOT_INTERVAL)
            {
                timeCount = 0;
                Instantiate(bulletPrefab, transform.position, Quaternion.identity);
                GameObject.Find("Canvas").GetComponent<UIController>().RemoveHP(1);
            }
        }
        else
        {
            timeCount = SHOT_INTERVAL;
        }
    }

    // ぶつかった瞬間に呼び出される
    void OnTriggerEnter2D(Collider2D c)
    {

        Debug.Log("HitPlayerCon");
        // レイヤー名を取得
        string layerName = LayerMask.LayerToName(c.gameObject.layer);

        // レイヤー名がBullet (Enemy)の時は弾を削除
        if (layerName == "Bullet (Enemy)")
        {
            // 弾の削除
            Destroy(c.gameObject);
        }

        // レイヤー名がBullet (Enemy)またはEnemyの場合は爆発
        if (layerName == "Bullet (Enemy)" || layerName == "Enemy")
        {
            // 爆発する
            spaceship.Explosion();

            // プレイヤーを削除
            Destroy(gameObject);
        }
    }
}