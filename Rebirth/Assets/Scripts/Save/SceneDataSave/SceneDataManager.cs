using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine.SceneManagement;

public class SceneDataManager : SingletonManager<SceneDataManager>
{
    private Dictionary<string, SceneData> sceneDatas = new();
    private HashSet<string> dirtySceneNames = new();
    private List<ISceneDataHandler> sceneDataHandlers = new();

    protected override void Awake()
    {
        base.Awake();

        SceneManager.sceneLoaded += RestoreSceneData;

        SaveManager.save += OnSave;
        SaveManager.load += OnLoad;

        // Add components here
        sceneDataHandlers.Add(new ItemDataHandler());
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= RestoreSceneData;

        SaveManager.save -= OnSave;
        SaveManager.load -= OnLoad;
    }

    private void RestoreSceneData(Scene scene, LoadSceneMode mode)
    {
        StartCoroutine(RestoreWaitWrapper(scene));
    }

    private IEnumerator RestoreWaitWrapper(Scene scene)
    {
        yield return RestoreSceneDataRoutine(scene);
    }

    private IEnumerator RestoreSceneDataRoutine(Scene scene)
    {
        if (sceneDatas.TryGetValue(scene.name, out var sceneData))
        {
            CleanUpScene();

            foreach (var handler in sceneDataHandlers)
            {
                handler.ApplyData(sceneData);
                yield return null;
            }
        }
    }

    public void SaveCurrentSceneData()
    {
        StartCoroutine(SaveWaitWrapper());
    }

    private IEnumerator SaveWaitWrapper()
    {
        yield return SaveCurrentSceneDataRoutine();
    }

    private IEnumerator SaveCurrentSceneDataRoutine()
    {
        var scene = SceneManager.GetActiveScene();
        var sceneData = new SceneData();

        foreach (var handler in sceneDataHandlers)
        {
            handler.CaptureData(sceneData);
            yield return null;
        }

        sceneDatas[scene.name] = sceneData;
        dirtySceneNames.Add(scene.name);
    }


    public async void OnSave()
    {
        StartCoroutine(SaveWaitWrapper());
        
        await SaveSceneDatasToDisk();
    }

    public async void OnLoad()
    {
        await LoadSceneDatasFromDisk();

        StartCoroutine(RestoreWaitWrapper(SceneManager.GetActiveScene()));
    }


    public async Task SaveSceneDatasToDisk()
    {
        var tasks = new List<Task>();
        foreach (var dirtySceneName in dirtySceneNames)
        {
            if (sceneDatas.TryGetValue(dirtySceneName, out var sceneData))
            {
                string filePath = DiskSaveSystem.GetSceneDataPath(dirtySceneName);
                tasks.Add(Task.Run(() => DiskSaveSystem.SaveSceneDataToDisk(filePath, sceneData)));
            }
        }
        await Task.WhenAll(tasks);
    }

    public async Task LoadSceneDatasFromDisk()
    {
        Dictionary<string, SceneData> sceneDatasInDisk = await DiskSaveSystem.LoadAllSceneDataFromDisk();
        foreach (var kvp in sceneDatasInDisk)
        {
            sceneDatas[kvp.Key] = kvp.Value;
        }
    }

    private void CleanUpScene()
    {
        foreach (var item in FindObjectsOfType<WorldItem>())
        {
            Destroy(item.gameObject);
        }
    }
}

