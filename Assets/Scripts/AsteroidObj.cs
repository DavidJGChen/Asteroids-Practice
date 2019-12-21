using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidObj : MonoBehaviour
{
    public Asteroid asteroid;
    void Start() {
        asteroid = transform.parent.GetComponent<Asteroid>();
    }
    void OnCollisionEnter2D(Collision2D other) {
        if (other.gameObject.CompareTag("Bullet")) {
            asteroid.BulletDamage();
        }
    }
}
