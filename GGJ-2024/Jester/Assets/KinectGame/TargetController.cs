using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static Assets.KinectGame.Enums;

public class TargetController : MonoBehaviour
{
    public LimbType TargetLimbType;
    public float timeToScore = 1f;
    public GameObject ProgressBar;
    public bool shouldRandomize = false;
    
    private Guid triggerGuid;
    private float timeFromTrigger = 0f;
    private bool startProgressBar = false;
    public bool shouldDestroy = false;

    public bool IsTriggered()
    {
        return triggerGuid != Guid.Empty;
    }

    public void StartProgressBar()
    {
        StartCoroutine(Collect(triggerGuid));
    }

    public void StopProgressBar()
    {
        timeFromTrigger = 0f;
        ProgressBar.GetComponent<Image>().fillAmount = 0;
        startProgressBar = false;
        shouldDestroy = false;
    }

    // Start is called before the first frame update
    void Start()
    {
        if (shouldRandomize)
        {
            this.transform.position = new Vector3(UnityEngine.Random.Range(-2f, 3), UnityEngine.Random.Range(-2f, 3), 0);
        }
        this.transform.localScale = Vector3.one * 0.5f;
        Color color = LimbTypeToColor(TargetLimbType);
        if (this.GetComponent<MeshRenderer>() != null)
        {
            this.GetComponent<MeshRenderer>().material.color = color;
        }
        else if(this.GetComponent<SpriteRenderer>() != null)
        {
            this.GetComponent<SpriteRenderer>().color = color;
            this.GetComponentInChildren<TextMeshPro>().text = LimbTypeToString(TargetLimbType);
            ProgressBar.GetComponent<Image>().color = color;
        }
    }

    string LimbTypeToString(LimbType lt)
    {
        switch (lt)
        {
            case LimbType.LeftHand:
                return "LH";
            case LimbType.RightHand:
                return "RH";
            case LimbType.LeftFoot:
                return "LF";
            case LimbType.RightFoot:
                return "RF";
            default:
                return "U";
        }
    }

    Color LimbTypeToColor(LimbType lt)
    {
        switch (lt)
        {
            case LimbType.LeftHand:
                return Color.red;
            case LimbType.RightHand:
                return Color.blue;
            case LimbType.LeftFoot:
                return Color.green;
            case LimbType.RightFoot:
                return Color.yellow;
            default:
                return Color.black;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (startProgressBar)
        {
            timeFromTrigger += Time.deltaTime;
            ProgressBar.GetComponent<Image>().fillAmount = timeFromTrigger / timeToScore;
        }
    }

    IEnumerator Collect(Guid guid)
    {
        startProgressBar = true;
        triggerGuid = guid;
        yield return new WaitForSeconds(timeToScore);
        if (triggerGuid == guid)
        {
            shouldDestroy = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Limb limb = other.gameObject.GetComponent<Limb>();
        if (limb != null && limb.LimbType == TargetLimbType)
        {
            triggerGuid = Guid.NewGuid();
            StartProgressBar();
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        Limb limb = other.gameObject.GetComponent<Limb>();
        if (limb != null && limb.LimbType == TargetLimbType)
        {
            triggerGuid = Guid.Empty;
            StopProgressBar();
        }
    }
}
