using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundController : MonoBehaviour
{
    public GameObject background1;
    public GameObject background2;
    private void FixedUpdate()
    {
        if (!GameManager.instance.inGame)
            return;
        background1.transform.position -= new Vector3(0f, 0.1f, 0f);
        background2.transform.position -= new Vector3(0f, 0.1f, 0f);
        if (background2.transform.position.y < 0)
        {
            background1.transform.position += new Vector3(0f, 22.5f, 0f);
            var tmp = background1;
            background1 = background2;
            background2 = tmp;
        }
    }
}

