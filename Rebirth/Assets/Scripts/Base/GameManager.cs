using UnityEngine;

public class GameManager : MonoBehaviour
{
    public CharacterStatusManager characterStatusManager;
    public DialogueManager dialogueManager;
    public GameStateManager gameStateManager;
    public InventoryManager inventoryManager;
    public SceneTransitionManager sceneTransitionManager;
    public NPCManager npcManager;
    public QuestManager questManager;
    public DimensionManager dimensionManager;

    private void Awake()
    {
        // create singleton instances
        dimensionManager = DimensionManager.Instance;
        sceneTransitionManager = SceneTransitionManager.Instance;
        gameStateManager = GameStateManager.Instance;
        questManager = QuestManager.Instance;
        npcManager = NPCManager.Instance;
        inventoryManager = InventoryManager.Instance;
        dialogueManager = DialogueManager.Instance;
        characterStatusManager = CharacterStatusManager.Instance;

        // initialize singleton instances
        // can customize the order of initialization
        dimensionManager.Initialize();
        sceneTransitionManager.Initialize();
        gameStateManager.Initialize();
        questManager.Initialize();
        npcManager.Initialize();
        inventoryManager.Initialize();
        dialogueManager.Initialize();
        characterStatusManager.Initialize();
    }
}