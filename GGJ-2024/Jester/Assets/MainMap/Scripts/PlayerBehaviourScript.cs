using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Windows;

using DG.Tweening;
using UnityEngine.UIElements;
using System.Linq;
using UnityEngine.UI;

public class PlayerBehaviourScript : MonoBehaviour
{
    public GameObject CurrentNode;
    public float Speed = 0.5f;

    private MainMapInputs input;
    private bool moveFinished = true;

    // Start is called before the first frame update
    void Start()
    {
        DOTween.Init(this);
        if (!string.IsNullOrEmpty(CrossSceneStorage.MainMapPlayerLocation))
        {
            var finishNode = GameObject.FindGameObjectsWithTag("MainMapFinishNode")
                .FirstOrDefault(n => n.GetComponent<SceneSelectScript>().SceneName == CrossSceneStorage.MainMapPlayerLocation);
            if(finishNode != null)
            {
                CurrentNode = finishNode;
            }

        }

        if (CurrentNode != null)
            this.transform.position = new Vector3(CurrentNode.transform.position.x, CurrentNode.transform.position.y, -2);
    }


    private void Awake()
    {
        input = new MainMapInputs();
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

    public void OnMovementInput(InputAction.CallbackContext context)
    {
        if (!moveFinished)
            return;

        var vect = context.ReadValue<Vector2>();
        if (vect.x == 1)
            Move(NodeNavigationScript.Dir.East);
        if (vect.x == -1)
            Move(NodeNavigationScript.Dir.West);
        if (vect.y == 1)
            Move(NodeNavigationScript.Dir.North);
        if (vect.y == -1)
            Move(NodeNavigationScript.Dir.South);
    }

    private void Json(InputAction.CallbackContext context)
    {
        Debug.Log("Json");
    }

    private void Move(NodeNavigationScript.Dir dir)
    {
        var nav = CurrentNode.GetComponent<NodeNavigationScript>();

        if (nav.Nodes.ContainsKey(dir))
        {
            moveFinished = false;
            var newNode = nav.Nodes[dir];
            if(CurrentNode.GetComponent<SceneSelectScript>() != null)
            {
                CurrentNode.GetComponent<SceneSelectScript>().userInside = false;
            }

            input.EasterEgg.Json.performed += Json;
            var tween = transform.DOMove(new Vector3(newNode.transform.position.x, newNode.transform.position.y, transform.position.z), 1);
            tween.onComplete = () =>
            {
                input.EasterEgg.Json.performed -= Json;
                CurrentNode = newNode;
                moveFinished = true;

                var sceneSelector = CurrentNode.GetComponent<SceneSelectScript>();
                if (sceneSelector != null)
                {
                    sceneSelector.userInside = true;
                    CrossSceneStorage.MainMapPlayerLocation = sceneSelector.SceneName;
                }
                else
                {
                    CrossSceneStorage.MainMapPlayerLocation = string.Empty;
                }
            };
        }
    }

    private class AnimationDestination
    {
        public GameObject target;
        public float destX;
        public float destY;
        public float startX;
        public float startY;
        public float transactionTime;
        public float leftTime;
    }
}
