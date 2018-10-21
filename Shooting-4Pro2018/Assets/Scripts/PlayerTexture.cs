using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerTexture : MonoBehaviour {

    // プレイヤーID
    private int playerId;

    // プレイヤーIDを保存するPlayerPrefKey
    private string playerIdKey = "playerId";

    // RendereCamera_Aプレハブ
    public GameObject Render_Camera_A;

    // AのRenderTextureCamera
    private RenderTextureCamera renTexCam_A;

    // RendereCamera_Bプレハブ
    public GameObject Render_Camera_B;

    // BのRenderTextureCamera
    private RenderTextureCamera renTexCam_B;

    // ImageTarget
    public GameObject imageTarget;

    // PlayerImage
    public Image playerImage;

    // PlayerImagesk
    public Image playerImageMask;

    // カウントダウン関係
    [SerializeField]
    private Text textCountdown;

    [SerializeField]
    private Image imageMask;

    // セットしてから登録までの時間
    public int waitTime = 20;


    // ステータスメッセージ
    public Text statusText;

    // 一回のみ実行する
    private int onse = 0, onse2 = 0;

    // 開始時間を記録
    private int startTime;



    // Use this for initialization
    void Start() {

        // RenderCamera_Aを検索し取得
        renTexCam_A = Render_Camera_A.GetComponent<RenderTextureCamera>();
        renTexCam_B = Render_Camera_B.GetComponent<RenderTextureCamera>();

    }

    // Update is called once per frame
    void Update() {

        GetImage();


        // IDをリセット
        // すべてのキーをリセット
        if (Input.GetKey(KeyCode.I) && Input.GetKey(KeyCode.D) && Input.GetKey(KeyCode.R))
        {
            idReset();
        }

    }

    // テクスチャを保存
    private void SaveImage()
    {
        // IDを取得
        string _ID = idGenerat().ToString();

        // 画像として保存
        renTexCam_A.MakeScreen(_ID, "Player");
        renTexCam_B.MakeScreen(_ID, "Name");
    }

    //  テクスチャを取得
    private void GetImage()
    {

        if (onse == 0)
        {
            startTime = (int)Time.time;
            onse = 1;
        }

        if (IsTracking() == true)
        {
            if (Time.time - startTime <= 3)//
            {
                statusText.text = "";

                if (onse2 == 0)
                {
                    StartCoroutine("CountdownCoroutine");

                    Debug.Log("Tracked");
                    onse2 = 1;
                }
            }
        }
        else
        {
            if (onse2 == 1)
            {
                StopCoroutine("CountdownCoroutine");
                onse2 = 0;
            }

            textCountdown.text = "";
            textCountdown.gameObject.SetActive(false);
            imageMask.gameObject.SetActive(false);
            playerImageMask.gameObject.SetActive(false);
            statusText.text = "紙をセットしてください．";
            onse = 0;
        }

    }


    // トラッキング中かどうか
    public bool IsTracking()
    {
        var TrackingStatus = imageTarget.GetComponent<DefaultTrackableEventHandler>().GetTrackingStatus();
        var UiStatus = false;

        if ((TrackingStatus == true) && (UiStatus == false))
        {
            return true;
        }
        else
        {
            return false;

        }
    }

    // カウントダウン～画像取得まで
    IEnumerator CountdownCoroutine()
    {
        imageMask.gameObject.SetActive(true);
        textCountdown.gameObject.SetActive(true);

        textCountdown.text = "Your image has been tracked!";
        yield return new WaitForSeconds(2.0f);

        textCountdown.text = "3";
        yield return new WaitForSeconds(1.0f);

        textCountdown.text = "2";
        yield return new WaitForSeconds(1.0f);

        textCountdown.text = "1";
        yield return new WaitForSeconds(1.0f);

        textCountdown.text = "";
        yield return new WaitForSeconds(0.5f);

        // 画像を取得
        statusText.text = "読み取り中...";

        SaveImage();
        yield return new WaitForSeconds(0.5f);

        statusText.text = "読み取り完了!";
        playerImageMask.gameObject.SetActive(true);
        string playerImgStr = "Player/" + getNewId().ToString();
        Sprite playerImg = Resources.Load<Sprite>(playerImgStr);
        playerImage.sprite = playerImg;
        yield return new WaitForSeconds(3.0f);

        statusText.text = "読み取りが完了しました．\n紙を持って，コントローラー付近でお待ち下さい！";
        textCountdown.gameObject.SetActive(false);
        imageMask.gameObject.SetActive(false);
        playerImageMask.gameObject.SetActive(false);

        onse2 = 0;
    }

    // IDを生成
    public int idGenerat()
    {
        // 値を設定
        int oldId = PlayerPrefs.GetInt(playerIdKey, -1);
        int newId = oldId + 1;

        // 値をセット
        PlayerPrefs.SetInt(playerIdKey, newId);
        PlayerPrefs.Save();

        return newId;
    }

    // 最新のIDを取得
    public int getNewId()
    {
        return PlayerPrefs.GetInt(playerIdKey, 0);
    }


    private void idReset()
    {
        PlayerPrefs.DeleteKey(playerIdKey);
        Debug.Log("### ALL ID ARE RESET! ###");
    }
}
