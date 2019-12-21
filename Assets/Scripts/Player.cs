using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dawid {
public class Player : WrapAround
{
    public GameObject bullet;
    private Rigidbody2D rb2D;
    private Vector2 velocity;
    private Vector2 velocitySmoothing;
    private float angularVelocitySmoothing;
    private float rotateAmount;
    private float thrust;
    public float moveSpeed = 8f;
    public float rotateSpeed = 360f;
    public float timeToAccel = 1f;
    private bool inControl;

    protected override void OnAwake() {}
    protected override void OnStart() {
        PositionGhosts();
        rb2D = currObject.GetComponent<Rigidbody2D>();
        rotateAmount = 0f;
        thrust = 0f;
    }

    protected override void OnUpdate() {
        rotateAmount = Input.GetAxisRaw("Horizontal");
        thrust = Input.GetAxisRaw("Vertical") == 1f ? 1f : 0f;

        inControl = rotateAmount != 0 || thrust != 0;

        if (Input.GetKeyDown(KeyCode.Space)) {
            var thingy = Instantiate(bullet, currObject.position + currObject.up * 0.25f, Quaternion.identity);
            thingy.GetComponent<Bullet>().Direction = currObject.up;
        }
    }

    // Update is called once per frame
    protected override void OnFixedUpdate()
    {
        Vector2 targetVelocity = currObject.up * moveSpeed * thrust;
        rb2D.velocity = Vector2.SmoothDamp(rb2D.velocity, targetVelocity, ref velocitySmoothing, timeToAccel);
        currObject.Rotate(0f, 0f, -1 * rotateAmount * rotateSpeed * Time.deltaTime);
        if (inControl) {
            rb2D.angularDrag = 0.95f;
        }
        else {
            rb2D.angularDrag = 0.4f;
        }
        if (Mathf.Abs(rb2D.angularVelocity) < 0.5) rb2D.angularVelocity = 0;
    }
}
}
