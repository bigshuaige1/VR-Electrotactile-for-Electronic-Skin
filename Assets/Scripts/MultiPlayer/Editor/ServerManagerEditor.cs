using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(ServerManager))]
public class ServerManagerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        serializedObject.Update();

        ServerManager serverManager = (ServerManager)target;
        foreach (var info in serverManager.discoveredServers.Values)
        {
            if (GUILayout.Button(info.EndPoint.Address.ToString()))
                serverManager.Connect(info);
        }
        
        GUILayout.BeginHorizontal();
        if (GUILayout.Button("刷新房间"))
            serverManager.FindServer();
        if (GUILayout.Button("创建房间"))
            serverManager.StartHost();
        if (GUILayout.Button("退出房间"))
            serverManager.ExitHost();
        GUILayout.EndHorizontal();

        serializedObject.ApplyModifiedProperties();
    }
}
