using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor;
using UnityEditor.Callbacks;

[CreateAssetMenu(fileName = "graph.asset", menuName = "Graph Asset")]
public class GraphAsset : ScriptableObject
{
    public List<SerializableNode> nodes = new List<SerializableNode>();
    public List<SaveData> data = new List<SaveData>();
}

[System.Serializable]
public class SerializableNode
{
    public string name;
    public Vector2 position;
    public List<SerializableEdge> edges = new List<SerializableEdge>();
    public SaveData data;
}

[System.Serializable]
public class SerializableEdge
{
    public int toID;
}
