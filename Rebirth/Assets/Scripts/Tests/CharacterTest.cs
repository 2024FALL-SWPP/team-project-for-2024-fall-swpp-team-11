using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using UnityEngine.InputSystem.Utilities;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.TestTools;

public class CharacterTest : InputTestFixture
{
    private static readonly string logPrefix = "[CharacterTest] ";
    private static readonly string heroPrefabPath = "Assets/Prefabs/GlobalPrefabs/Character/3D Hero.prefab";
    private GameObject heroPrefab;

    [UnitySetUp]
    public IEnumerator UnitySetup()
    {
        AsyncOperationHandle<GameObject> handle = Addressables.LoadAssetAsync<GameObject>(heroPrefabPath);
        yield return handle;
        Assert.IsTrue(handle.Status == AsyncOperationStatus.Succeeded, "Failed to load hero prefab");

        heroPrefab = Object.Instantiate(handle.Result);
        Debug.Log(logPrefix + "Setup complete");
    }

    [UnityTearDown]
    public void UnityTeardown()
    {
        if (heroPrefab != null)
        {
            Object.Destroy(heroPrefab);
        }
    }

    [UnityTest]
    public IEnumerator TestPlayerMovement()
    {
        var keyboard = InputSystem.AddDevice<Keyboard>();
        
        Character3D character = heroPrefab.GetComponent<Character3D>();
        Assert.IsNotNull(character, "Character3D component not found");

        character.enabled = true;
        
        Press(keyboard.upArrowKey);
        yield return new WaitForSeconds(0.1f);

        Assert.Greater(character.transform.position.z, 0, "Character did not move forward");

        Release(keyboard.upArrowKey);
    }
}
