using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Ending : MonoBehaviour {

    // エンディング中かどうか
    private bool nowState = false;

    // Endingオブジェクト
    public GameObject gameClear;
    public GameObject gameOvar;
    public GameObject gameResults;

    // ゲーム結果テキスト
    // スコア
    public Text resultsScoreText;
    // ランキング
    public Text resultsRankingText;

    // GameOvar画面を表示する時間
    public float gameOvarTime = 3;

	// Use this for initialization
	void Start () {
        // GameClear・Ovarを無効にする
        gameClear.SetActive(false);
        gameOvar.SetActive(false);
    }
	
	// Update is called once per frame
	void Update () {

        // エンディング中でなければ
        if (IsEnding() == false)
        {
            // ゲームの状態を取得
            switch (FindObjectOfType<Manager>().GetState())
            {
                // ゲーム進行中
                case "Playing":
                    break;


                // ゲームオーバー
                case "GameOvar":
                    // エンディング中にする
                    SetEndingState(true);

                    // ゲームオーバー演出の実行
                    GameOvar();
                    break;

                // ゲームクリア
                case "GameClear":
                    // エンディング中にする
                    SetEndingState(true);

                    // ゲームクリア演出の実行
                    GameClear();
                    break;
            }
        }

    }




    // ゲームオーバー
    private void GameOvar()
    {
        Debug.Log("Called GameOvar()");
        gameOvar.SetActive(true);
        ScreenRanking();
        StartCoroutine("GameResults");
    }


    // ゲームクリア
    private void GameClear()
    {

        gameClear.SetActive(true);
        Debug.Log("Called GameClear()");
    }

    // ゲーム結果を表示
    IEnumerator GameResults()
    {
        // Game Clear/Ovar 画面を無効化
        gameOvar.SetActive(false);
        gameClear.SetActive(false);

        // スコアを取得
        int _score = FindObjectOfType<Score>().GetLastScore();

        // ランキング表示用のテキストを作成
        int[] rankVal = FindObjectOfType<Score>().GetRank(_score);
        string rankText = rankVal[0].ToString() + " 位 / " + rankVal[1].ToString() + " 人";

        // ゲーム結果を表示
        gameResults.SetActive(true);

        // ゲーム結果の値をセット
        resultsScoreText.text =_score.ToString() + " point";
        resultsRankingText.text = rankText;

        yield return new WaitForSeconds(3.0f);
        FindObjectOfType<Manager>().GameFinish();
        gameResults.SetActive(false);

        FindObjectOfType<Manager>().SetState("Playing");
        SetEndingState(false);

    }

    // エンディング中かどうかセットする
    public void SetEndingState(bool state)
    {
       nowState = state;
    }

    // エンディング中かどうか
    public bool IsEnding()
    {
        return nowState;
    }

    // ランキングをセットする
    public void ScreenRanking()
    {

        FindObjectOfType<Score>().Sava();

    }
}
