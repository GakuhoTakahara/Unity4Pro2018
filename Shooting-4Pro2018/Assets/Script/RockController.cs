using UnityEngine;
using System.Collections;

public class RockController : MonoBehaviour
{

    float fallSpeed;
    float rotSpeed;

    void Start()
    {
        this.fallSpeed = 0.01f + 0.1f * Random.value;
        this.rotSpeed = 5f + 3f * Random.value;
    }

    void Update()
    {
        transform.Translate(0, -fallSpeed, 0, Space.World);
        transform.Rotate(0, 0, rotSpeed);
    }

    // プレイヤーが障害物にぶつかったとき
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            // HPを減らす
            GameObject.Find("Canvas").GetComponent<UIController>().RemoveHP();

            // Rockを削除
            Destroy(this.gameObject);
        }
    }

}