using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeatReceiver : MonoBehaviour
{
    float scale = 4f;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        scale -= 0.05f;
        if(scale < 0f) scale = 0f;
        transform.localScale = new Vector3(scale, scale, scale);
    }

    public void Nye()
    {
        scale = 4f;
    }
}
