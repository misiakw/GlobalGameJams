using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LifeRefresh : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        SpellMoveScript._failsCounter = 3;
    }
}
