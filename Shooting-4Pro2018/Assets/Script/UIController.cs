using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UIController : MonoBehaviour
{

    private int score = 0;
    private int hp = 100;
    GameObject scoreText;
    GameObject gameOverText;
    Slider hpbar;

    public void CheckGameOver()
    {
        if (this.hp == 0)
        {
            this.gameOverText.GetComponent<Text>().text = "GameOver";
        }
    }

    public void AddScore()
    {
        this.score += 10;
        CheckScore();
    }

    public void RemoveHP(int val=10)
    {
        this.hp -= val;
        if(hp<0)
        {
            hp = 0;
        }else if(hp>100)
        {
            hp = 100;
        }
    }

    public void AddHP(int val = 10)
    {
        this.hp += val;
        if (hp < 0)
        {
            hp = 0;
        }
        else if (hp > 100)
        {
            hp += 100;
        }
    }

    public int GetScore()
    {
        return score;
    }

    public void CheckScore()
    {
        if (this.score % 100 == 0) AddHP();
    }

    public float Map(float value, float start1, float stop1, float start2, float stop2)
    {
        return start2 + (stop2 - start2) * ((value - start1) / (stop1 - start1));
    }

    // Use this for initialization
    void Start()
    {
        this.scoreText = GameObject.Find("Score");
        this.gameOverText = GameObject.Find("GameOver");
        hpbar = GameObject.Find("HPBar").GetComponent<Slider>();
    }

    void Update()
    {
        scoreText.GetComponent<Text>().text = "Score:" + score.ToString("D4");
        hpbar.value = Map(hp, 0, 100, 0, 1);
        CheckGameOver();
        Debug.Log("HP：" + hp);
    }
}