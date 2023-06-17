using UnityEngine;

public class ScreenGlowEffect : MonoBehaviour
{
    public Animation glowAnimation;

    public void StartGlow()
    {
        glowAnimation.Play();
    }

    public void StopGlow()
    {
        glowAnimation.Stop();
    }
}