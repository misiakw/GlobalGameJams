using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Windows;

public class EndGameScript : MonoBehaviour
{
    private EndScreenPtoceed input;

    private void Awake()
    {
        input = new EndScreenPtoceed();
    }
    public void OnEnable()
    {
        input.Enable();
        input.Confirm.Confirm.performed += (c) => SceneManager.LoadScene("MainMap");
    }

    public void OnDisable()
    {
        input.Disable();
    }
}