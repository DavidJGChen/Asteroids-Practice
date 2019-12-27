using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dawid {
public class Bullet1 : MonoBehaviour
{
    private WrapAround1 wrapAround;
    private Rigidbody2D rb2D;
    private SpriteRenderer spriteRenderer;
    public float timeToDeath;
    public float fireForce;
    private float deathTimer;
    private void Awake() {
        spriteRenderer = GetComponent<SpriteRenderer>();

        wrapAround = GetComponent<WrapAround1>();
        wrapAround.CreateGhosts();

        rb2D = GetComponent<Rigidbody2D>();
    }
    private void OnEnable() {
        deathTimer = timeToDeath;
    }

    private void FixedUpdate() {
        deathTimer -= Time.deltaTime;
        if (deathTimer < 0) {
            ObjectPooler.SharedInstance.ReturnToPool(gameObject);
        }
    }

    public void SetStartingOrientation(Vector2 pos, Quaternion rot) {
        this.transform.position = pos;
        this.transform.rotation = rot;
    }

    public void ChangeToProjectile(Projectile proj) {
        UpdateProjectile(proj);
    }

    public void Shoot(Vector2 direction) {
        rb2D.AddForce(direction * fireForce, ForceMode2D.Impulse);
    }

    private void UpdateProjectile(Projectile proj) {
        spriteRenderer.sprite = proj.projectileSprite;
        rb2D.mass = proj.mass;
        timeToDeath = proj.timeToDeath;
        fireForce = proj.fireForce;
    }
}
}