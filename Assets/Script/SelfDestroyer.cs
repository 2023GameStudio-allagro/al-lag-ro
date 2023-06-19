using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelfDestroyer : MonoBehaviour
{
    public float destoryTime = 1f;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(SelfDestroy(destoryTime));
    }
    IEnumerator SelfDestroy(float time)
    {
        yield return new WaitForSeconds(time);
        Destroy(gameObject);
    }
}
