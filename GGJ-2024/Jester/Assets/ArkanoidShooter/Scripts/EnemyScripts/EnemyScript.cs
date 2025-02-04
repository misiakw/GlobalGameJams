using System.Collections;
using System.Security.Cryptography;

using TMPro;

using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class EnemyScript : MonoBehaviour
{
    public float speed = 5f;
    public float yMin = -5f;
    public float yMax = 5f;
    public float respawnInterval = 5f;
    public float spawnXPosition = 11f;
    public float respawnTime = 5.0f;

    //public GameObject score;
    public static int scoreInt;
    private Transform player;
    private Animator animator;
    public GameObject enemyPrefab;

    public GameObject score;

    public GameObject Orchestrator;
    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (Orchestrator.GetComponent<ArcanoidOrchestrator>().IsRunning)
        {
            Vector3 targetPosition = new Vector2(player.position.x, player.position.y);
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Bullet"))
        {
            Destroy(other.gameObject);
            PlayDestructionAnimation();
        }
        if(CompareTag("Player"))
        {
            Destroy(gameObject);
        }
    }

    private void PlayDestructionAnimation()
    {
        if (animator != null)
        {
            Vector3 spawnPosition = new Vector3(spawnXPosition, Random.Range(yMin, yMax), transform.position.z);
            var newEnemy = Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);
            newEnemy.GetComponent<EnemyScript>().Orchestrator = Orchestrator;
            
        }

        Destroy(gameObject);
    }
}
