using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dawid {
public class UIManager : MonoBehaviour
{
    private static UIManager manager;
    public static UIManager Manager {
        get {
            if (manager == null) {
                manager = FindObjectOfType<UIManager>();
            }
            return manager;
        }
    }
}
}