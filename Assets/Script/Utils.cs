using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Utils
{
    public static bool Between(float value, float min, float max)
    {
        return min < value && value < max;
    }
    public static float Map(float value, float fromMin, float fromMax, float toMin, float toMax)
    {
        float ratio = (value - fromMin) / (fromMax - fromMin);
        return ratio * (toMax - toMin) + toMin;
    }
    public static float ClampedMap(float value, float fromMin, float fromMax, float toMin, float toMax)
    {
        float ratio = (value - fromMin) / (fromMax - fromMin);
        if(ratio < 0f) ratio = 0f;
        else if(ratio > 1f) ratio = 1f;
        return ratio * (toMax - toMin) + toMin;
    }
    public static float MapExp(float value, float fromMin, float fromMax, float toMin, float toMax)
    {
        if(toMin <= 0)  throw new System.ArgumentException("Parameter should be positive number", "toMin");
        if(toMax <= 0)  throw new System.ArgumentException("Parameter should be positive number", "toMax");
        float ratio = (value - fromMin) / (fromMax - fromMin);
        return toMin * Mathf.Exp(ratio * Mathf.Log(toMax / toMin));
    }
    public static Vector2 GetRandomPositionOutBound(Bounds bound, float offset)
    {
        int direction = Random.Range(0, 4);
        float randX = Random.Range(bound.min.x-offset, bound.max.x+offset);
        float randY = Random.Range(bound.min.y-offset, bound.max.y+offset);
        switch(direction)
        {
            case 0:
                return new Vector2(bound.min.x-offset, randY);
            case 1:
                return new Vector2(randX, bound.min.y-offset);
            case 2:
                return new Vector2(bound.max.x+offset, randY);
            case 3:
                return new Vector2(randX, bound.max.y+offset);
            default:
                return new Vector2();
        }
    }
    public static void DrawCircle(LineRenderer lineRenderer, float radius)
    {
        float vertexCount = lineRenderer.positionCount - 1;

        float deltaTheta = (2f * Mathf.PI) / vertexCount;
        float theta = 0f;

        for (int i = 0; i <= vertexCount; i++)
        {
            float x = radius * Mathf.Cos(theta);
            float y = radius * Mathf.Sin(theta);
            Vector3 pos = new Vector3(x, y, 0f);

            lineRenderer.SetPosition(i, pos);

            theta += deltaTheta;
        }
    }
}
