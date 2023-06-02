using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TemporalStatus 
{
	private bool activated = false;
	private float duration = 0f;
	public void Activate(float time)
	{
		activated = true;
		duration = time;
	}
	public void Update()
	{
		if(!activated) return;
		duration -= Time.deltaTime;
		if(duration <= 0)
		{
			duration = 0;
			activated = false;
		}
	}
	public bool ToBool()
	{
		return activated;
	}
	public static implicit operator bool(TemporalStatus status)
    {
        return status.activated;
    }
}