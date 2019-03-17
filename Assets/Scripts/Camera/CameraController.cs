using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    // Public function for shaking the camera.
    public void ShakeCamera(float duration, float amount)
    {
        StartCoroutine(Shake(duration, amount));
    }

    private IEnumerator Shake(float duration, float amount)
    {
        Vector3 originalPos = transform.position;
        float elapsedTime = 0f;
        while (elapsedTime < duration)
        {
            float x = Random.Range(-1f, 1f) * amount;
            float y = Random.Range(-1f, 1f) * amount;
            transform.position = new Vector3(originalPos.x + x, originalPos.y + y, originalPos.z);

            elapsedTime += Time.deltaTime;
            yield return 0;
        }
        transform.position = originalPos;
    }
}
