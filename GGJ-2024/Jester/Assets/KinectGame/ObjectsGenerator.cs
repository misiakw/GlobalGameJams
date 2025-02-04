using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static Assets.KinectGame.Enums;

public class ObjectsGenerator : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject TargetPrefab;
    private GameObject[] Targets = new GameObject[4];
    private bool redrawObjects = true;
    public int score = 0;
    public bool isRunning = false;
    private int previousPreset = 0;

    private List<Vector3[]> TargetPositions = new List<Vector3[]>()
    {
        //                       LH,                        RH,                             LF,                             RF
        new Vector3[4] { new Vector3(1f,2.5f),         new Vector3(-0.8f,2f),       new Vector3(-1.5f,-2f),         new Vector3(1.5f,-2f), }, //cross hands
        new Vector3[4] { new Vector3(-2f, 2.8f),    new Vector3(2f,2.8f),       new Vector3(-1.5f,-2),          new Vector3(1.5f,-2)  }, //spread
        new Vector3[4] { new Vector3(-2f,2f),        new Vector3(2f, 2f),         new Vector3(0f,-2.3f),          new Vector3(1.5f,-1)  }, //RF up
        new Vector3[4] { new Vector3(-2f,2f),        new Vector3(2f, 2f),         new Vector3(-1.5f,-1f),          new Vector3(0f,-2.3f)  }, //LF up
        new Vector3[4] { new Vector3(-2f, 2.8f),     new Vector3(2f, 2.8f),     new Vector3(-0.5f,-2.3f),       new Vector3(0.5f,-2.3f)  }, //Y
        new Vector3[4] { new Vector3(-0.2f,2.5f),      new Vector3(0.2f, 2.5f),       new Vector3(-0.5f,-2.3f),       new Vector3(0.5f,-2.3f)  }, //face cover
        new Vector3[4] { new Vector3(1f,2f),         new Vector3(2f, 2f),         new Vector3(-0.5f,-2.3f),       new Vector3(0.5f,-2.3f)  }, //C
        new Vector3[4] { new Vector3(-2f,2f),        new Vector3(-1f, 2f),        new Vector3(-0.5f,-2.3f),       new Vector3(0.5f,-2.3f)  }, //reverse C
        new Vector3[4] { new Vector3(1f,2.5f),         new Vector3(2f, 2.5f),         new Vector3(-0.5f,-2.3f),       new Vector3(0.5f,-2.3f)  }, //C up
        new Vector3[4] { new Vector3(-2f,2.5f),        new Vector3(-1f, 2.5f),        new Vector3(-0.5f,-2.3f),       new Vector3(0.5f,-2.3f)  }, //reverse C up
        new Vector3[4] { new Vector3(-0.2f,3),      new Vector3(0.2f, 3),       new Vector3(-0.5f,-2.3f),       new Vector3(0.5f,-2.3f)  }, //A
    };

    void Start()
    {
        Targets = new GameObject[4];
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(!isRunning)
        {
            return;
        }

        if (redrawObjects)
        {
            redrawObjects = false;
            RestartTargets(true);
        }
        //TriggerTargets();
        redrawObjects = CheckIfAllTargetsDestroyed();
    }

    void RestartTargets(bool usePresets)
    {
        if (usePresets)
        {
            int selectedPreset = UnityEngine.Random.Range(0, TargetPositions.Count);
            while (selectedPreset == previousPreset)
            {
                selectedPreset = UnityEngine.Random.Range(0, TargetPositions.Count);
            }
            for (int i = 0; i != Targets.Length; i++)
            {
                if (Targets[i] == null || Targets[i].GetComponent<TargetController>().shouldDestroy)
                {
                    GameObject targetToDelete = Targets[i];
                    LimbType lt = (LimbType)(i + 1);
                    Targets[i] = Instantiate(TargetPrefab);
                    if (targetToDelete != null)
                    {
                        Destroy(targetToDelete);
                    }
                    Targets[i].name = "Target" + lt;
                    TargetController tc = Targets[i].GetComponent<TargetController>();
                    tc.transform.position = TargetPositions[selectedPreset][i] - new Vector3(0,1.8f);
                    tc.TargetLimbType = lt;
                    tc.shouldRandomize = false;
                }
            }
        }
        else
        {
            for (int i = 0; i != Targets.Length; i++)
            {
                if (Targets[i] == null || Targets[i].GetComponent<TargetController>().shouldDestroy)
                {
                    LimbType lt = (LimbType)(i + 1);
                    Targets[i] = Instantiate(TargetPrefab);
                    Targets[i].name = "Target" + lt;
                    TargetController tc = Targets[i].GetComponent<TargetController>();
                    tc.TargetLimbType = lt;
                    tc.shouldRandomize = true;
                }
            }
        }
    }

    bool CheckIfAllTargetsDestroyed()
    {
        for (int i = 0; i != Targets.Length; i++)
        {
            if (!Targets[i].GetComponent<TargetController>().shouldDestroy)
                return false;
        }
        score+=3;
        return true;
    }

    void TriggerTargets()
    {
        bool success = true;
        for (int i = 0; i != Targets.Length; i++)
        {
            success &= Targets[i].GetComponent<TargetController>().IsTriggered();
        }
        for (int i = 0; i != Targets.Length; i++)
        {
            if (success)
            {
                Targets[i].GetComponent<TargetController>().StartProgressBar();
            }
            else
            {
                Targets[i].GetComponent<TargetController>().StopProgressBar();
            }
        }
    }
}
