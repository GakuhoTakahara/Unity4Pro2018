﻿using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;


public class Manager : MonoBehaviour
{
    // Playerプレハブ
    public GameObject player;

    // Playing Playerプレハブ
    public GameObject playing;

    public GameObject eventSysytem;

    // NextPlayer表示関係
    public GameObject titleNextPlayer;
    public GameObject titleText;

    // カウントダウンText
    public Text textStartCountdown;

    // カウントダウンGameObject
    public GameObject startCountdown;

    // Playerスクリプト
    Player playerScript;

    // タイトル
    private GameObject title;

    // HpBar
    public SimpleHealthBar hpBar;

    // BulletBar
    public SimpleHealthBar bulletBar;

    // ゲーム終了時ゲームオーバーかクリアか
    private string GameState = "Playing";

    // キー操作を受け付けるか
    private bool canKeyInput=true;

    // NameImage
    public Image nameImage;

    // PlayerImage
    public Image playerImage;


    // プレイ中のIDを入れる
    private int playingId;

    // プレイヤーしたIDを保存するPlayerPrefKey
    private string playedIdKey = "playedId";


    void Start()
    {
        // Titleゲームオブジェクトを検索し取得する
        title = GameObject.Find("Title");

        // IDをセット
        playingId = GetPlayedId();
        //if (playingId == -1) playingId = 0;
        //SetPlayedId();

        // タイトルにNextPlayerをセット
        //SetTitleImage(playingId);

        // ファイル監視を起動
        StartCoroutine(PlayerForderMonitor());
    }

    void Update()
    {
        // ゲーム中ではなく、Xキーが押されたらtrueを返す。
        if (IsPlaying() == false &&
            GetCanKeyInput()==true &&
            Input.GetKeyDown(KeyCode.X))
        {
            StartCoroutine("GameStart");
            //GameStart();
        }

        // Play中ならBarをセット
        if (IsPlaying() == true)
        {
            SetHpBar();
            SetBulletBar();
            //if (playerScript.GetHp() == 0) GameOver();
        }
    }

    IEnumerator GameStart()
    {

        // キー操作を無効にする
        SetCanKeyInput(false);

        // スコアを初期化する
        FindObjectOfType<Score>().Initialize();

        Destroy(playing);
        Destroy(playerScript);

        // Titleを非表示にする
        title.SetActive(false);
        startCountdown.SetActive(true);

        // カウントダウン
        textStartCountdown.text = "3";
        yield return new WaitForSeconds(1.0f);

        textStartCountdown.text = "2";
        yield return new WaitForSeconds(1.0f);

        textStartCountdown.text = "1";
        yield return new WaitForSeconds(1.0f);

        textStartCountdown.text = "Start";
        yield return new WaitForSeconds(0.5f);

        startCountdown.SetActive(false);


        // ゲームスタート時に、プレイヤーを作成する
        Instantiate(player, player.transform.position, player.transform.rotation);
        playing = GameObject.Find("Player(Clone)");
        playerScript = playing.GetComponent<Player>();
        SetPlayerImage(playingId);
        StopCoroutine(PlayerForderMonitor());
        Debug.Log("Playing ID : " + playingId);
        // キー操作を有効にする
        SetCanKeyInput(true);

    }

    // ゲームが完全に終了したときに呼ぶ
    public void GameFinish()
    {
        // スコア,HPのリセット
        FindObjectOfType<Score>().Initialize();
        SetHpBar();

        // ゲームオーバー時に、タイトルを表示する
        title.SetActive(true);

        // IDを更新
        playingId = GetPlayedId();
        //if (playingId == -1) playingId = 0;
        SetPlayedId();

        // タイトルにNextPlayerをセット
        StartCoroutine(PlayerForderMonitor());

        Debug.Log("Game Finish");
    }

    public void SetHpBar()
    {
        int val = playerScript.GetHp();
        hpBar.UpdateBar(val, 100f);
        Debug.Log("SetHP:" + val);
    }

    public void SetBulletBar()
    {
        if (FindObjectOfType<Player>()) {
            int val = FindObjectOfType<Player>().GetPlayerBullet();
            int max = FindObjectOfType<Player>().GetPlayerBulletMax();
            bulletBar.UpdateBar(val, max);
        }
    }

    // ゲームの状態をセット
    public void SetState(string state)
    {
        if (state == "Playing" || state == "GameClear" || state == "GameOvar")
        {
            //値をセット
            GameState = state;
            Debug.Log("See State:" + state);
        }
        else
        {
            return;
        }

    }

    // 最後のPlay ID を取得
    public int GetPlayedId()
    {
        return PlayerPrefs.GetInt(playedIdKey, 0);
    }

    // プレイ済みのIDを更新
    public void SetPlayedId()
    {
        var oldId = GetPlayedId();
        //if (oldId == -1) oldId = 0;
        var newId = oldId + 1;
        PlayerPrefs.SetInt(playedIdKey, newId);
        PlayerPrefs.Save();

    }

    // TitleにImageをセット
    public void SetTitleImage(int _id)
    {
        string playerImgStr = "Player/" +_id.ToString();
        Sprite playerImg = Resources.Load<Sprite>(playerImgStr);
        string nameImgStr = "Name/" + _id.ToString();
        Sprite nameImg = Resources.Load<Sprite>(nameImgStr);
        playerImage.sprite = playerImg;
        nameImage.sprite = nameImg;
    }

    // PlayerにImageをセット
    　public void SetPlayerImage(int _id)
    {
        string playerImgStr = "Player/" + _id.ToString();
        Sprite playerImg = Resources.Load<Sprite>(playerImgStr);
        playing.GetComponent<SpriteRenderer>().sprite = playerImg;
    }


    // ゲームの状態を返す
    public string GetState()
    {
        return GameState;
    }

    // PlayerImageフォルダ監視
    IEnumerator PlayerForderMonitor()
    {
        while (true)
        {
            Debug.Log("Search New Player");
            playingId = GetPlayedId();
            int nextid = playingId;
            //if (nextid == -1) nextid = 0;
            string playerImgStr = "Player/" + nextid.ToString();
            Sprite playerImg = Resources.Load<Sprite>(playerImgStr);
            Debug.Log("ID : "+ nextid);
            Debug.Log(playerImg);
            if (playerImg != null)
            {
                titleNextPlayer.SetActive(true);
                titleText.SetActive(false);
                SetCanKeyInput(true);
                SetTitleImage(nextid);
                Debug.Log("Find New Player");
            }
            else
            {
                titleNextPlayer.SetActive(false);
                titleText.SetActive(true);
                SetCanKeyInput(false);
            }
            // 処理を待つ
            yield return new WaitForSeconds(5);
        }
    }

    // キー操作を受け付けるか
    public bool GetCanKeyInput()
    {
        return canKeyInput;
    }

    public void SetCanKeyInput(bool value)
    {
        canKeyInput = value;
    }

    public bool IsPlaying()
    {
        if (title.activeSelf == false && 
            startCountdown.activeSelf == false)return true;

        return false;
    }
}