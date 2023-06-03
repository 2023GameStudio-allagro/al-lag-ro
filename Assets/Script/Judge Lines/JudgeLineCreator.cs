using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JudgeLineCreator : MonoBehaviour
{
    private Queue<JudgeLine> noteQueue = new Queue<JudgeLine>();
    [SerializeField] private GameObject judgeLineBase;

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
        GameObject noteObject = Instantiate(judgeLineBase);
        noteObject.transform.SetParent(this.transform, false);
        JudgeLine note = noteObject.GetComponent<JudgeLine>();
        note.onTimeover += TimeoverNote;
        noteQueue.Enqueue(note);
    }

    public Judgement HitNote()
    {
        if(noteQueue.Count == 0) return Judgement.early;

        JudgeLine note = noteQueue.Peek();
        return note.JudgeTiming();
    }
    public void RemoveCurrentNode()
    {
        if(noteQueue.Count == 0) return;

        JudgeLine note = noteQueue.Dequeue();
        note.Delete();
    }

    private void TimeoverNote()
    {
        noteQueue.Dequeue();
        ScoreManager.Instance?.Miss();
    }
}
