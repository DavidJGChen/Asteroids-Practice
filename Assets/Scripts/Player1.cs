using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dawid {
public class Player1 : MonoBehaviour
{
    private WrapAround1 wrapAround;
    private Rigidbody2D rb2D;
    private Vector2 velocity;
    private Vector2 velocitySmoothing;
    private float angularVelocitySmoothing;
    private float rotateAmount;
    private float thrust;
    public float moveSpeed = 8f;
    public float fireForce = 20f;
    public float rotateSpeed = 360f;
    public float timeToAccel = 1f;
    private bool inControl;
    public int health = 5;
    public float owieTimer;

    private void Awake() {
        wrapAround = GetComponent<WrapAround1>();
        wrapAround.CreateGhosts();
        rb2D = GetComponent<Rigidbody2D>();
    }

    private void Start() {
        rotateAmount = 0f;
        thrust = 0f;
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
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        if (health <= 0) {
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

    private void TakeDamage() {
        if (owieTimer <= 0){
            health--;
            GetComponent<SpriteRenderer>().color = Color.red;
            owieTimer = 2f;
        }
    }
}
}