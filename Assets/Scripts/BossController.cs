using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossController : MonoBehaviour
{

    private float speed = 2.5f;
    private Rigidbody2D rb2d;
    public GameObject bossProjectile;
    public GameObject projectile;
    private float minFireRateTime = 0.1f;
    private float maxFireRateTime = 1.5f;
    private float baseFireWaitTime = 0.2f;
    private float shootingTime = 2f;
    private float timer = 0f;
    private int enemyHealth;
    public GameObject explosion;
    private bool isShooting;

    void Start()
    {
        enemyHealth = 10;
        isShooting = false;
        rb2d = GetComponent<Rigidbody2D>();
        rb2d.velocity = new Vector2(1, 0) * speed;
        baseFireWaitTime = baseFireWaitTime + UnityEngine.Random.Range(minFireRateTime, maxFireRateTime);
    }

    void Update()
    {
        if (!GameManager.instance.inGame)
        {
            rb2d.velocity = Vector2.zero;
            return;
        }
        timer += Time.deltaTime;
        if (UnityEngine.Random.Range(0.0f, 10.0f) < 1.0f)
            if (!isShooting)
                StartCoroutine("ShootNormalProjectile");
        if(isShooting)
            Instantiate(projectile, transform.position, Quaternion.identity);
        if (timer > baseFireWaitTime)
        {

            Shoot();
            timer = 0f;
        }

    }
    IEnumerator ShootNormalProjectile()
    {
        yield return new WaitForSeconds(shootingTime);
        isShooting = true;
        
        yield return new WaitForSeconds(shootingTime-1f);

        isShooting = false;
    }
 
    
    private void Shoot()
    {
        if (UnityEngine.Random.Range(0f, 1.1f) < 1f)
        {
            baseFireWaitTime = baseFireWaitTime + UnityEngine.Random.Range(minFireRateTime, maxFireRateTime);
            Instantiate(bossProjectile, transform.position, Quaternion.identity);
            Instantiate(bossProjectile, new Vector2(transform.position.x + 0.2f,transform.position.y), Quaternion.identity);
            Instantiate(bossProjectile, new Vector2(transform.position.x - 0.2f, transform.position.y), Quaternion.identity);
        }


    }

    void ChangeDirection(int direction)
    {
        Vector2 newVelocity = rb2d.velocity;
        newVelocity.x = speed * direction;
        rb2d.velocity = newVelocity;
    }
    //void ChangeHeight()
    //{
    //    Vector2 newVelocity = rb2d.velocity;
    //    newVelocity.y = speed * direction;
    //    rb2d.velocity = newVelocity;
    //}

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.name == "Left")
        {
            ChangeDirection(1);
        }
        if (collision.gameObject.name == "Right")
        {
            ChangeDirection(-1);
        }
        if (collision.gameObject.name == "Player")
        {
            GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;
            GetComponent<Animator>().enabled = false;
            GameManager.instance.GameOver();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("PlayerProjectile"))
        {
            --enemyHealth;

            if (enemyHealth < 0)
            {
                Destroy(gameObject);
                GameManager.instance.WinLevel();
            }

        }

    }
}
