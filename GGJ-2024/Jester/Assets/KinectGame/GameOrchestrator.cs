using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOrchestrator : MonoBehaviour
{
    public float TimeLimit = 60f;
    public GameObject ScoreText;
    public GameObject TimeLeftText;
    public GameObject ObjectsGenerator;
    public GameObject Kween;
    public GameObject EndScreen;

    private float ElapsedTime = 0f;
    private bool isRunning = false;
    private int score = 0;
    private float targetScore = 50f;
    private SpriteRenderer happyKweenSpriteRenderer;
    // Start is called before the first frame update
    void Start()
    {
        ElapsedTime = 0f;
        happyKweenSpriteRenderer = Kween.transform.Find("KweenHappy").GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!isRunning)
        {
            if (Input.GetKeyDown(KeyCode.Return))
            {
                ElapsedTime = 0f;
                ObjectsGenerator.GetComponent<ObjectsGenerator>().score = 0;
                isRunning = true;
            }
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            StopGame();
            //SceneManager.LoadScene("MainMap");
        }
    }

    private void FixedUpdate()
    {
        score = ObjectsGenerator.GetComponent<ObjectsGenerator>().score;
        TimeLeftText.GetComponent<TextMeshProUGUI>().text = $"Time left: {TimeLimit - ElapsedTime:00.0}";
        ScoreText.GetComponent<TextMeshProUGUI>().text = $"Score: {score}";
        happyKweenSpriteRenderer.color = new Color(happyKweenSpriteRenderer.color.r, happyKweenSpriteRenderer.color.g, happyKweenSpriteRenderer.color.b, score/targetScore);
        ObjectsGenerator.GetComponent<ObjectsGenerator>().isRunning = isRunning;
        if(isRunning)
        {
            ElapsedTime += Time.deltaTime;
            if (ElapsedTime >= TimeLimit)
            {
                StopGame();
            }
        }
        //else
        //{
        //    if(Input.GetKeyDown(KeyCode.Return))
        //    {
        //        ElapsedTime = 0f;
        //        ObjectsGenerator.GetComponent<ObjectsGenerator>().score = 0;
        //        isRunning = true;
        //    }
        //}
        //if(Input.GetKeyDown(KeyCode.Escape))
        //{
        //    StopGame();
        //    //SceneManager.LoadScene("MainMap");
        //}
    }

    private void StopGame()
    {
        isRunning = false;
        ShowEndScreen();
    }

    private void ShowEndScreen()
    {
        EndScreen.SetActive(true);
        var happySprite = EndScreen.transform.Find("PersonHappy").GetComponent<SpriteRenderer>();
        happySprite.color = new Color(happySprite.color.r, happySprite.color.g, happySprite.color.b, score / targetScore);
        if(score >= targetScore/2)
        {
            EndScreen.GetComponentInChildren<Text>().text = successText;
            CrossSceneStorage.IsKinectComplete = true;
        }
        else
        {
            EndScreen.GetComponentInChildren<Text>().text = failureText;
        }
    }

    private const string successText = @"The Queen:
""Oh, Jester, you are a marvel! Your dance and acrobatics have broken the chains of sorrow that bound me. I haven't laughed like this in ages. Thank you for bringing color back to my world and joy to my heart.""


Jester to the Queen:
""My body is ready! An entire night of acrobatics and singing wouldn't wear me down for you My Queen! <3""";
    
    
    private const string failureText = @"The Queen:
""Oh Jester, your efforts are amendable. Will you dance for me more?""";
}
