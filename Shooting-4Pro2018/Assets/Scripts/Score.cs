using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class Score : MonoBehaviour {

    // スコア表示のテキスト
    public Text scoreText;

    public Text highScoreText;

    // ManagerObject
    public GameObject manager;

    // RankingのUsarName
    public Image[] rankingUserName=new Image[5];

    // スコア
    private int score;

    // ハイスコア
    private int highScore;

    // 最後のスコア
    private int lastScore;

    // 最後にセットしたスコア
    private static int lastSetScore = -2;

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

    //  ユーザーデータとスコアを保存
    private int[] userScore = new int[RANKING_NUM];

    private int[] latestRanking = new int[5];

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
        if(Input.GetKey(KeyCode.A)&& Input.GetKey(KeyCode.L) && Input.GetKey(KeyCode.R))
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
        // 最後のランキングをセットする
        lastScore = score;

        //プレイ人数を増やす
        PlayerPrefs.SetInt(playCountKey, PlayerPrefs.GetInt(playCountKey, 0)+1);

        // ハイスコアを保存する
        PlayerPrefs.SetInt(highScoreKey,highScore);
        PlayerPrefs.Save();

        // ランキング関係
        GetRankling();
        SaveRanking(score);
        SetRanking();

        // ランキングに画像を表示
        SetUserData(score);
        SetRankingNameImage();

        GetRank(score);

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
        for (var i = 0; i < 5; i++)
        {
            ranking_string = ranking_string+ranking[i] +"\n";

            latestRanking[i] = ranking[i];
            Debug.Log("Set latest Ranking : " + latestRanking[i]);
        }
        

        // ランキングを表示する
        rankingText.text = ranking_string;
    }


    // ランキングを取得
    public int[] GetRank(int myVal)
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

        Debug.Log("My Ranking:" + rank + "位 / " + PlayerPrefs.GetInt(playCountKey) + " 人");

        string rankText = rank + "位 / " + PlayerPrefs.GetInt(playCountKey) + " 人";

        // 戻り値用の配列
        int[] rankAndCount = new int[] { rank, PlayerPrefs.GetInt(playCountKey) };

        return rankAndCount;

    }

    // ユーザーデータを保存
    public void SetUserData(int newScore)
    {
        var userId = manager.GetComponent<Manager>().GetPlayedId();
        userScore[userId] = newScore;
        Debug.Log("Set User Score : " + userScore[userId]);

    }

    // RankingにUserNameImageを表示
    public void SetRankingNameImage()
    {
        // userScoreを複製
        var _userScore = userScore;

        

        for(int i=4; i>=0; i--)
        {
        
            int match = -1;
            for (int j = 0; j < RANKING_NUM; j++)
            {
                if (latestRanking[i] == _userScore[j])
                {
                    _userScore[j] = -1;
                    match = j;
                }
                lastSetScore = _userScore[j];
            }

            Debug.Log("SetRankingName" + match);
            if (match != -1)
            {
                string nameImgStr = "Name/" + match.ToString();
                Sprite nameImg = Resources.Load<Sprite>(nameImgStr);
                rankingUserName[i].sprite = nameImg;
            }
        }
    }

    // スコアを取得
    public int GetLastScore()
    {
        return lastScore;
    }

    private void AllResetKey()
    {
        PlayerPrefs.DeleteKey(highScoreKey);
        PlayerPrefs.DeleteKey(playCountKey);
        PlayerPrefs.DeleteKey(rankingPrefKey);
        PlayerPrefs.DeleteKey("playedId");
        Debug.Log("### KEY IS ALL RESET ###");
    }
}

