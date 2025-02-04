using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ArcanoidOrchestrator : MonoBehaviour
{
    public bool IsRunning = true;
    public GameObject EndScreen;
    public bool isSuccess = true;

    private const string EndGameKindSucceed = @"
The King:
""Huzzah! You've saved me from the stampede of cursed chaos, dear Jester!\nYour clever tricks and quick thinking have restored order to my kingdom. I shall not forget this day of laughter and rescue!""



Jester:
""I am always at your service my liege, but please do not tease Witches anymore T.T""";
    private const string EndGameKingFailed = @"King:
""You've stopped some of them - great!. Yet, they're still coming. Come Jester, I need you!""";


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!IsRunning)
        {
            EndScreen.GetComponentInChildren<Text>().text = isSuccess ? EndGameKindSucceed : EndGameKingFailed;
            EndScreen.SetActive(true);
            CrossSceneStorage.IsArkanoidComplete = isSuccess;
        }

    }
}
