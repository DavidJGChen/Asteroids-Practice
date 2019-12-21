using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asteroid : WrapAround
{
    private Rigidbody2D rb2D;
    public float moveSpeed = 100f;

    protected override void OnStart() {
        rb2D = currObject.GetComponent<Rigidbody2D>();
        Vector2 randomVector = Random.insideUnitCircle.normalized;
        rb2D.AddForce(randomVector * moveSpeed, ForceMode2D.Impulse);
        Revealed(false);
    }

    protected override void OnUpdate() {}

    protected override void OnFixedUpdate() {
        // Vector2 randomVector = Random.insideUnitCircle.normalized;
        // rb2D.AddForce(randomVector * moveSpeed * 100, ForceMode2D.Impulse);
    }

    public void Owie() {
        DestroyAll();
        Destroy(gameObject);
        Debug.Log("Owie");
    }
}
