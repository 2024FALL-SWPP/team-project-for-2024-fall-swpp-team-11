using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Threading.Tasks;

public class DimensionManager : SingletonManager<DimensionManager>
{
    public GameObject player2DPrefab;
    public GameObject player3DPrefab;

    private string anchorID;
    private bool isSwitching = false;
    private Dimension dimension;

    protected override void Awake()
    {
        base.Awake();

        string currentSceneName = SceneManager.GetActiveScene().name;
        dimension = currentSceneName.EndsWith("2D") ? Dimension.TWO_DIMENSION : Dimension.THREE_DIMENSION;
    }

    async void Update()
    {
        if (Input.GetKeyDown(KeyCode.T) && !isSwitching &&
            (CharacterStatusManager.Instance.PlayerState == PlayerState.IsToxified ||
             CharacterStatusManager.Instance.PlayerState == PlayerState.CanUseWeirdPotionCure))
        {
            await SwitchDimension();
        }
    }

    public async Task SwitchDimension()
    {
        string currentSceneName = SceneManager.GetActiveScene().name;
        if (currentSceneName == "Dungeon2D" || currentSceneName == "Dungeon3D")
        {
            Debug.Log("SwitchDimension is disabled in Dungeon2D or Dungeon3D scenes.");
            return;
        }

        isSwitching = true;

        string targetSceneName;
        if (!FindTargetSceneName(out targetSceneName))
        {
            isSwitching = false;
            return;
        }

        GameObject currentPlayer;
        if (!FindCurrentPlayer(out currentPlayer))
        {
            isSwitching = false;
            return;
        }

        Anchor currentAnchor;
        if (!FindCurrentAnchor(currentPlayer, out currentAnchor))
        {
            isSwitching = false;
            return;
        }

        GameObject playerPrefab;
        if (!FindPlayerPrefab(targetSceneName, out playerPrefab))
        {
            isSwitching = false;
            return;
        }

        await SceneTransitionManager.Instance.FadeInAsync();
        await SceneTransitionManager.Instance.LoadSceneAsync(targetSceneName);

        Anchor matchingAnchor;
        if (!FindMatchingAnchor(currentAnchor, out matchingAnchor))
        {
            isSwitching = false;
            return;
        }

        MoveOrSpawnPlayer(matchingAnchor, playerPrefab);
        Debug.Log("Successfully transitioned to anchor with ID: " + currentAnchor.anchorID + matchingAnchor.anchorID);

        InventoryManager.Instance.HandleSceneChange();
        CharacterStatusManager.Instance.RefreshStatusUI();

        await SceneTransitionManager.Instance.FadeOutAsync();

        isSwitching = false;
    }

    private void MoveOrSpawnPlayer(Anchor matchingAnchor, GameObject playerPrefab)
    {
        GameObject newPlayer = GameObject.FindWithTag("Player");
        if (newPlayer == null)
        {
            newPlayer = Instantiate(playerPrefab, matchingAnchor.transform.position, Quaternion.identity);
        }
        else
        {
            newPlayer.transform.position = matchingAnchor.transform.position;
        }
    }

    private bool FindPlayerPrefab(string targetSceneName, out GameObject playerPrefab)
    {
        playerPrefab = GetPlayerPrefabForScene(targetSceneName);
        if (playerPrefab == null)
        {
            Debug.LogError("Player prefab not found for the scene: " + targetSceneName);
            isSwitching = false;
            return false;
        }
        return true;
    }

    private bool FindCurrentAnchor(GameObject currentPlayer, out Anchor currentAnchor)
    {
        currentAnchor = FindClosestAnchor(currentPlayer.transform.position);
        if (currentAnchor == null)
        {
            Debug.LogError("No anchor found in the current scene.");
            isSwitching = false;
            return false;
        }
        return true;
    }

    private bool FindMatchingAnchor(Anchor currentAnchor, out Anchor matchingAnchor)
    {
        anchorID = currentAnchor.anchorID;
        matchingAnchor = FindAnchorByID(anchorID);
        if (matchingAnchor == null)
        {
            Debug.LogError("Matching anchor not found in the target scene." + currentAnchor.anchorID);
            isSwitching = false;
            return false;
        }
        return true;
    }

    private bool FindCurrentPlayer(out GameObject currentPlayer)
    {
        currentPlayer = GameObject.FindGameObjectWithTag("Player");
        if (currentPlayer == null)
        {
            Debug.LogError("Player not found in the current scene.");
            isSwitching = false;
            return false;
        }
        return true;
    }

    private bool FindTargetSceneName(out string targetSceneName)
    {
        string currentSceneName = SceneManager.GetActiveScene().name;
        if (currentSceneName.EndsWith("2D"))
        {
            targetSceneName = currentSceneName.Substring(0, currentSceneName.Length - 2) + "3D";
            dimension = Dimension.THREE_DIMENSION;
        }
        else if (currentSceneName.EndsWith("3D"))
        {
            targetSceneName = currentSceneName.Substring(0, currentSceneName.Length - 2) + "2D";
            dimension = Dimension.TWO_DIMENSION;
        }
        else
        {
            Debug.LogError("Scene name must end with '2D' or '3D'.");
            isSwitching = false;
            targetSceneName = null;
            return false;
        }
        return true;
    }

    Anchor FindClosestAnchor(Vector3 position)
    {
        Anchor[] anchors = FindObjectsOfType<Anchor>();

        if (anchors.Length == 0)
            return null;

        Anchor closestAnchor = null;
        float closestDistance = Mathf.Infinity;

        foreach (Anchor anchor in anchors)
        {
            float distance = Vector3.Distance(position, anchor.transform.position);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestAnchor = anchor;
            }
        }

        return closestAnchor;
    }

    Anchor FindAnchorByID(string anchorID)
    {
        Anchor[] anchors = FindObjectsOfType<Anchor>();

        foreach (Anchor anchor in anchors)
        {
            if (anchor.anchorID == anchorID)
            {
                return anchor;
            }
        }

        return null;
    }

    GameObject GetPlayerPrefabForScene(string sceneName)
    {
        if (sceneName.EndsWith("2D"))
        {
            return player2DPrefab;
        }
        else if (sceneName.EndsWith("3D"))
        {
            return player3DPrefab;
        }

        return null;
    }

    public Dimension GetCurrentDimension()
    {
        return dimension;
    }
}
