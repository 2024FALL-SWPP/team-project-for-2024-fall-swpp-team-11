using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using UnityEngine.InputSystem.Utilities;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.TestTools;
using UnityEngine.SceneManagement;

public class CharacterTest : InputTestFixture
{
    private static readonly string logPrefix = "[CharacterTest] ";
    private static readonly string heroPrefabPath = "Assets/Prefabs/GlobalPrefabs/Character/3D Hero.prefab";
    private static readonly string cameraPrefabPath = "Assets/Prefabs/GlobalPrefabs/Camera/MainCamera3D.prefab";
    private static readonly string testSceneName = "Village3D";

    private GameObject hero;

    private void MoveMouse(Vector2 position)
    {
        InputSystem.QueueStateEvent(Mouse.current, new MouseState
        {
            position = position
        });
        InputSystem.Update();
    }

    private IEnumerator MoveMouseTime(Vector2 position, float time)
    {
        // move mout to position linearly in time
        Vector2 start = Mouse.current.position.ReadValue();
        float elapsedTime = 0;
        while (elapsedTime < time)
        {
            elapsedTime += Time.deltaTime;
            MoveMouse(Vector2.Lerp(start, position, elapsedTime / time));
            yield return null;
        }
    }

    private IEnumerator MoveMouseTimeDelta(Vector2 position, float time)
    {
        // move mout to position linearly in time
        Vector2 start = Mouse.current.position.ReadValue();
        float elapsedTime = 0;
        while (elapsedTime < time)
        {
            elapsedTime += Time.deltaTime;
            MoveMouse(start + (position - start) * elapsedTime / time);
            yield return null;
        }
    }

    private void ResetMouse()
    {
        Vector2 screenCenter = new Vector2(Screen.width / 2, Screen.height / 2);
        InputSystem.QueueStateEvent(Mouse.current, new MouseState
        {
            position = screenCenter
        });
        InputSystem.Update();
    }


    [UnitySetUp]
    public IEnumerator UnitySetup()
    {
        ResetMouse();

        var loadOp = SceneManager.LoadSceneAsync(testSceneName, LoadSceneMode.Single);
        while (!loadOp.isDone)
        {
            yield return null;
        }

        // AsyncOperationHandle<GameObject> handle = Addressables.LoadAssetAsync<GameObject>(heroPrefabPath);
        // yield return handle;
        // Assert.IsTrue(handle.Status == AsyncOperationStatus.Succeeded, "Failed to load hero prefab");
        // heroPrefab = Object.Instantiate(handle.Result);

        // handle = Addressables.LoadAssetAsync<GameObject>(cameraPrefabPath);
        // yield return handle;
        // Assert.IsTrue(handle.Status == AsyncOperationStatus.Succeeded, "Failed to load camera prefab");
        // GameObject cameraPrefab = Object.Instantiate(handle.Result);

        // var camera = cameraPrefab.GetComponent<CameraController3D>();
        // Assert.IsNotNull(camera, "CameraController3D component not found");

        // camera.enabled = true;
        // camera.target = heroPrefab.transform;

        hero = GameObject.FindWithTag("Player");

        Debug.Log(logPrefix + "Setup complete");
    }

    [UnityTearDown]
    public void UnityTeardown()
    {
        // if (heroPrefab != null)
        // {
        //     Object.Destroy(heroPrefab);
        // }
        SceneManager.LoadScene("MainMenu");
    }

    [UnityTest]
    public IEnumerator TestPlayerMovement()
    {
        var keyboard = InputSystem.AddDevice<Keyboard>();
        var mouse = InputSystem.AddDevice<Mouse>();

        Vector3 oldPosition = hero.transform.position;
        Debug.Log(logPrefix + "Old position: " + oldPosition);

        // Press(keyboard.upArrowKey);
        // yield return new WaitForSeconds(2f);

        yield return MoveMouseTimeDelta(new Vector2(100, 100), 2f);

        Debug.Log(logPrefix + "New position: " + hero.transform.position);

        Assert.Greater(hero.transform.position.z, oldPosition.z, "Character did not move forward");

        Release(keyboard.upArrowKey);
    }
}
