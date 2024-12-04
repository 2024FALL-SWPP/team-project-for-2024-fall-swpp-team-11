using System.Threading.Tasks;

public interface ISceneDataHandler
{
    Task CaptureDataAsync(SceneData sceneState);
    Task ApplyDataAsync(SceneData sceneState);
}
