using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dawid {
public class Player : MonoBehaviour
{
    public GameObject bullet;
    private Rigidbody2D rb2D;
    private Vector2 velocity;
    private Vector2 velocitySmoothing;
    private float rotateAmount;
    private float thrust;
    public float moveSpeed = 8f;
    public float rotateSpeed = 360f;
    public float timeToAccel = 1f;
    private void Awake() {
        rb2D = GetComponent<Rigidbody2D>();
    }

    private void Start() {
        rotateAmount = 0f;
        thrust = 0f;
    }

    void Update() {
        rotateAmount = Input.GetAxisRaw("Horizontal");
        thrust = Input.GetAxisRaw("Vertical") == 1f ? 1f : 0f;

        if (Input.GetKeyDown(KeyCode.Space)) {
            var thingy = Instantiate(bullet, transform.position + transform.up * 0.5f, new Quaternion());
            thingy.GetComponent<Bullet>().Direction = transform.up;
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        transform.Rotate(0f, 0f, -1 * rotateAmount * rotateSpeed * Time.deltaTime);
        Vector2 targetVelocity = transform.up * moveSpeed * thrust;
        // Debug.Log(transform.up);
        rb2D.velocity = Vector2.SmoothDamp(rb2D.velocity, targetVelocity, ref velocitySmoothing, timeToAccel);
        // Debug.Log(rigidbody2D.velocity);
    }
}
}
