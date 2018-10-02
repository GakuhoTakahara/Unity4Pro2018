using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class Score : MonoBehaviour {

    // スコア表示のテキスト
    public Text scoreText;

    public Text highScoreText;

    // スコア
    private int score;

    // ハイスコア
    private int highScore;

    // PlayerPlefsでhighScoreを保存するときのキー
    private string highScoreKey = "highScore";

    // PlayerPlefsでRankingを保存するときのキー
    private string rankingPrefKey = "ranking";

    // PlayerPlefsでプレイ人数を保存するときのキー
    private string playCountKey = "playCount";

    // ランキングを所得する数(静的)
    private const int RANKING_NUM = 999;

    // ランキングを格納
    private int[] ranking = new int[RANKING_NUM];

    // ランキング表示のテキスト
    public Text rankingText;

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

        // すべてのキーをリセット
        if(Input.GetKey(KeyCode.T)&& Input.GetKey(KeyCode.P) && Input.GetKey(KeyCode.M))
        {
            AllResetKey();
        }
	}

    // ゲーム開始前の状態に戻す
    public void Initialize()
    {
        // スコアを0に戻す
        score = 0;

        // ハイスコアを取得する．保存されていないときは0を取得する．
        highScore = PlayerPrefs.GetInt(highScoreKey, 0);
    }

    // ポイントの追加
    public void AddPoint(int point)
    {
        score = score + point;
    }

    // ハイスコアの保存
    public void Sava()
    {
        //プレイ人数を増やす
        PlayerPrefs.SetInt(playCountKey, PlayerPrefs.GetInt(playCountKey, 0)+1);

        // ハイスコアを保存する
        PlayerPrefs.SetInt(highScoreKey,highScore);
        PlayerPrefs.Save();

        // ランキング関係
        GetRankling();
        SaveRanking(score);
        SetRanking();

        CheckRank(score);

       // ゲーム開始前の状態に戻す
       Initialize();
    }

    // ランキングの取得
    public void GetRankling()
    {
        var _ranking = PlayerPrefs.GetString(rankingPrefKey);
        if (_ranking.Length > 0)
        {
            var _score = _ranking.Split(","[0]);
            ranking = new int[RANKING_NUM];
            for (var i = 0; i < _score.Length && i < RANKING_NUM; i++)
            {
                ranking[i] = int.Parse(_score[i]);
            }
        }
    }

    // ランキングの保存
    public void SaveRanking(int newScore)
    {
        if (ranking.Length > 0)
        {
            int _tmp = 0;
            for (var i = 0; i < ranking.Length; i++)
            {
                if (ranking[i] < newScore)
                {
                    _tmp = ranking[i];
                    ranking[i] = newScore;
                    newScore = _tmp;
                }
            }
        }
        else
        {
            ranking[0] = newScore;
        }

        // 配列を文字列に変換して PlayerPrefs に格納
        var ranking_string = string.Join(",", ranking.Select(e => e.ToString()).ToArray());
        PlayerPrefs.SetString(rankingPrefKey, ranking_string);
        PlayerPrefs.Save();

    }

    // ランキングの表示
    public void SetRanking()
    {
        Debug.Log("CALLED SET RANKING");
        string ranking_string="";
        for (var i = 0; i < (ranking.Length)||(i<=10); i++)
        {
            ranking_string = ranking_string + (i + 1) + "  位" +"\t"+ ranking[i] +"\t"+ " point\n";
        }

        // ランキングを表示する
        rankingText.text = ranking_string.ToString();
    }

    public int CheckRank(int myVal)
    {
        // 順位
        int rank = 1;

  
        // はじめにランキングを取得
        var _ranking = PlayerPrefs.GetString(rankingPrefKey);
        if (_ranking.Length > 0)
        {
            var _score = _ranking.Split(","[0]);
            ranking = new int[RANKING_NUM];
            for (var i = 0; i < _score.Length; i++)
            {
                ranking[i] = int.Parse(_score[i]);
            }
        }

        for (int i = 0; ranking[i] > myVal; i++) rank++;
        Debug.Log("My Ranking:" + rank+"位 / "+ PlayerPrefs.GetInt(playCountKey)+" 人");
        return rank;
    }

    private void AllResetKey()
    {
        PlayerPrefs.DeleteKey(highScoreKey);
        PlayerPrefs.DeleteKey(playCountKey);
        PlayerPrefs.DeleteKey(rankingPrefKey);
        Debug.Log("### KEY IS ALL RESET ###");
    }
}

