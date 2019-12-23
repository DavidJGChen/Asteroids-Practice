using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dawid {
public class Bullet1 : MonoBehaviour
{
    private WrapAround1 wrapAround;
    private Rigidbody2D rb2D;
    private Vector2 direction;
    public float moveSpeed = 20f;
    private float timeToDeath = 0.5f;
    private void Awake() {
        wrapAround = GetComponent<WrapAround1>();
        wrapAround.CreateGhosts();
        rb2D = GetComponent<Rigidbody2D>();
    }
    private void OnEnable() {
        rb2D.AddForce(direction * moveSpeed, ForceMode2D.Impulse);
        timeToDeath = 0.5f;
    }

    private void FixedUpdate() {
        timeToDeath -= Time.deltaTime;
        if (timeToDeath < 0) {
            ObjectPooler.SharedInstance.ReturnToPool(gameObject);
        }
    }

    public Vector2 Direction {
        get { return direction; }
        set { 
            direction = value; 
        }
    }
}
}