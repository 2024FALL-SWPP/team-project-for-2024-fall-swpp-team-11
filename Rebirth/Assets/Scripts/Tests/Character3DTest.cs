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

public class Character3DTest : InputTestFixture
{
    private static readonly string logPrefix = "[CharacterTest3D] ";
    private static readonly string testSceneName = "InputSystemTest3D";

    private GameObject hero;

    private Keyboard keyboard;
    private Mouse mouse;

    private void AddDevices()
    {
        keyboard = InputSystem.AddDevice<Keyboard>();
        mouse = InputSystem.AddDevice<Mouse>();
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
            playerInput.SwitchCurrentControlScheme("Keyboard&Mouse", keyboard, mouse);
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

        Assert.Greater(hero.transform.position.z, oldPosition.z, "Character did not move forward");

        Release(keyboard.wKey);
        InputSystem.Update();
        yield return null;

        yield return MyTearDown();
    }
}
