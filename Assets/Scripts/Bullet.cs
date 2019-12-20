using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private Vector2 direction;
    public float moveSpeed = 40f;

    private float timeToDeath = 2f;

    private void FixedUpdate() {
        transform.Translate(direction * moveSpeed * Time.deltaTime);
        timeToDeath -= Time.deltaTime;
        if (timeToDeath < 0) {
            Destroy(gameObject);
        }
    }

    public Vector2 Direction {
        get { return direction; }
        set { 
            direction = value; 
        }
    }
}
