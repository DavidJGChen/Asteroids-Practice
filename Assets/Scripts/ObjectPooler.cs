using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dawid {

[System.Serializable]
public class ObjectPoolItem {
    public GameObject objectToPool;
    public int amountToPool;
}

public class ObjectPooler : MonoBehaviour
{
    public static ObjectPooler SharedInstance;

    public List<ObjectPoolItem> itemsToPool;
    private Dictionary<string, Stack<GameObject>> pool;
    private Dictionary<string, int> poolAmounts;

    private void Awake() {
        SharedInstance = this;
        pool = new Dictionary<string, Stack<GameObject>>();
        poolAmounts = new Dictionary<string, int>();
    }
    private void Start() {
        foreach (var item in itemsToPool) {
            var poolStack = new Stack<GameObject>();
            for (int i = 0; i < item.amountToPool; i++) {
                var obj = Instantiate(item.objectToPool);
                obj.SetActive(false);
                poolStack.Push(obj);
            }
            pool.Add(item.objectToPool.tag, poolStack);
            poolAmounts.Add(item.objectToPool.tag, item.amountToPool);
        }
    }

    public GameObject GetPooledObject(string tag) {
        Stack<GameObject> poolStack;
        if (!pool.TryGetValue(tag, out poolStack)) return null;
        if (poolStack.Count > 0 && !poolStack.Peek().activeInHierarchy) {
            return poolStack.Pop();
        }
        foreach (var item in itemsToPool) {
            if (item.objectToPool.tag == tag) {
                var obj = Instantiate(item.objectToPool);
                obj.SetActive(false);
                return obj;
            }
        }
        return null;
    }
    public void ReturnToPool(GameObject obj) {
        if (obj == null) return; // throw exception TODO
        Stack<GameObject> poolStack;
        if (!pool.TryGetValue(obj.tag, out poolStack)) Destroy(obj);
        if (poolStack.Count < poolAmounts[obj.tag]) {
            obj.SetActive(false);
            poolStack.Push(obj);
        }
        else {
            Destroy(obj);
        }
    }
}
}