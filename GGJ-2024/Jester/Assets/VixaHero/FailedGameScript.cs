using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class FailedGameScript : MonoBehaviour
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

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        
    }
}