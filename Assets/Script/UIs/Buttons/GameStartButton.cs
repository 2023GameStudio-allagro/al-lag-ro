using UnityEngine;

public class GameStartButton : MonoBehaviour
{
    public void OnClick()
    {
        GameManager.Instance.GameStart();
    }
}
