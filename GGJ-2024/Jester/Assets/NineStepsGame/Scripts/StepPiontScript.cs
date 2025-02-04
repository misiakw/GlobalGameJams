using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StepPiontScript : MonoBehaviour
{
    public GameObject BaseSpriteObj;

    private SpriteRenderer renderer;
    private float timer = 10;
    public float Points;
    public float WaitTime = 0.6f;
    // Start is called before the first frame update
    void Start()
    {
        renderer = BaseSpriteObj.GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;

        //-0.5 => -0.15
        if (timer >= WaitTime*-1 && timer < -0) //R0. G0. B1
        {
            var part = (timer + WaitTime) / WaitTime;
            var color = new Color(0, part, (1 - part));
            renderer.color = color;
        }
        else if (timer >= 0 && timer <= WaitTime) //R1 G0 B0
        {
            var part = (timer) / WaitTime;
            var color = new Color(part, (1-part), 0);
            renderer.color = color;
        }
        else
        {
            renderer.color = Color.white;
        }
    }

    public void StartCounter()
    {
        BaseSpriteObj.SetActive(true);
        timer = -0.75f;
    }

    public void Enter(NineStepsPlayerScript player)
    {
        BaseSpriteObj.SetActive(false);
        int gain = 50 - (int)(Mathf.Abs(timer) * 100);
        if(gain > 0)
        {
            player.IncreaseScore(gain);
        }
    }
}
