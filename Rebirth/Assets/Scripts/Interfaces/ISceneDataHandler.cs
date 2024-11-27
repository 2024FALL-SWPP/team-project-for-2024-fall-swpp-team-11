using System.Threading.Tasks;

public interface ISceneDataHandler
{
    Task CaptureData(SceneData sceneState);
    void ApplyData(SceneData sceneState);
}
