using System.Collections;
using System.Collections.Generic;

using TMPro;

using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public float speed = 5f;

    public float min_Y, max_Y;

    [SerializeField]
    private GameObject player_Bullet;
    [SerializeField]
    private Transform attack_Point;

    private float current_Attack_Timer;
    public float attack_Timer = 0.05f;
    public float attack_Delay = 0.2f;
    public int max_Bullets = 6;
    public float bullet_Delay_timer = 0f;

    private float current_Max_Attack_Timer = 6f;
    private bool canAttack =true;
    private int max_Health = 3;
    private int remainingBullets;
    public GameObject bulletsText;
    public float yMin = -5f;
    public float yMax = 5f;

    public GameObject enemyPrefab;

    [SerializeField]
    private GameObject[] eggs;

    [SerializeField]
    private GameObject[] healths;

    public GameObject Orchestrator;

    public float spawnXPosition = 11f;
    void Attack_1()
    {
        attack_Timer += Time.deltaTime;
        bullet_Delay_timer += Time.deltaTime;
        if (remainingBullets == 0)
        {
             if (bullet_Delay_timer > current_Max_Attack_Timer)
                 {
                    canAttack = true;
                    remainingBullets = max_Bullets;
                for (int i = 0; i < max_Bullets; i++)
                {
                    eggs[i].gameObject.SetActive(true);
                }
                    bullet_Delay_timer = 0f;
                 }
        }
        //if (attack_Timer > current_Attack_Timer)
        //{
        //    canAttack = true;
        //}
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (canAttack && remainingBullets > 0)
            {
                //canAttack = false;
                attack_Timer = 0f;
                remainingBullets--;
                eggs[remainingBullets].gameObject.SetActive(false);
                Instantiate(player_Bullet, attack_Point.position, Quaternion.identity);
            }
        }
    }
        

    void UpdateHealthText()
    {
    }

    void Start()
    {
        current_Attack_Timer = attack_Timer;
        remainingBullets = max_Bullets;

        bulletsText.GetComponent<TextMeshProUGUI>().text = "Bullets: ";

        for (int i = 0; i < max_Bullets; i++)
        {
            eggs[i].gameObject.SetActive(true);
        }
        for(int i = 0; i<=2; i++ )
        {
            healths[i].gameObject.SetActive(true);
        }
        UpdateHealthText();
    }


    void Update()
    {
        if (Orchestrator.GetComponent<ArcanoidOrchestrator>().IsRunning)
        {
            MovePlayer();
            Attack_1();
            Endgame();
        }

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Enemy"))
         {
            max_Health--;
            if (max_Health >= 0)
            {
                healths[max_Health].gameObject.SetActive(false);
            }
            UpdateHealthText();
            Vector3 spawnPosition = new Vector3(spawnXPosition, Random.Range(yMin, yMax), transform.position.z);
            Instantiate(collision.gameObject, spawnPosition, Quaternion.identity);
            Destroy(collision.gameObject);
        }
        if (collision.CompareTag("EnemyBullet"))
        {
            max_Health--;
            if (max_Health >= 0)
            {
                healths[max_Health].gameObject.SetActive(false);
            }
            UpdateHealthText();
            Destroy(collision.gameObject);
        }
    }

    private void Endgame()
    {
        if (!Orchestrator.GetComponent<ArcanoidOrchestrator>().IsRunning || max_Health <= 0)
        {
            Orchestrator.GetComponent<ArcanoidOrchestrator>().IsRunning = false;
            if(max_Health <=0)
                Orchestrator.GetComponent<ArcanoidOrchestrator>().isSuccess = false;
        }
    }

    void MovePlayer()
    {
        if(Input.GetAxisRaw("Vertical")>0f)
        {
            Vector3 temp = transform.position;
            temp.y += speed * Time.deltaTime;
            
            if(temp.y > max_Y)
            {
                temp.y = max_Y;
            }

            transform.position = temp;
        }
        else if (Input.GetAxisRaw("Vertical") < 0f)
        {
            Vector3 temp = transform.position;
            temp.y -= speed * Time.deltaTime;
            if( temp.y < min_Y)
            {
                temp.y = min_Y;
            }
            transform.position = temp;
        }
    }

}
