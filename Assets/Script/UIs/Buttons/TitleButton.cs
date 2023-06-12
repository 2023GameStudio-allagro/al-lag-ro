using UnityEngine;

public class TitleButton : MonoBehaviour
{
    public void OnClick()
    {
        GameManager.Instance.MoveToTitle();
    }
}
