using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destroyer : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D projectile)
    {
        if (projectile.CompareTag("PlayerProjectile") || projectile.CompareTag("EnemyProjectile"))
        {
            Destroy(projectile.gameObject);
        }
    }
}
