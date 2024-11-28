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

        SceneManager.sceneLoaded += async (scene, mode) => await RestoreSceneDataAsync(scene);

        SaveManager.save += OnSave;
        SaveManager.load += OnLoad;

        // Add components here
        sceneDataHandlers.Add(new ItemDataHandler());
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= async (scene, mode) => await RestoreSceneDataAsync(scene);

        SaveManager.save -= OnSave;
        SaveManager.load -= OnLoad;
    }

    private async Task RestoreSceneDataAsync(Scene scene)
    {
        if (sceneDatas.TryGetValue(scene.name, out var sceneData))
        {
            CleanUpScene();

            var tasks = new List<Task>();
            foreach (var handler in sceneDataHandlers)
            {
                tasks.Add(handler.ApplyDataAsync(sceneData));
            }
            await Task.WhenAll(tasks);
        }
    }

    public async Task SaveCurrentSceneDataAsync()
    {
        var scene = SceneManager.GetActiveScene();
        var sceneData = new SceneData();

        var tasks = new List<Task>();
        foreach (var handler in sceneDataHandlers)
        {
            tasks.Add(handler.CaptureDataAsync(sceneData));
        }
        await Task.WhenAll(tasks);

        sceneDatas[scene.name] = sceneData;
        dirtySceneNames.Add(scene.name);
    }

    public async void OnSave()
    {
        await SaveCurrentSceneDataAsync();
        await SaveSceneDatasToDisk();
    }

    public async void OnLoad()
    {
        await LoadSceneDatasFromDisk();
        await RestoreSceneDataAsync(SceneManager.GetActiveScene());
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
