using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndCutsceneTutorialMover : MonoBehaviour
{
    public void OnEndCutscene()
    {
        GameManager.Instance.MoveToTutorial();
    }
}