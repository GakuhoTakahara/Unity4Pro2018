using UnityEngine;
using System.Collections;



public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private int SHOT_INTERVAL = 10;
    private int timeCount;
    public GameObject bulletPrefab;

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
}