using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "new SingletonManagerPrefabHolder", menuName = "SingletonManagerPrefabHolder")]
public class SingletonManagerPrefabHolder : ScriptableObject
{
    public GameObject Prefab;
}