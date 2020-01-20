using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Initialization : MonoBehaviour
{
    // Start is called before the first frame update
    private void Awake() {
        Screen.SetResolution(1280, 720, FullScreenMode.Windowed);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
