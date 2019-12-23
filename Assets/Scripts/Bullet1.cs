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
    }
    private void OnEnable() {
        timeToDeath = 0.5f;
    }

    private void FixedUpdate() {
        timeToDeath -= Time.deltaTime;
        if (timeToDeath < 0) {
            ObjectPooler.SharedInstance.ReturnToPool(gameObject);
        }
    }
}
}