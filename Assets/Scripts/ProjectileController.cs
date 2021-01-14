using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileController : MonoBehaviour
{
    private float speed;
    private Rigidbody2D rb2d;
    public GameObject explosion;
    private float destroyTime = 0.5f;

    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        speed = 3;
        if(gameObject.CompareTag("PlayerProjectile"))
            rb2d.velocity = Vector2.up * speed;
        else if(gameObject.CompareTag("EnemyProjectile"))
            rb2d.velocity = Vector2.down * speed;
    }

    private void Update()
    {
        if (!GameManager.instance.inGame)
        {
            rb2d.velocity = Vector2.zero;
            return;
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.gameObject.CompareTag("Player") && !GameManager.instance.isImmortal)
        {
            DestroyObject(collision);
            GameManager.instance.GameOver();
        }
        if (collision.gameObject.CompareTag("PlayerProjectile"))
        {
            Destroy(gameObject);
            DestroyObject(collision);
        }
        if (collision.gameObject.CompareTag("EnemyProjectile"))
        {
            Destroy(gameObject);
            DestroyObject(collision);
        }
        if (collision.gameObject.CompareTag("Shield"))
        {
            DestroyObject(collision);
        }
    }

    private void DestroyObject(Collider2D collision)
    {
        Destroy(gameObject);
        Destroy(
                Instantiate(explosion,
                            new Vector2(collision.gameObject.transform.position.x, collision.gameObject.transform.position.y),
                            Quaternion.identity),
                        destroyTime);
        Destroy(collision.gameObject);
    }
}
