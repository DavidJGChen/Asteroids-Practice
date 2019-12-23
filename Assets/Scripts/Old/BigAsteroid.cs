using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BigAsteroid : Asteroid
{
    protected override void OnAwake() {}
    // Start is called before the first frame update
    protected override void OnStart()
    {
        base.OnStart();
        Vector2 randomVector = Random.insideUnitCircle.normalized;
        rb2D.AddForce(randomVector * Random.Range(100f,500f), ForceMode2D.Impulse);
        health = 4;
        SetReveal(false);
    }

    // Update is called once per frame
    protected override void OnDestruction() {
        Instantiate(nextAsteroid, currObject.position + new Vector3(-0.25f,.25f, 0), Quaternion.identity);
        Instantiate(nextAsteroid, currObject.position + new Vector3(0.25f,.25f, 0), Quaternion.identity);
        Instantiate(nextAsteroid, currObject.position + new Vector3(0,-.25f, 0), Quaternion.identity);
        Debug.Log("weeee");
        DestroyAll();
        Destroy(gameObject);
    }
}
