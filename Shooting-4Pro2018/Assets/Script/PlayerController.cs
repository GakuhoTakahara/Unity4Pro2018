using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour
{

    public GameObject bulletPrefab;

    private int timeCount;

    void Update()
    {

        // 連射機構のため
        timeCount += 1;

        if (Input.GetKey(KeyCode.LeftArrow))
        {
            transform.Translate(-0.1f, 0, 0);
        }
        if (Input.GetKey(KeyCode.RightArrow))
        {
            transform.Translate(0.1f, 0, 0);
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            
        }
        if (Input.GetKey(KeyCode.Space))
        {
            // 連射
            if (timeCount % 10 == 0)
            {
                Instantiate(bulletPrefab, transform.position, Quaternion.identity);
                GameObject.Find("Canvas").GetComponent<UIController>().RemoveHP(2);
            }
        }
    }
}