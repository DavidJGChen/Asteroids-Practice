using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dawid {
public class ExplosionManager : MonoBehaviour
{
    // Start is called before the first frame update
    public static ExplosionManager Manager;
    private Transform cameraTransform;
    private void Awake() {
        Manager = this;
        cameraTransform = Camera.main.transform;
    }

    public void Shake(float magnitude = 0.5f, float duration = 0.2f) {
        StartCoroutine(ShakeCoroutine(magnitude, duration));
    }

    private IEnumerator ShakeCoroutine(float magnitude, float duration) {
        var originalPos = cameraTransform.localPosition;

        var elapsed = 0f;

        while (elapsed < duration) {
            elapsed += Time.deltaTime;

            var dir = Random.insideUnitCircle * magnitude;

            cameraTransform.localPosition = new Vector3(dir.x, dir.y, originalPos.z);

            yield return null;
        }

        cameraTransform.localPosition = originalPos;
    }
}
}