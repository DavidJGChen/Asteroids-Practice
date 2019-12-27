using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dawid {
public class Game : MonoBehaviour
{
    float newAsteroidTime = 5f;
    float timer;
    int numAsteroids = 5;
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
            if (numAsteroids > 0) {
                Instantiate(asteroid, transform);
                timer = newAsteroidTime;
                numAsteroids--;
            }
        }

        if (Input.GetKeyDown(KeyCode.Alpha0)) {
            UnityEngine.SceneManagement.SceneManager.LoadScene("Scene1");
        }
    }
}
}