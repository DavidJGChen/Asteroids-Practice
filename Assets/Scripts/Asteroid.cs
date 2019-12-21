using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Asteroid : WrapAround
{
    protected Rigidbody2D rb2D;
    public float moveSpeed = 100f;
    public GameObject nextAsteroid;
    protected int health;

    protected override void OnStart() {
        rb2D = currObject.GetComponent<Rigidbody2D>();
    }

    protected override void OnUpdate() {}

    protected override void OnFixedUpdate() {
        if (health <= 0) {
            OnDestruction();
        }
    }

    protected abstract void OnDestruction();

    public void BulletDamage() {
        health--;
    }
}
