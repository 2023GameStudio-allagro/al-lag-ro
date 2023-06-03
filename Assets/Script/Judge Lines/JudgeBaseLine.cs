using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JudgeBaseLine : MonoBehaviour
{
    private int vertexCount = 50;
    private LineRenderer lineRenderer;

    // Start is called before the first frame update
    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.positionCount = vertexCount + 1;
        Utils.DrawCircle(lineRenderer, Constants.JUDGE_BASE_SIZE);
    }
}