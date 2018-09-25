using UnityEngine;

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

    // HpBat
    public SimpleHealthBar hpBar;


    void Start()
    {
        // Titleゲームオブジェクトを検索し取得する
        title = GameObject.Find("Title");
    }

    void Update()
    {
        // ゲーム中ではなく、Xキーが押されたらtrueを返す。
        if (IsPlaying() == false && Input.GetKeyDown(KeyCode.X))
        {
            GameStart();
        }

        // Play中ならHpbBarをセット
        if (IsPlaying() == true)
        {
            SetHpBar();
           // if (playerScript.GetHp() == 0) GameOver();
        }
    }

    void GameStart()
    {
        // ゲームスタート時に、タイトルを非表示にしてプレイヤーを作成する
        title.SetActive(false);
        Instantiate(player_origin, player_origin.transform.position, player_origin.transform.rotation);
        //player_origin = player;
        player = GameObject.Find("Player(Clone)");
        playerScript = player.GetComponent<Player>();
    }

    public void GameOver()
    {
        // ゲームオーバー時に、タイトルを表示する
        title.SetActive(true);
    }

   public void SetHpBar()
    {
        int val = playerScript.GetHp();
        hpBar.UpdateBar(val, 100f);
        Debug.Log("SetHP:" + val);
    }


    public bool IsPlaying()
    {
        // ゲーム中かどうかはタイトルの表示/非表示で判断する
        return title.activeSelf == false;
    }
}