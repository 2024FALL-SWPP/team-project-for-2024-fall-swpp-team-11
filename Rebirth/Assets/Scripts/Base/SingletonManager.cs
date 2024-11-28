using UnityEngine;

public abstract class SingletonManager<T> : MonoBehaviour where T : SingletonManager<T>
{
    private static T _instance;
    public static T Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<T>();

                if (_instance == null)
                {
                    Debug.LogWarning("[SingletonManager] " + typeof(T).Name + " instance not found. Creating from prefab.");
                    _instance = InstantiateFromPrefab();
                    if (_instance != null)
                    {
                        DontDestroyOnLoad(_instance.gameObject);
                    }
                    else
                    {
                        Debug.LogError("[SingletonManager] " + typeof(T).Name + " instance not found. Prefab not found.");
                    }
                }
            }
            return _instance;
        }
    }

    protected virtual void Awake()
    {
        if (_instance == null)
        {
            _instance = this as T;
            DontDestroyOnLoad(gameObject);
        }
        else if (_instance != this)
        {
            Destroy(gameObject);
        }
    }

    public virtual void Initialize() { }

    private static T InstantiateFromPrefab()
    {
        // Load a prefab holder using the manager's type name
        SingletonManagerPrefabHolder prefabHolder = Resources.Load<SingletonManagerPrefabHolder>($"Managers/{typeof(T).Name}PrefabHolder");

        if (prefabHolder != null && prefabHolder.Prefab != null)
        {
            GameObject instance = Instantiate(prefabHolder.Prefab);
            return instance.GetComponent<T>();
        }

        Debug.LogError($"[SingletonManager] Prefab or prefab holder not found for {typeof(T).Name}. Ensure the asset is in the Resources folder.");
        return null;
    }
}
