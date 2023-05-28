using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JudgeLine : MonoBehaviour
{
    private int vertexCount = 50;
    private LineRenderer lineRenderer;
    
    public float elapsedTime {get; private set;}

    // Start is called before the first frame update
    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.positionCount = vertexCount + 1;

        elapsedTime = -1f;
    }

    // Update is called once per frame
    void Update()
    {
        elapsedTime += Time.deltaTime;

        // Draw circle
        float scale = Utils.MapExp(elapsedTime, -1f, 0f, Constants.JUDGE_LINE_SIZE, Constants.JUDGE_BASE_SIZE);
        float alpha = Utils.ClampedMap(elapsedTime, -1f, 0f, 0.2f, 1f);
        Utils.DrawCircle(lineRenderer, scale);
        SetAlpha(alpha);
        if(elapsedTime > 0.4f) Destroy(gameObject);
    }
    void SetAlpha(float alpha)
    {
        Gradient grad = lineRenderer.colorGradient;
        Color c = Color.white;
        grad.SetKeys(
            new GradientColorKey[] { new GradientColorKey(c, 0.0f), new GradientColorKey(c, 1.0f) },
            new GradientAlphaKey[] { new GradientAlphaKey(alpha, 0.0f), new GradientAlphaKey(alpha, 1.0f) }
        );
        lineRenderer.colorGradient = grad;
    }
}
