using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JudgeLineCrator : MonoBehaviour
{
    [SerializeField] GameObject judgeLineBase;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void MakeNote()
    {
        Debug.Log("Nye");
        GameObject note = Instantiate(judgeLineBase);
        note.transform.SetParent(this.transform, false);
    }
}
