using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ending : MonoBehaviour {

    // エンディング中かどうか
    private bool nowState = false;

    // Endingオブジェクト
    public GameObject gameClear;
    public GameObject gameOvar;

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
        gameOvar.SetActive(true);
        ScreenRanking();
       Debug.Log("Called GameOvar()");
    }


    // ゲームクリア
    private void GameClear()
    {

        gameClear.SetActive(true);
        Debug.Log("Called GameClear()");
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
