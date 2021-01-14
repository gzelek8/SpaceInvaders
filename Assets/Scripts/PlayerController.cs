using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
	public GameObject projectile;
	private float speed = 10;
	private Rigidbody2D rb2d;
	void Start()
	{
		rb2d = GetComponent<Rigidbody2D>();
	}

	void FixedUpdate()
	{
		if (!GameManager.instance.inGame)
			return;
		MoveSpaceship();
		Shoot();
		GameManager.instance.WinLevel();
	}


	void MoveSpaceship()
    {
		float horizontalMove = Input.GetAxisRaw("Horizontal");
		rb2d.velocity = new Vector2(horizontalMove, 0) * speed;
	}

	void Shoot()
    {
		if (Input.GetKeyDown(KeyCode.Space))
		{
			if(GameManager.instance.isTrippleAttack)
            {
				Instantiate(projectile, new Vector2(transform.position.x + 0.3f, transform.position.y + 0.5f), Quaternion.identity);
				Instantiate(projectile, new Vector2(transform.position.x -0.3f, transform.position.y + 0.5f), Quaternion.identity);
			}
			Instantiate(projectile, new Vector2(transform.position.x, transform.position.y + 0.5f), Quaternion.identity);
		}
	}

    private void OnTriggerEnter2D(Collider2D collision)
    {
		if(GameManager.instance.isImmortal && collision.gameObject.CompareTag("EnemyProjectile"))
        {
			Destroy(collision.gameObject);
			GameManager.instance.StopShield();
        }
        if(collision.gameObject.CompareTag("Enemy") && collision.gameObject.CompareTag("EnemyProjectile") && !GameManager.instance.isImmortal)
        {
			print(collision.gameObject.name);
			PlayerDeath();
        }
		if (collision.gameObject.CompareTag("AttackBonus"))
		{
			Destroy(collision.gameObject);
			GameManager.instance.ActiveTripleShot();
			print("elo");
		}
		if (collision.gameObject.CompareTag("ShieldBonus"))
		{
			Destroy(collision.gameObject);
			GameManager.instance.ActiveShield();
		}
	}

	private void PlayerDeath()
    {
		GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;
		GameManager.instance.GameOver();
    }
}