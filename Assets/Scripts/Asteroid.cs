using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dawid {
public class Asteroid : MonoBehaviour
{
    public AsteroidData defaultAsteroid;
    private AsteroidData currAsteroid;
    private WrapAround1 wrapAround;
    private Rigidbody2D rb2D;
    private SpriteRenderer spriteRenderer;
    private int health;
    private int currHealth;
    private float minForce;
    private float maxForce;
    private AsteroidData nextAsteroid;
    private int splitAmount;

    private void Awake() {
        spriteRenderer = GetComponent<SpriteRenderer>();

        wrapAround = GetComponent<WrapAround1>();

        rb2D = GetComponent<Rigidbody2D>();

        UpdateAsteroid(defaultAsteroid);

        wrapAround.CreateGhosts();
    }

    private void OnEnable() {
        // UpdateAsteroid(defaultAsteroid);
    }

    private void FixedUpdate() {
        if (currHealth <= 0) {
            OnDestruction();
        }
    }
    public void SetStartingOrientation(Vector2 pos, Quaternion rot) {
        this.transform.position = pos;
        this.transform.rotation = rot;
    }

    private void UpdateAsteroid(AsteroidData asteroid) {
        spriteRenderer.sprite = asteroid.asteroidSprites[Random.Range(0,asteroid.asteroidSprites.Length)];
        rb2D.mass = asteroid.mass;
        health = asteroid.health;
        currHealth = health;

        minForce = asteroid.minForce;
        maxForce = asteroid.maxForce;
        nextAsteroid = asteroid.nextAsteroid;
        splitAmount = asteroid.splitAmount;

        Destroy(GetComponent<PolygonCollider2D>());
        gameObject.AddComponent<PolygonCollider2D>();

        currAsteroid = asteroid;
    }

    public void HideAsteroid() {
        wrapAround.HideObject();
    }
    public void ApplyRandomForce() {
        var randomVector = Random.insideUnitCircle.normalized;
        rb2D.AddForce(randomVector * Random.Range(minForce, maxForce), ForceMode2D.Impulse);
    }

    private void TakeBulletDamage() {
        currHealth--;
    }

    private void OnDestruction() {
        if (nextAsteroid != null && splitAmount > 0) {
            Vector2[] offsets;
            CalculateNextAsteroidOffset(0.25f, out offsets);
            for (int i = 0; i < splitAmount; i++) {
                var splitAsteroid = ObjectPooler.SharedInstance.GetPooledObject(gameObject.tag);
                var splitAsteroidScript = splitAsteroid.GetComponent<Asteroid>();
                splitAsteroid.SetActive(true);
                splitAsteroidScript.UpdateAsteroid(nextAsteroid);
                splitAsteroidScript.SetStartingOrientation(new Vector2(this.transform.position.x, this.transform.position.y) + offsets[i], this.transform.rotation);
                var randomVector = Random.insideUnitCircle.normalized * Random.Range(nextAsteroid.minForce, nextAsteroid.minForce);
                splitAsteroid.GetComponent<Rigidbody2D>().AddForce(randomVector, ForceMode2D.Impulse);
            }
        }

        ObjectPooler.SharedInstance.ReturnToPool(gameObject);
    }

    private void CalculateNextAsteroidOffset(float magnitude, out Vector2[] offsets) {
        offsets = new Vector2[splitAmount];
        float incrementalAngle = Mathf.PI * 2 * 1 / splitAmount;
        float angle = 0f;
        for (int i = 0; i < splitAmount; i++) {
            offsets[i] = new Vector2(Mathf.Cos(angle) * magnitude, Mathf.Sin(angle) * magnitude);
            angle += incrementalAngle;
        }
    }

    private void OnCollisionEnter2D(Collision2D other) {
        if (other.gameObject.CompareTag("Bullet")) {
            TakeBulletDamage();
        }
        if (other.gameObject.CompareTag("Ship")) {
            TakeBulletDamage();
        }
    }
}
}