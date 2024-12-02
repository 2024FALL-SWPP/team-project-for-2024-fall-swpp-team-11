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

public class Character2DTest : InputTestFixture
{
    private static readonly string logPrefix = "[CharacterTest2D] ";
    private static readonly string testSceneName = "InputSystemTest2D";

    private GameObject hero;

    private Keyboard keyboard;

    private void AddDevices()
    {
        keyboard = InputSystem.AddDevice<Keyboard>();
    }

    private IEnumerator LoadScene(string sceneName)
    {
        var loadOp = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Single);
        while (!loadOp.isDone)
        {
            yield return null;
        }
    }

    private void SetupAfterLoad()
    {
        hero = GameObject.FindWithTag("Player");

        var playerInput = hero.GetComponent<PlayerInput>();
        if (playerInput == null)
        {
            playerInput.neverAutoSwitchControlSchemes = true;
            playerInput.SwitchCurrentControlScheme("Keyboard", keyboard);
        }
    }

    private IEnumerator MySetUp()
    {
        AddDevices();
        yield return LoadScene(testSceneName);
        SetupAfterLoad();
    }

    private IEnumerator MyTearDown()
    {
        // SceneManager.UnloadSceneAsync(testSceneName);
        yield return null;
    }

    [UnityTest]
    public IEnumerator TestPlayerMovement()
    {
        yield return MySetUp();

        Vector3 oldPosition = hero.transform.position;
        Debug.Log(logPrefix + "Old position: " + oldPosition);

        Press(keyboard.wKey);
        InputSystem.Update();
        yield return new WaitForSeconds(2f);

        Debug.Log(logPrefix + "New position: " + hero.transform.position);

        Assert.Greater(hero.transform.position.y, oldPosition.y, "Character did not move forward");

        Release(keyboard.wKey);
        InputSystem.Update();
        yield return null;

        yield return MyTearDown();
    }
}
