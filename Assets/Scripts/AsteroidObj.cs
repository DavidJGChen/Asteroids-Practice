using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidObj : MonoBehaviour
{
    void OnCollisionEnter2D(Collision2D other) {
        Debug.Log("what the shit");
        if (other.gameObject.CompareTag("Bullet")) {
            transform.parent.GetComponent<Asteroid>().Owie();
        }
    }
}
