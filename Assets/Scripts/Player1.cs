using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dawid {
public class Player1 : MonoBehaviour
{
    public Ship[] ships;
    private int currentShip;

    private WrapAround1 wrapAround;
    private Rigidbody2D rb2D;
    private Vector2 velocity;
    private Vector2 velocitySmoothing;
    private float angularVelocitySmoothing;
    private float rotateAmount;
    private float thrust;
    private float moveSpeed;
    public float fireForce = 20f;
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
    }

    private void Start() {
        UpdateShip(0);
        currentShip = 0;

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
            var bullet = ObjectPooler.SharedInstance.GetPooledObject("Bullet");
            var bulletSpawn = this.transform.position + this.transform.up * 0.25f;
            bullet.transform.position = bulletSpawn;
            bullet.transform.rotation = Quaternion.identity;
            bullet.SetActive(true);

            bullet.GetComponent<Rigidbody2D>().AddForce(this.transform.up * fireForce, ForceMode2D.Impulse);
        }
        if (Input.GetKeyDown(KeyCode.Tab)) {
            currentShip = (currentShip + 1) % ships.Length;
            UpdateShip(currentShip);
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
                GetComponent<SpriteRenderer>().color = Color.white;
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
        GetComponent<SpriteRenderer>().sprite = ships[i].shipSprite;
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
    }

    private void TakeDamage() {
        if (owieTimer <= 0){
            currHealth--;
            GetComponent<SpriteRenderer>().color = Color.red;
            owieTimer = 2f;
        }
    }
}
}