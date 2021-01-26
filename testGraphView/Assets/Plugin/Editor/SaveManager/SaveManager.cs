﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Linq;



public enum ConditionDataField
{
    アクション終了時 = 4,
    アクション開始時,
    アクション終了時is自分の持つイベントから,
    特定のアクションが終わったらisステート用,
    Update処理 = 10,
    視点切り替え完了時, 自分の持つイベントから = 20,
    セットisハンドデータ = 100,
    特定の範囲内に入ったら = 200,
    トリガー処理isString = 300,
    ターゲット座標設定時,
    ターゲットスキル設定時,
    FloatParamの比較 = 500,
    スキルのフラグをチェック = 600
}
public enum ContentDataField
{
    アクションの終了 = 5,
    三人称切り替え = 10,
    FloatParamのセット = 50,
    FloatParamのDeltaTime加算 = 55,
    入力に対する移動 = 100,
    入力に対する回転,
    移動入力に応じたアニメーション,
    スキルからアニメーションパラム設定 = 105,
    移動ベクトルに０をセット = 110,
    キネマティックの設定 = 130,
    コライダーの有効or無効判定,
    スキルを設定is現在のアクションに渡す用 = 140,
    座標を設定is現在のアクションに渡す用 = 142,
    スキルから座標設定 = 150,
    ターゲット座標にボーンオフセットを加算 = 152,
    スキルからの回転設定 = 160,
    回転の直値設定 = 162,
    ターゲットへ向けて移動 = 200,
    ターゲットへのLerp座標移動 = 300,
    ターゲットへのLerp回転移動,
    トリガーの実行 = 500,
    ステート切り替えisステート用 = 1000
}

[Serializable]
public struct SaveData
{

    public String stateKey;
    public bool isBase;
    public List<ActionData> events;
    public List<String> clipPath;
    public Vector2 position;
    public string pathName;


    public void Dump()
    {
        //stateKeyチェック
        Debug.Log("stateKey = " + ((stateKey == null) ? "NoData" : stateKey));

        //isBaseチェック
        if(isBase)
        {
            Debug.Log("isBase = " + isBase);
        }
        //eventsチェック
        if(events.Count > 0)
        {
            Debug.Log("events : ");
            for (int j = 0; j < events.Count; j++)
            {
                events[j].Dump();
            }
        }
        else
        {
            Debug.Log("events = " + "NoData");
        }

        //clipPathチェック
        if(clipPath.Count > 0)
        {
            for(int j = 0; j < clipPath.Count; j++)
            {
                Debug.Log("clipPath = " + clipPath[j]);
            }
        }
        else
        {
            Debug.Log("clipPath = " + "NoData");
        }

        //Debug.Log("x = " + x);
        //Debug.Log("y = " + y);
    }
}

[Serializable]
public struct ActionData
{
    public ConditionDataField condition;
    public String conditionData;
    public ContentDataField content;
    public String contentData;

    //		"condition":5,
    //		"conditionData":"",
    //		"content":1000,
    //		"contentData":"WaitState"

    public void Dump()
    {
        Debug.Log("{");
        Debug.Log("condition = " + condition);

        //conditionData
        Debug.Log("conditionData = " + ((conditionData == null) ? "NoData" : conditionData));

        Debug.Log("content = " + content);

        //contentData
        Debug.Log("contentData = " + ((contentData == null) ? "NoData" : contentData));
        Debug.Log("}");


    }
}



public class SaveManager : MonoBehaviour
{
    public string saveDataName = "save.txt";
    public string pathName = "save.txt";

    public SaveData readData;

    //拡張子探索
    [DllImport("ExtensionCheck")]
    private static extern String GetExtension(String path);
    //FileName探索
    [DllImport("ExtensionCheck")]
    private static extern String GetExtensionFileName(String path);

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.S))
        {
            // Sキーでセーブ実行
            var data = new SaveData();
            // JSONにシリアライズ
            var json = JsonUtility.ToJson(data);

            // Assetsフォルダに保存する
            var path = Application.dataPath + "/" + saveDataName;
            var writer = new StreamWriter(path, false); // 上書き
            writer.WriteLine(json);
            writer.Flush();
            writer.Close();
        }

        else if (Input.GetKeyDown(KeyCode.L))
        {
            // Lキーでロード実行
            // Assetsフォルダからロード
            var info = new FileInfo(Application.dataPath + "/" + saveDataName);
            var reader = new StreamReader(info.OpenRead());
            var json = reader.ReadToEnd();
            var data = JsonUtility.FromJson<SaveData>(json);
            data.Dump();
        }

    }
    public void OpenFile()
    {
        //パスの取得
        var path = EditorUtility.OpenFilePanel("Open text", "", "txt");
        //string[] fillters = { "txt", "json" };
        //var path = EditorUtility.OpenFilePanelWithFilters("Open SaveData", "", fillters);
        if (string.IsNullOrEmpty(path))
            return;
        Debug.Log(path);

        //拡張子探索
        var filename = GetExtensionFileName(path);
        saveDataName = filename;
        pathName = path;


        //読み込み
        var info = new FileInfo(path);
        var reader = new StreamReader(info.OpenRead());
        var json = reader.ReadToEnd();

        //jsonデータの読み込み
        var data = JsonUtility.FromJson<SaveData>(json);
        data.Dump();
        readData = data;
    }
    public void LoadFile(string pathName)
    {
        if (string.IsNullOrEmpty(pathName))
            return;
        Debug.Log(pathName);

        //拡張子探索
        var filename = GetExtensionFileName(pathName);
        saveDataName = filename;


        //読み込み
        var info = new FileInfo(pathName);
        var reader = new StreamReader(info.OpenRead());
        var json = reader.ReadToEnd();

        //jsonデータの読み込み
        var data = JsonUtility.FromJson<SaveData>(json);
        data.Dump();
        readData = data;
    }

}
