using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BonusController : MonoBehaviour
{
    private float speed;
    private Rigidbody2D rb2d;
    private int shieldHealth;

    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        speed = 3;
        rb2d.velocity = Vector2.down * speed;
    }



}
