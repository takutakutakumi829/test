using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Linq;

[Serializable]
public struct SaveData
{

    public String stateKey;
    public bool isBase;
    public List<ActionData> events;
    public List<String> clipPath;

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
    public int condition;
    public String conditionData;
    public int content;
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


        //読み込み
        var info = new FileInfo(path);
        var reader = new StreamReader(info.OpenRead());
        var json = reader.ReadToEnd();

        //jsonデータの読み込み
        var data = JsonUtility.FromJson<SaveData>(json);
        data.Dump();
        readData = data;
    }

}
