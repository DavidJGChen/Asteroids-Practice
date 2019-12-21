using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game : MonoBehaviour
{
    float newAsteroidTime = 5f;
    float timer;
    public GameObject asteroid;
    // Start is called before the first frame update
    void Start()
    {
        timer = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (timer > 0) {
            timer -= Time.deltaTime;
        }
        else {
            Instantiate(asteroid, transform);
            timer = newAsteroidTime;
        }
    }
}
