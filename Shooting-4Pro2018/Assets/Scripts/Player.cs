using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour
{
    // Spaceshipコンポーネント
    Spaceship spaceship;
    int timeCount;

    // HP
    private int hp = 100;

    // Bulletの持ち数
    private int playerBullet;

    // Bulletの持ち数の最大数
    public int playerBulletMax = 100;

    // 何秒ごとに弾を補充するか
    public float addBulletTime = 5;

    // 弾をいくつ補充するか
    public int addBulletCount = 10;

    private void Start()
    {
        // Spaceshipコンポーネントを取得
        spaceship = GetComponent<Spaceship>();

        // 弾の持ち数を最大値にセット
        SetPlayerBullet(playerBulletMax);

        // コルーチンをセット
        StartCoroutine(AddPlayerBullet());
    }

    void ShotBullet()
    {
        
        int SHOT_INTERVAL = spaceship.shotInterva;

        if ((Input.GetKey(KeyCode.X))&&(GetPlayerBullet()>0))
        {
            timeCount++;
            //カウントが発射間隔に達したら、弾を発射
            if (timeCount > SHOT_INTERVAL)
            {
                timeCount = 0;  //カウント初期化
                // 弾をプレイヤーと同じ位置/角度で作成
                spaceship.Shot(transform);
                // ショット音を鳴らす
                GetComponent<AudioSource>().Play();

                // 弾の持ち数を減らす
                SetPlayerBullet(-1);
            }
        }
        else
        {
            // ボタンが押されていない場合、次弾用意
            timeCount = SHOT_INTERVAL;
        }
    }

    void Update()
    {
        // 右・左
        float x = Input.GetAxisRaw("Horizontal");

        // 上・下
        float y = Input.GetAxisRaw("Vertical");

        // 移動する向きを求める
        Vector2 direction = new Vector2(x, y).normalized;

        // 弾の発射
        ShotBullet();

        // 移動の制限
        Move(direction);

        

    }

    // 機体の移動
    void Move(Vector2 direction)
    {
        // 画面左下のワールド座標をビューポートから取得
        //Vector2 min = Camera.main.ViewportToWorldPoint(new Vector2(0, 0));
        Vector2 min = new Vector2(-1.766f, -3.0f);

        // 画面右上のワールド座標をビューポートから取得
        //Vector2 max = Camera.main.ViewportToWorldPoint(new Vector2(1, 1));
        Vector2 max = new Vector2(1.766f, 3.0f);

        // プレイヤーの座標を取得
        Vector2 pos = transform.position;

        Debug.Log("min" + min + "max:" + max);

        // 移動量を加える
        pos += direction * spaceship.speed * Time.deltaTime;

        // プレイヤーの位置が画面内に収まるように制限をかける
        pos.x = Mathf.Clamp(pos.x, min.x, max.x);
        pos.y = Mathf.Clamp(pos.y, min.y, max.y);

        // 制限をかけた値をプレイヤーの位置とする
        transform.position = pos;
    }

    // ぶつかった瞬間に呼び出される
    void OnTriggerEnter2D(Collider2D c)
    {
        // レイヤー名を取得
        string layerName = LayerMask.LayerToName(c.gameObject.layer);

        // レイヤー名がBullet (Enemy)の時は弾を削除
        if (layerName == "Bullet (Enemy)")
        {
            // 弾の削除
            Destroy(c.gameObject);
        }

        // レイヤー名がBullet (Enemy)またはEnemyの場合はHpを下げる
        if (layerName == "Bullet (Enemy)" || layerName == "Enemy")
        {
            switch (layerName)
            {
                case "Bullet (Enemy)":
                    SetHp(5);
                    break;

                case "Enemy":
                    SetHp(10);
                    break;
            }
        }
        
        // ゲームオーバー
        if (GetHp() == 0)
        {
            // Barの値を確実に書き換える
            FindObjectOfType<Manager>().SetHpBar();

            // Playerを削除
            Destroy(gameObject);

            // ゲームオーバーのフラグを立てる
            FindObjectOfType<Manager>().SetState("GameOvar");
        }
    }

    // HPを一定時間ごとに追加
    IEnumerator AddPlayerBullet()
    {
        while (true)
        {
            SetPlayerBullet(addBulletCount);
            yield return new WaitForSeconds(addBulletTime);
        }
    }

    // HPをセット
    public void SetHp(int val)
    {
        hp -= val;
        if (hp > 100) hp = 100;
        if (hp < 0) hp = 0;
    }

    // 現在のHPを取得
    public int GetHp()
    {
        return hp;
    }

    // 球数をセット
    public void SetPlayerBullet(int val)
    {
        playerBullet += val;
        if (playerBullet > playerBulletMax) playerBullet = playerBulletMax;
        else if (playerBullet < 0) playerBullet = 0;
    }

    // 残りの球数を取得
    public int GetPlayerBullet()
    {
        return playerBullet;
    }

    // 球数の最大値を取得
    public int GetPlayerBulletMax()
    {
        return playerBulletMax;
    }
}