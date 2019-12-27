using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dawid {
public class ObjectPooler : MonoBehaviour
{
    private static ObjectPooler sharedInstance;

    public static ObjectPooler SharedInstance {
        get {
            if (sharedInstance == null) {
                sharedInstance = FindObjectOfType<ObjectPooler>();
            }
            return sharedInstance;
        }
    }

    private Dictionary<string, Stack<GameObject>> pool = new Dictionary<string, Stack<GameObject>>();
    private Dictionary<string, int> poolAmounts = new Dictionary<string, int>();
    private GameObject overflowPool;
    
    private void Start() {
        overflowPool = new GameObject("Overflow Pool");
        overflowPool.transform.parent = this.transform;
    }

    public void CreatePool(GameObject prefab, int amountToPool) {
        string key = prefab.tag;

        if (!pool.ContainsKey(key)) {
            var poolStack = new Stack<GameObject>();
            pool.Add(key, poolStack);

            var poolContainer = new GameObject(prefab.name + " Pool");
            poolContainer.transform.parent = this.transform;

            for (int i = 0; i < amountToPool; i++) {
                var obj = Instantiate(prefab);
                obj.SetActive(false);
                poolStack.Push(obj);
                obj.transform.parent = poolContainer.transform;
            }

            poolAmounts.Add(key, amountToPool);
        }
    }

    public GameObject GetPooledObject(GameObject prefab) {
        Stack<GameObject> poolStack;
        if (!pool.TryGetValue(prefab.tag, out poolStack)) return null;
        if (poolStack.Count > 0 && !poolStack.Peek().activeInHierarchy) {
            return poolStack.Pop();
        }
        var obj = Instantiate(prefab);
        obj.SetActive(false);
        obj.transform.parent = overflowPool.transform;
        return obj;
    }

    public void ReturnToPool(GameObject obj) {
        if (obj == null) return; // throw exception TODO
        Stack<GameObject> poolStack;
        if (!pool.TryGetValue(obj.tag, out poolStack)) Destroy(obj);
        if (obj.transform.parent == overflowPool.transform) {
            Destroy(obj);
        }
        else {
            obj.SetActive(false);
            poolStack.Push(obj);
        }
    }
}
}