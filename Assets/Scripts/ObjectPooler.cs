using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dawid {
public class ObjectPooler : MonoBehaviour
{
    public static ObjectPooler SharedInstance;

    public Stack<GameObject> pooledObjects;
    public GameObject objectToPool;
    public int amountToPool;
    private void Awake() {
        SharedInstance = this;
        pooledObjects = new Stack<GameObject>();
    }
    private void Start() {
        for (int i = 0; i < amountToPool; i++) {
            var obj = Instantiate(objectToPool);
            obj.SetActive(false);
            pooledObjects.Push(obj);
        }
    }

    public GameObject GetPooledObject() {
        if (pooledObjects.Count > 0 && !pooledObjects.Peek().activeInHierarchy) {
            return pooledObjects.Pop();
        }
        var obj = Instantiate(objectToPool);
        obj.SetActive(false);
        return obj;
    }
    public void ReturnToPool(GameObject obj) {
        if (pooledObjects.Count < amountToPool) {
            obj.SetActive(false);
            pooledObjects.Push(obj);
        }
        else {
            Destroy(obj);
        }
    }
}
}