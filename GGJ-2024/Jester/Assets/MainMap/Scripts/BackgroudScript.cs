using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroudScript : MonoBehaviour
{
    public GameObject TopOverlay;
    public GameObject BottomOverlay;
    public GameObject LeftOverlay;
    public GameObject RightOverlay;
    // Start is called before the first frame update
    void Start()
    {
        if(CrossSceneStorage.IsArkanoidComplete)
            TopOverlay.SetActive(false);
        if(CrossSceneStorage.IsNineStepsComplete) 
            BottomOverlay.SetActive(false);
        if(CrossSceneStorage.IsKinectComplete)
            RightOverlay.SetActive(false);
        if(CrossSceneStorage.IsVixaComplete)
            LeftOverlay.SetActive(false);
    }
}
