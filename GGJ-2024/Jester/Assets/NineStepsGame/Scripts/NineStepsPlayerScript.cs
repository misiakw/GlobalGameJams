using DG.Tweening;
using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;

public class NineStepsPlayerScript : MonoBehaviour
{
    public GameObject[] TopSteps = new GameObject[3];
    public GameObject[] MidSteps = new GameObject[3];
    public GameObject[] BottomSteps = new GameObject[3];
    public GameObject[] MovementSprites = new GameObject[4];
    public GameObject GameView;
    public GameObject EndScreen;
    public float MoveSpeed = 0.25f;
    public float minWaitTime = 0.5f;
    public float maxWaitTime = 1.5f;
    public float GameTimer = 60f;
    public Text TextLabel;

    private GameObject[,] Steps = new GameObject[3, 3];
    private NineStepsInput input;
    private Pos pos = new Pos { X = 1, Y = 1 };
    private Pos lastSelectPoint = new Pos { X = 1, Y = 1 };
    private float inputDelay = 0;
    private float spawnDelay = 0;
    private int score = 0;

    // Start is called before the first frame update
    void Start()
    {
        DOTween.Init(this);
        for (var i = 0; i < 3; i++)
        {
            Steps[0, i] = TopSteps[i];
            Steps[1, i] = MidSteps[i];
            Steps[2, i] = BottomSteps[i];
        }

        var transformPosition = Steps[pos.X, pos.Y].transform.position;
        transform.position = new Vector3(transformPosition.x, transformPosition.y, transform.position.z);
    }

    private void Awake()
    {
        input = new NineStepsInput();
    }
    public void OnEnable()
    {
        input.Enable();
        input.Player.Movement.performed += OnMovementInput;
    }

    public void OnDisable()
    {
        input.Player.Movement.performed -= OnMovementInput;
        input.Disable();
    }

    private void FixedUpdate()
    {
        if(inputDelay > 0)
        {
            inputDelay -= Time.deltaTime;
            if (inputDelay < 0) inputDelay = 0;
        }

        if(spawnDelay <= 0)
        {
            SpawnPoint();
            spawnDelay = Random.Range(minWaitTime, maxWaitTime);
        }
        else
            spawnDelay -= Time.deltaTime;

        if (GameTimer < 0)
        {
            GameTimer = float.MaxValue;
            GameView.SetActive(false);
            EndScreen.SetActive(true);
            input.Disable();
            CrossSceneStorage.IsNineStepsComplete = true;
        }
        else
            GameTimer -= Time.deltaTime;
    }

    public void OnMovementInput(InputAction.CallbackContext context)
    {
        if (inputDelay > 0)
            return;

        inputDelay = MoveSpeed;
        var vect = context.ReadValue<Vector2>();

        var newX = pos.X;
        var newY = pos.Y;

        if (vect.x == -1 && pos.X > 0)
            newX--;
        if (vect.x == 1 && pos.X < 2)
            newX++;
        if (vect.y == -1 && pos.Y < 2)
            newY++;
        if (vect.y == 1 && pos.Y > 0)
            newY--;

        var newPos = Steps[newY, newX].transform.position;
        var dX = pos.X - newX;
        var dY = pos.Y - newY;
        pos.X = newX;
        pos.Y = newY;

        if (dX == 0 && dY == 0) //no movement, no animation
            return;

        foreach (var sprite in MovementSprites)
            sprite.SetActive(false);

        if (dY > 0)
            MovementSprites[0].SetActive(true);
        if (dY < 0)
            MovementSprites[1].SetActive(true);
        if (dX > 0)
            MovementSprites[2].SetActive(true);
        if (dX < 0)
            MovementSprites[3].SetActive(true);


        var tween = transform.DOMove(new Vector3(newPos.x, newPos.y, transform.position.z), MoveSpeed);
        tween.onComplete = () => Steps[pos.Y, pos.X].GetComponent<StepPiontScript>().Enter(this);
    }

    public void SpawnPoint()
    {
        var platesToSelect = new List<Pos>();
        if (lastSelectPoint.X > 0)
            platesToSelect.Add(new Pos { Y = lastSelectPoint.Y, X = lastSelectPoint.X - 1 });
        if (lastSelectPoint.X < 2)
            platesToSelect.Add(new Pos { Y = lastSelectPoint.Y, X = lastSelectPoint.X + 1 });
        if (lastSelectPoint.Y > 0)
            platesToSelect.Add(new Pos { Y = lastSelectPoint.Y - 1, X = lastSelectPoint.X });
        if (lastSelectPoint.Y < 2)
            platesToSelect.Add(new Pos { Y = lastSelectPoint.Y + 1, X = lastSelectPoint.X });

        var rand = Random.Range(0, platesToSelect.Count);
        lastSelectPoint = platesToSelect.ToArray()[rand];
        var newStep = Steps[lastSelectPoint.Y, lastSelectPoint.X];
        newStep.GetComponent<StepPiontScript>().StartCounter();
    }


    public int IncreaseScore(int gain)
    {
        score += gain;
        TextLabel.GetComponent<Text>().text = $"Score {score}";
        return score;
    }

    private struct Pos
    {
        public int X;
        public int Y;
    }
} 

