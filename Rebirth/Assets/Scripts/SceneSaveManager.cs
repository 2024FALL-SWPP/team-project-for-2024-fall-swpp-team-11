using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

[System.Serializable]
public class SceneObjectData
{
    public string objectID; // 고유 ID 또는 이름
    public Vector3 position;
    public Quaternion rotation;
}

[System.Serializable]
public class PositionData
{
    public float x;
    public float y;
    public float z;
}

[System.Serializable]
public class RotationData
{
    public float x;
    public float y;
    public float z;
    public float w;
}

[System.Serializable]
public class SceneObjectDataLoaded
{
    public string objectID; 
    public PositionData position; 
    public RotationData rotation; 
}

[System.Serializable]
public class SceneData
{
    public List<SceneObjectData> objectsInScene; // 저장할 오브젝트들
}

[System.Serializable]
public class SceneDataWrapper
{
    public SceneObjectDataLoaded[] objectsInScene; // SceneObjectData를 리스트로 저장
}


public class SceneSaveManager : SingletonManager<SceneSaveManager>
{
    private string saveDirectory;

    private void Start()
    {
        saveDirectory = Application.persistentDataPath + "/SceneSaves/";

        if (!Directory.Exists(saveDirectory))
        {
            Directory.CreateDirectory(saveDirectory);
        }

        // 씬이 로드되거나 언로드될 때 처리
        SceneManager.sceneLoaded += OnSceneLoaded;
        SceneManager.sceneUnloaded += OnSceneUnloaded;
    }

    private void OnDisable()
    {
        // 씬 이벤트 구독 해제
        SceneManager.sceneLoaded -= OnSceneLoaded;
        SceneManager.sceneUnloaded -= OnSceneUnloaded;
    }

    private void OnSceneUnloaded(Scene scene)
    {
        // 씬이 언로드되기 전에 상태 저장
        // SaveSceneState(scene.name);
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // 씬이 로드된 후 상태 복원
        Debug.Log('w');
        LoadSceneState(scene.name);
    }

    // public void SaveSceneState()
    // {
    //     string saveFilePath = GetSceneSaveFilePath("hii");
    //     SceneData sceneData = new SceneData();
    //     sceneData.objectsInScene = new List<SceneObjectData>();

    //     // Save logic (예: 특정 태그를 가진 아이템들만 저장)
    //     GameObject[] taggedObjects = GameObject.FindGameObjectsWithTag("Item");
    //     foreach (var obj in taggedObjects)
    //     {
    //         SceneObjectData objectData = new SceneObjectData
    //         {
    //             objectID = obj.name,
    //             position = obj.transform.position,
    //             rotation = obj.transform.rotation
    //         };
    //         sceneData.objectsInScene.Add(objectData);
    //     }

    //     // 데이터를 JSON으로 직렬화하여 파일에 저장
    //     string json = JsonUtility.ToJson(sceneData);
    //     System.IO.File.WriteAllText(saveFilePath, json);
    // }

    public void SaveSceneState(string sceneName)
    {
        string saveFilePath = GetSceneSaveFilePath(sceneName);
        SceneData sceneData = new SceneData();
        sceneData.objectsInScene = new List<SceneObjectData>();

        GameObject[] taggedObjects = GameObject.FindGameObjectsWithTag("Item"); 
        Debug.Log(taggedObjects.Length);
        foreach (var obj in taggedObjects)
        {
            SceneObjectData objectData = new SceneObjectData
            {
                objectID = obj.name, // 고유한 ID로 바꾸면 더 안전
                position = obj.transform.position,
                rotation = obj.transform.rotation,
            };
            sceneData.objectsInScene.Add(objectData);
        }


        // JSON 형식으로 저장
        string json = JsonUtility.ToJson(sceneData);
        Debug.Log(json);
        System.IO.File.WriteAllText(saveFilePath, json);
        Debug.Log($"Scene {sceneName} data saved at {saveFilePath}");
    }

    // 씬 상태 로드
    public void LoadSceneState(string sceneName)
    {
        string saveFilePath = GetSceneSaveFilePath(sceneName);
        Debug.Log("LOAD");

        if (File.Exists(saveFilePath))
        {
            string json = File.ReadAllText(saveFilePath);
            // string json = "{\"objectsInScene\":[{\"objectID\":\"[3D item]IDcard\",\"position\":{\"x\":-62.735252380371097,\"y\":3.4596569538116457,\"z\":-66.51517486572266},\"rotation\":{\"x\":-0.000024726445190026425,\"y\":-0.0011819228529930115,\"z\":0.000013898476026952267,\"w\":0.9999993443489075}}]}";
            // string json = "{\"objectsInScene\":[{\"objectID\":\"[3D item]IDcard\",\"position\":{\"x\":-62.735252380371097,\"y\":3.4596569538116457,\"z\":-69.51517486572266}}]}";
            // string json = "{\"skillDatas\": [{\"ID\":1}]}";
            // JSON을 객체로 역직렬화
            // Debug.Log(json);

            // 씬에 있는 기존 아이템들을 모두 삭제
            // SkillDatas s = JsonUtility.FromJson<SkillDatas>(json); 
            SceneDataWrapper wrapper = JsonUtility.FromJson<SceneDataWrapper>(json);
            // SceneDataWrapper wrapper = JsonConvert.DeserializeObject<SceneDataWrapper>(json);
            // Debug.Log(wrapper.objectsInScene);
            // Debug.Log(wrapper.objectsInScene.Length);

            GameObject[] existingItems = GameObject.FindGameObjectsWithTag("Item"); // "Item" 태그로 아이템 찾기
            foreach (var item in existingItems)
            {
                Destroy(item); // 기존 아이템 삭제
            } 

            Debug.Log(json);
            // Debug.Log(wrapper);
            // Debug.Log(wrapper.sceneObjects.Count);
         
            foreach (var objectData in wrapper.objectsInScene)
            {
                string cleanObjectID = objectData.objectID.Replace("(Clone)", "").Trim();
                GameObject prefab = Resources.Load<GameObject>("3D/" + cleanObjectID);
                if (prefab != null)
                {
                    // Prefab을 씬에 인스턴스화
                    GameObject newItem = Instantiate(prefab);

                    // 아이템 위치와 회전 설정
                    newItem.transform.position = new Vector3(objectData.position.x, objectData.position.y, objectData.position.z);
                    // newItem.transform.rotation = objectData.rotation;

                 
                }
                else
                {
                    Debug.LogWarning("Prefab not found: " + objectData.objectID);
                }
            }
        }
    }

    // 씬 이름에 맞는 저장 파일 경로 얻기
    private string GetSceneSaveFilePath(string sceneName)
    {
        return Path.Combine(saveDirectory, sceneName + ".json");
    }
}
