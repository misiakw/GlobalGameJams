using System.Collections;

using TMPro;

using UnityEngine;

public class DialogueBoxScript : MonoBehaviour
{
    public TextMeshProUGUI textComponent;
    private string[] lines = new string[]
    {
        "My nikogo nie dyskryminujemy, chłopi i chłopki po równo praw nie mają.",
        "Często powtarzam moim chłopom — nie głosujesz, nie masz prawa narzekać.",
        "Jesteś swój chłop... Tak się tylko mówi. Oczywiście jesteś mój chłop!\r\n",
        "Ja za darmo nic nie dostałem, wszystko sam musiałem odziedziczyć",
        "Rozmiar nie ma znaczenia. Tak mówią ci z małymi wsiami.",
        "Dostaniemy zapłatę za pańszczyznę? Idź się wyspowiadać z tego pomysłu.",
        "Człowiek dochodzi do czegoś ciężką pracą swoich chłopów, to zabrać mu! Taka mentalność.",
        "Metoda marchewki i kija. Marchewka była dla wołu, a kij dla chłopa, jak się lenił."
    };

    public float textSpeed;

    private int index;
    private float timer;
    private float timer_wait = 5f;
    void Start()
    {
        textComponent.text = string.Empty;
        StartDialogue();
        Dialog();
    }

    void Update()
    {
        timer += Time.deltaTime;
        if (timer > timer_wait)
        {
            Dialog();
            timer = 0f;
        }
    }
    void Dialog()
    {
        if (textComponent.text == lines[index])
        {
            NextLine();
        }
        else
        {
            StopAllCoroutines();
            textComponent.text = lines[index];
        }
    }
    void StartDialogue()
    {
        index = 0;
        StartCoroutine(TypeLineWithDelay());
    }

    IEnumerator TypeLineWithDelay()
    {
        foreach (var line in lines)
        {
            textComponent.text = string.Empty;

            foreach (var c in line.ToCharArray())
            {
                textComponent.text += c;
                yield return new WaitForSeconds(textSpeed);
            }
        }

        gameObject.SetActive(false);
    }

    void NextLine()
    {
        if (index < lines.Length - 1)
        {
            index++;
        }
        else
        {
            gameObject.SetActive(false);
        }
    }
}

