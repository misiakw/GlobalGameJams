using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class SickleBullet : MonoBehaviour
{
    public float deactivate_Timer = 10f;

    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (GameObject.Find("GameOrchestrator").GetComponent<ArcanoidOrchestrator>().IsRunning)
        {
            this.transform.Rotate(new Vector3(0, 0, 180 * Time.deltaTime));
        }
        else
        {
            this.GetComponent<Rigidbody2D>().velocity = Vector3.zero;
        }
    }

    private void OnBecameInvisible()
    {
        Destroy(gameObject);
    }

}
