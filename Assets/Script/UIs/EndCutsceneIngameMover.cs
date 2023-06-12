using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndCutsceneIngameMover : MonoBehaviour
{
    public void OnEndCutscene()
    {
        GameManager.Instance.MoveToIngame();
    }
}