using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTexture : MonoBehaviour {

    // プレイヤーID
    private int playerId;

    // プレイヤーIDを保存するPlayerPrefKey
    private string playerIdKey = "playerId";
    
    // RendereCamera_Aプレハブ
    public GameObject Render_Camera_A;

    // AのRenderTextureCamera
    private RenderTextureCamera renTexCam_A;


    // Use this for initialization
    void Start () {

        // RenderCamera_Aを検索し取得
        renTexCam_A = Render_Camera_A.GetComponent<RenderTextureCamera>();
    }
	
	// Update is called once per frame
	void Update () {

        // Sが押されたら画像保存
        if (Input.GetKeyUp(KeyCode.A)) renTexCam_A.MakeScreen();
		
	}
}
