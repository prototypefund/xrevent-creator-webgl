using System;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(WebGLConnection))]
public class WebGLConnectorUI : Editor
{
    private bool localRoomFile;
    
    string GUIDTextField = "";
    string newGUIDTextField = "";

    string pathTOGLTF =
        "bag-chair.gltf";
    public override void OnInspectorGUI()
    {
        WebGLConnection myTarget = (WebGLConnection) target;
        
        
        DrawDefaultInspector();

        TestConfigHelper.PrefabList = myTarget.prefabList;
        
        GUILayout.Space(20);
        if(GUILayout.Button("Delete Selected Object"))
        {
            myTarget.DeleteSelectedItem();
        }
        GUILayout.Space(20);
        GUILayout.Label("Spawn Primitive List:");
        

        

        foreach (var x in TestConfigHelper.PrefabList)
        {
            if(GUILayout.Button($"Spawn {x.name}"))
            {
                myTarget.SpawnPrefab(x.name);
            }
        }
        GUILayout.Space(20);
        GUILayout.Label("GLTF Importer");
        
        GUILayout.BeginHorizontal();
        GUILayout.Label("Path to GLTF:     ",GUILayout.ExpandWidth(false));
        pathTOGLTF = GUILayout.TextField(pathTOGLTF);
        GUILayout.EndHorizontal();
        if (GUILayout.Button("GLTFImport"))
        {
            myTarget.SpawnGltf(pathTOGLTF);
        }
        
        GUILayout.Space(20);
        GUILayout.Label("Persistent Manager Communication");
        persistenceManager pm = myTarget.GetComponent<persistenceManager>();
        GUILayout.BeginHorizontal();
        GUILayout.BeginHorizontal();
        
        newGUIDTextField = GUILayout.TextField(newGUIDTextField);
        if (GUILayout.Button("change GUID", GUILayout.ExpandWidth(false)))
        {
            pm.setGUID(newGUIDTextField);
        }
        GUILayout.EndHorizontal();
        if (localRoomFile == false)
        {
            if (GUILayout.Button("switch to local", GUILayout.ExpandWidth(false)))
            {
                localRoomFile = true;
            }
            GUIDTextField = GUILayout.TextField(pm.getGUID());
        }
        else
        {
            if (GUILayout.Button("switch to Xrchitecture.de webserver", GUILayout.ExpandWidth(false)))
            {
                localRoomFile = false;
            }
            GUIDTextField = GUILayout.TextField(Application.persistentDataPath + "/TestRooms/test.json");
        }
        
        GUILayout.EndHorizontal();
        
        
        
        
        
        GUILayout.BeginHorizontal();
        if(GUILayout.Button("Create new Room"))
        {
            myTarget.newRoom();
        }
        if(GUILayout.Button("Load a Room"))
        {
            if (localRoomFile)
            {
                Debug.Log(new System.IO.FileInfo(GUIDTextField).Exists);
                myTarget.roomSaverLoader.LoadRoom(true,GUIDTextField);
                return;   
            }
            myTarget.loadRoom(GUIDTextField);
        }
        if(GUILayout.Button("Save a Room"))
        {
            if (localRoomFile)
            {
                myTarget.roomSaverLoader.SaveRoom(true,GUIDTextField);
                return;   
            }
            myTarget.SaveRoom();
        }
        GUILayout.EndHorizontal();



    }
}
