using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dawid {
public class GhostContainer : MonoBehaviour
{
    public static GhostContainer SharedInstance;
    
    private void Awake() {
        SharedInstance = this;
    }
}
}
