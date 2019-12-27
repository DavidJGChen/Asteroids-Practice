using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dawid {
public class Player1 : MonoBehaviour
{
    public ShipData[] ships;
    private int currShip;
    public GameObject projectilePrefab;
    private ProjectileData currProjectile;

    private WrapAround1 wrapAround;
    private Rigidbody2D rb2D;
    private SpriteRenderer spriteRenderer;
    private Vector2 velocity;
    private Vector2 velocitySmoothing;
    private float angularVelocitySmoothing;
    private float rotateAmount;
    private float thrust;
    private float moveSpeed;
    private float rotateSpeed;
    private float timeToAccel;
    private bool inControl;
    private int health;
    private int currHealth;
    private float owieTimer;

    private bool gameStarted = false;

    private void Awake() {
        wrapAround = GetComponent<WrapAround1>();

        rb2D = GetComponent<Rigidbody2D>();

        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Start() {
        UpdateShip(0);
        currShip = 0;

        ObjectPooler.SharedInstance.CreatePool(projectilePrefab, 3); // Create Projectiles

        wrapAround.CreateGhosts();
        rotateAmount = 0f;
        thrust = 0f;
        gameStarted = true;
    }

    private void Update() {
        rotateAmount = Input.GetAxisRaw("Horizontal");
        thrust = Input.GetAxisRaw("Vertical") == 1f ? 1f : 0f;

        inControl = rotateAmount != 0 || thrust != 0;

        if (Input.GetKeyDown(KeyCode.Space)) {
            Shoot();
        }
        if (Input.GetKeyDown(KeyCode.Tab)) {
            currShip = (currShip + 1) % ships.Length;
            UpdateShip(currShip);
        }
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        if (currHealth <= 0) {
            wrapAround.DestroyGhosts();
            Destroy(gameObject);
        }
        if (owieTimer > 0) {
            owieTimer -= Time.deltaTime;
            if (owieTimer <= 0) {
                spriteRenderer.color = Color.white;
            }
        }
        Vector2 targetVelocity = this.transform.up * moveSpeed * thrust;
        rb2D.velocity = Vector2.SmoothDamp(rb2D.velocity, targetVelocity, ref velocitySmoothing, timeToAccel);
        this.transform.Rotate(0f, 0f, -1 * rotateAmount * rotateSpeed * Time.deltaTime);
        if (inControl) {
            rb2D.angularDrag = 0.95f;
        }
        else {
            rb2D.angularDrag = 0.4f;
        }
        if (Mathf.Abs(rb2D.angularVelocity) < 0.5) rb2D.angularVelocity = 0;
    }

    private void UpdateShip(int i) {
        spriteRenderer.sprite = ships[i].shipSprite;
        moveSpeed = ships[i].moveSpeed;
        rb2D.mass = ships[i].mass;
        rotateSpeed = ships[i].rotateSpeed;
        timeToAccel = ships[i].timeToAccel;
        health = ships[i].health;
        if (!gameStarted) {
            currHealth = health;
        }

        Destroy(GetComponent<PolygonCollider2D>());
        gameObject.AddComponent<PolygonCollider2D>();

        UpdateProjectile(ships[i].defaultProjectile);
    }

    private void UpdateProjectile(ProjectileData proj) {
        currProjectile = proj;
    }

    private void Shoot() {
        var projInstance = ObjectPooler.SharedInstance.GetPooledObject(projectilePrefab);

        var projScript = projInstance.GetComponent<Bullet1>();

        projScript.ChangeToProjectile(currProjectile);

        var projectileSpawn = this.transform.position + this.transform.up * 0.25f;

        projScript.SetStartingOrientation(projectileSpawn, this.transform.rotation);

        projInstance.SetActive(true);

        projScript.Shoot(this.transform.up);
    }

    private void TakeDamage() {
        if (owieTimer <= 0){
            currHealth--;
            spriteRenderer.color = Color.red;
            owieTimer = 2f;
        }
    }

    private void OnCollisionEnter2D(Collision2D other) {
        if (other.gameObject.CompareTag("Asteroid")) {
            TakeDamage();
        }
    }
}
}