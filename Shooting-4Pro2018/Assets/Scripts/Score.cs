using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Score : MonoBehaviour {

    // スコア表示のテキスト
    public Text scoreText;

    public Text highScoreText;

    // スコア
    private int score;

    // ハイスコア
    private int highScore;

    // PlayerPlefsで保存するときのキー
    private string highScoreKey = "highScore";

	// Use this for initialization
	void Start ()
    {
        Initialize();
	}
	
	// Update is called once per frame
	void Update ()
    {
        // スコアがハイスコアより大きいとき
        if (highScore < score) highScore = score;

        // スコア・ハイスコアの表示
        scoreText.text = score.ToString();
        highScoreText.text = highScore.ToString();
	}

    // ゲーム開始前の状態に戻す
    private void Initialize()
    {
        // スコアを0に戻す
        score = 0;

        // ハイスコアを取得する．保存されていないときは0を取得する．
        highScore = PlayerPrefs.GetInt(highScoreKey, 0);
    }

    // ポイントの追加
    public void addPoint(int point)
    {
        score = score + point;
    }

    // ハイスコアの保存
    public void Sava()
    {
        // ハイスコアを保存する
        PlayerPrefs.SetInt(highScoreKey,highScore);
        PlayerPrefs.Save();

        // ゲーム開始前の状態に戻す
        Initialize();
    }
}

