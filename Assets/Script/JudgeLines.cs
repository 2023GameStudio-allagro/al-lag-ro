using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JudgeLine : MonoBehaviour
{
    private int numPoints = 50;
    public float radius = 2f;
    private LineRenderer lineRenderer;
    // Start is called before the first frame update
    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.positionCount = numPoints + 1;
        lineRenderer.useWorldSpace = false;
        lineRenderer.loop=true;
        DrawCircle(radius);
    }

    // Update is called once per frame
    void Update()
    {
        
        // radius -= 1f * Time.deltaTime;
        // if(radius < 0f) Destroy();
    }
    void DrawCircle(float radius)
    {
        float angle = 0.0f;
        float angleIncrement = 2 * Mathf.PI / numPoints;
        for(int i=0; i<=numPoints; i++)
        {
            float x = Mathf.Sin(angle) * radius;
            float y = Mathf.Cos(angle) * radius;
            lineRenderer.SetPosition(i, new Vector3(x, y, 0f));
            angle += angleIncrement;
        }
    }
}
