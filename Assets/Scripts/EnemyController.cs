using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{

    private float speed = 1;
    private Rigidbody2D rb2d;
    public GameObject enemyProjectile;
    private float minFireRateTime = 2.0f;
    private float maxFireRateTime = 5.0f;
    private float baseFireWaitTime = 1.0f;
    private float timer = 0f;
    private int enemyHealth;
    private int pointForEnemy;
    public GameObject explosion;
    public List<GameObject> pickUps;

    void Start()
    {
        if (gameObject.CompareTag("Enemy1"))
        {
            enemyHealth = 0;
            pointForEnemy = 10;
        }
        else if (gameObject.CompareTag("Enemy2"))
        {
            enemyHealth = 2;
            pointForEnemy = 25;
        }
        rb2d = GetComponent<Rigidbody2D>();
        rb2d.velocity = new Vector2(1, 0) * speed;
        baseFireWaitTime = baseFireWaitTime + Random.Range(minFireRateTime, maxFireRateTime);
    }

    void Update()
    {
        if (!GameManager.instance.inGame)
        {
            rb2d.velocity = Vector2.zero;
            return;
        }
        timer += Time.deltaTime;
        if (timer > baseFireWaitTime)
        {
            Shoot();
            timer = 0f;
        }

    }

    private void Shoot()
    {
        if (Random.Range(0f, 10f) < 1f)
        {
            baseFireWaitTime = baseFireWaitTime + Random.Range(minFireRateTime, maxFireRateTime);
            Instantiate(enemyProjectile, transform.position, Quaternion.identity);
        }


    }

    void ChangeDirection(int direction)
    {
        Vector2 newVelocity = rb2d.velocity;
        newVelocity.x = speed * direction;
        rb2d.velocity = newVelocity;
    }

    void MoveDown()
    {
        Vector2 position = transform.position;
        position.y -= 1;
        transform.position = position;
    }



    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.name == "Left")
        {
            ChangeDirection(1);
            MoveDown();
        }
        if (collision.gameObject.name == "Right")
        {
            ChangeDirection(-1);
            MoveDown();
        }
        if (collision.gameObject.name == "Player" && !GameManager.instance.isImmortal)
        {
            GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;
            GameManager.instance.GameOver();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("PlayerProjectile"))
        {
            --enemyHealth;
            Destroy(collision.gameObject);
            if (enemyHealth < 0)
            {
                DestroyObject(collision);
                GameManager.instance.ScoreCollected(pointForEnemy);
            }

        }

    }

    private void DestroyObject(Collider2D collision)
    {

        Destroy(
                Instantiate(explosion,
                            new Vector2(collision.gameObject.transform.position.x, collision.gameObject.transform.position.y),
                            Quaternion.identity),
                            0.5f);
        Destroy(gameObject);
        if (Random.Range(0f, 10f) < 1f)
        {
            GameObject bonus = pickUps[UnityEngine.Random.Range(0, 2)];
            Instantiate(bonus, new Vector2(collision.gameObject.transform.position.x, collision.gameObject.transform.position.y), Quaternion.identity);
        }
    }
}
