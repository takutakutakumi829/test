using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Linq;
using UnityEditor.Experimental.GraphView;

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

    public void Init(Vector2 pos, string path)
    {
        stateKey = "";
        isBase = false;
        position = pos;
        pathName = path;

        events = new List<ActionData>();
        clipPath = new List<string>();
    }
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
public struct UploadSaveDataOutPath
{
    public String stateKey;
    public bool isBase;
    public List<ActionData> events;
}
[Serializable]
public struct UploadSaveDataInPath
{
    public String stateKey;
    public bool isBase;
    public List<ActionData> events;
    public List<String> clipPath;
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

    public void SetActionData(ConditionDataField contion, String conditionData, ContentDataField content, String contentData)
    {
        this.condition = contion;
        this.conditionData = conditionData;
        this.content = content;
        this.contentData = contentData;
    }

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

    public string GetExtensionFileNameSaveData(String path)
    {
        return GetExtensionFileName(path);
    }

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

    public void UploadFile(SaveData data)
    {
        UploadSaveDataInPath inPathData;
        UploadSaveDataOutPath outPathData;
        string json;
        //まずはdataをUpload用に移し替える
        //chipPathが存在する場合とそうでない場合で保存先を分ける
        if(data.clipPath.Count > 0)
        {
            inPathData.isBase = data.isBase;
            inPathData.stateKey = data.stateKey;
            inPathData.events = data.events;
            inPathData.clipPath = data.clipPath;

            // JSONにシリアライズ
            json = JsonUtility.ToJson(inPathData);

        }
        else
        {
            outPathData.isBase = data.isBase;
            outPathData.stateKey = data.stateKey;
            outPathData.events = data.events;

            // JSONにシリアライズ
            json = JsonUtility.ToJson(outPathData);

        }

        // Assetsフォルダに保存する
        var path = EditorUtility.SaveFilePanel("Upload Text", Application.dataPath, "SaveDataName", "txt");
        if (string.IsNullOrEmpty(path))
            return;

        //var path = Application.dataPath + "/" + saveDataName;
        var writer = new StreamWriter(path, false); // 上書き
        writer.WriteLine(json);
        writer.Flush();
        writer.Close();
    }


    public static void SaveGraph(string fileName, GraphView graphView)
    {

        var graphData = ScriptableObject.CreateInstance<GraphAsset>();
        var path = EditorUtility.SaveFilePanelInProject("Save Asset", "SaveAssetName", "asset", "Save to Asset");
        if (string.IsNullOrEmpty(path))
            return;

        if (path == null)
        {
            return;
        }

        var nodes = GetNodes(graphView);
        foreach (var node in nodes)
        {
            var isBase = node.GetIsBase();
            bool isbase = isBase;

            var actionData = node.GetEventsData();
            var clip = node.GetClipPath();

            graphData.data.Add(new SaveData()
            {
                stateKey = node.GetStateKey,
                isBase = isbase,
                events = actionData,
                clipPath = clip,
                pathName = node.GetPathName,
                position = node.GetPosition

            });
        }
        AssetDatabase.CreateAsset(graphData, $"{path}");
        AssetDatabase.SaveAssets();
    }

    private static List<Edge> GetEdges(GraphView graphView) => graphView.edges.ToList();
    private static List<OutlineNode> GetNodes(GraphView graphView) => graphView.nodes.ToList().Cast<OutlineNode>().ToList();

}
