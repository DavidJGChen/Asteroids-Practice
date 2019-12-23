using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmallAsteroid : Asteroid
{

    protected override void OnAwake() {}
    // Start is called before the first frame update
    protected override void OnStart()
    {
        base.OnStart();
        Vector2 randomVector = Random.insideUnitCircle.normalized;
        rb2D.AddForce(randomVector * Random.Range(100f,500f), ForceMode2D.Impulse);
        health = 1;
    }

    // Update is called once per frame
    protected override void OnDestruction() {
        DestroyAll();
        Destroy(gameObject);
    }
}
