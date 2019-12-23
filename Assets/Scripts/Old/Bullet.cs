using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : WrapAround
{
    private Vector2 direction;
    private Rigidbody2D rb2D;
    public float moveSpeed = 20f;
    private float timeToDeath = 0.5f;

    protected override void OnAwake() {}
    protected override void OnStart() {
        rb2D = currObject.GetComponent<Rigidbody2D>();
    }
    private void OnEnable() {
        PositionGhosts();
        rb2D.AddForce(direction * moveSpeed, ForceMode2D.Impulse);
        timeToDeath = 0.5f;
    }

    protected override void OnUpdate() {}

    protected override void OnFixedUpdate() {
        // transform.Translate(direction * moveSpeed * Time.deltaTime);
        timeToDeath -= Time.deltaTime;
        if (timeToDeath < 0) {
            gameObject.SetActive(false);
        }
    }

    public Vector2 Direction {
        get { return direction; }
        set { 
            direction = value; 
        }
    }

    // private void OnCollisionEnter(Collision other) {
    //     if (other.gameObject.CompareTag("Asteroid")) {
    //         Debug.Log("shit");
    //         DestroyAll();
    //         Destroy(gameObject);
    //     }
    // }
}
