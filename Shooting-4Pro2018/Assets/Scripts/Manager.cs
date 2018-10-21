using UnityEngine;
using UnityEngine.UI;


public class Manager : MonoBehaviour
{
    // Playerプレハブ
    public GameObject player;

    // Playing Playerプレハブ
    public GameObject player_origin;

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

    // NameImage
    public Image nameImage;

    // PlayerImage
    public Image playerImage;

    // プレイヤーしたIDを保存するPlayerPrefKey
    private string playedIdKey = "playedId";


    void Start()
    {
        // Titleゲームオブジェクトを検索し取得する
        title = GameObject.Find("Title");

        // タイトルにNextPlayerをセット
        TitleImageSet();
    }

    void Update()
    {
        // ゲーム中ではなく、Xキーが押されたらtrueを返す。
        if (IsPlaying() == false && Input.GetKeyDown(KeyCode.X))
        {
            GameStart();
        }

        // Play中ならBarをセット
        if (IsPlaying() == true)
        {
            SetHpBar();
            SetBulletBar();
            //if (playerScript.GetHp() == 0) GameOver();
        }
    }

    void GameStart()
    {

        // スコアを初期化する
        FindObjectOfType<Score>().Initialize();

        // ゲームスタート時に、タイトルを非表示にしてプレイヤーを作成する
        title.SetActive(false);
        Instantiate(player_origin, player_origin.transform.position, player_origin.transform.rotation);
        //player_origin = player;
        player = GameObject.Find("Player(Clone)");
        playerScript = player.GetComponent<Player>();
    }

    // ゲームが完全に終了したときに呼ぶ
    public void GameFinish()
    {
        // スコア,HPのリセット
        FindObjectOfType<Score>().Initialize();

        // プレイ済みのIDとを更新
        setPlayedId();

        // ゲームオーバー時に、タイトルを表示する
        title.SetActive(true);

        // タイトルにNextPlayerをセット
        TitleImageSet();
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
    public int getPlayedId()
    {
        return PlayerPrefs.GetInt(playedIdKey, -1);
    }

    // プレイ済みのIDを更新
    public void setPlayedId()
    {
        var oldId = getPlayedId();
        var newId = oldId + 1;
        PlayerPrefs.SetInt(playedIdKey, newId);
        PlayerPrefs.Save();

    }

    // TitleにImageをセット
    public void TitleImageSet()
    {
        string playerImgStr = "Player/" + (getPlayedId()+1).ToString();
        Sprite playerImg = Resources.Load<Sprite>(playerImgStr);
        string nameImgStr = "Name/" + (getPlayedId() + 1).ToString();
        Sprite nameImg = Resources.Load<Sprite>(nameImgStr);
        playerImage.sprite = playerImg;
        nameImage.sprite = nameImg;
    }


    // ゲームの状態を返す
    public string GetState()
    {
        return GameState;
    }

    public bool IsPlaying()
    {
        // ゲーム中かどうかはタイトルの表示/非表示で判断する
        return title.activeSelf == false;
    }
}