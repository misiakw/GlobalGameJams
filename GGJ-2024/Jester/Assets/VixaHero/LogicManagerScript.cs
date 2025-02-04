using UnityEngine;
using UnityEngine.UI;

public class LogicManagerScript : MonoBehaviour
{
    public Text textUI;
    public GameObject gameOver;
    public GameObject gameFailed;
    public Text clock;
    private float timer = 60;
    public GameObject vixaHero;
    private bool _gameFailedFlag = false;

    // Start is called before the first frame update
    void Start()
    {
        timer = 20;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        timer -= Time.deltaTime;
        clock.text = $"{(int)timer}";

        if (!_gameFailedFlag)
        {
            if (timer <= 0)
            {
                GameFinished();
            }
        }
    }

    public void GameFailed()
    {;
        DeactivateSpells();
        vixaHero.SetActive(false);

        gameFailed.SetActive(true);
    }

    public void GameFinished()
    {
        DeactivateSpells();
        vixaHero.SetActive(false);

        gameOver.SetActive(true);
        CrossSceneStorage.IsVixaComplete = true;
    }

    public void UpdateScore(int score)
    {
        textUI.text = score.ToString();

        if (score <=  0)
        {
            _gameFailedFlag = true;
            GameFailed();
        }
    }
    private void DeactivateSpells()
    {
        var spells = GameObject.FindGameObjectsWithTag("SpellTag");

        foreach (GameObject spell in spells)
        {
            spell.SetActive(false);
        }
    }
}