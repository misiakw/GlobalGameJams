using System.Collections;

using TMPro;

using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class EnemyController : MonoBehaviour
{
    public GameObject bulletPrefab;
    public float bulletSpeed = 10f;
    public float bulletLifetime = 2f;
    public float bulletSpawnRate = 1f;
    public float searchRadius = 30f;
    private float current_Attack_Timer;
    public float attack_Timer = 2f;
    private Transform player;
    private Vector3 targetPosition;
    private bool canFire = true;
    public float yMin = -3f;
    public float yMax = 3f;
    public float spawnXPosition = 11f;

    public GameObject score;

    public GameObject Orchestrator;
    public int scoreInt;
    void Start()
    {
        // Set the initial target position
        targetPosition = new Vector3(8f, transform.position.y, transform.position.z);
        // Invoke the method to search for the player periodically
        InvokeRepeating("SearchForPlayer", 0f, 1f / bulletSpawnRate);
    }

    void Update()
    {
        if (Orchestrator.GetComponent<ArcanoidOrchestrator>().IsRunning)
        {
            // Move towards target position
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, Time.deltaTime);
            current_Attack_Timer += Time.deltaTime;
            // Throw bullets at player
            if (attack_Timer < current_Attack_Timer)
            {
                canFire = true;

                if (canFire && player != null)
                {
                    Vector3 direction = (player.transform.position - transform.position).normalized;
                    FireBullet(direction);
                    canFire = false;
                    current_Attack_Timer = 0f;
                }
            }
        }
    }
    //private void UpdateScore()
    //{
    //    if (score != null)
    //    {
    //        score.GetComponent<TextMeshProUGUI>().text = $"Score : {scoreInt}";
    //    }
    //}
    void SearchForPlayer()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Bullet"))
        {
            Destroy(collision.gameObject);
            Vector3 spawnPosition = new Vector3(spawnXPosition, Random.Range(yMin, yMax), transform.position.z);
            Vector3 spawnPosition2 = new Vector3(spawnXPosition, Random.Range(yMin, yMax), transform.position.z);
            scoreInt += 10;
            var newEnemy = Instantiate(this.gameObject, spawnPosition, Quaternion.identity);
            newEnemy.GetComponent<EnemyController>().Orchestrator = Orchestrator;
            newEnemy = Instantiate(this.gameObject, spawnPosition2, Quaternion.identity);
            newEnemy.GetComponent<EnemyController>().Orchestrator = Orchestrator;

            var targetPosition = new Vector3(8f, transform.position.y, transform.position.z);
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, Time.deltaTime);
            Destroy(this.gameObject);
        }
    }
    void FireBullet(Vector3 direction)
    {
        // Create enemy bullet
        GameObject bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
        bullet.GetComponent<Rigidbody2D>().velocity = direction * bulletSpeed;
    }
}