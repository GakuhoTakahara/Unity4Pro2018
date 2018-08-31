using UnityEngine;

public class Bullet : MonoBehaviour
{
    public int speed = 10;

    void Start()
    {
        GetComponent<Rigidbody2D>().velocity = transform.up.normalized * speed;
    }

    // プレイヤーが障害物にぶつかったとき
    private void OnTriggerEnter2D(Collider2D collision)
    {
            // Bulletを削除
        Destroy(collision.gameObject);
        Destroy(this.gameObject);
    }
}