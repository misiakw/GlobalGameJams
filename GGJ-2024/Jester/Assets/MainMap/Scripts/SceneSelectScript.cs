using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class SceneSelectScript : MonoBehaviour
{
    public string SceneName;
    public Scene Scene;
    private MainMapInputs input;

    public bool userInside = false;
    private void Awake()
    {
        input = new MainMapInputs();
    }
    public void OnEnable()
    {
        input.Enable();
        input.GameSelect.Confirm.performed += OnPressed;
    }
    public void OnDisable()
    {
        input.GameSelect.Confirm.performed -= OnPressed;
        input.Disable();
    }

    public void OnPressed(InputAction.CallbackContext context)
    {
        if (userInside)
        {
            CrossSceneStorage.MainMapPlayerLocation = SceneName;
            SceneManager.LoadScene(SceneName);
        }
    }
}
