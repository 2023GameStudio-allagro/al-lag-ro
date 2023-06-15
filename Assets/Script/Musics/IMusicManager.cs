using System;
using UnityEngine;
using UnityEngine.Events;

public interface IMusicManager
{
    float bpm { get; }
    UnityEvent<char> noteEvent { get; }
    UnityEvent<int> beatEvent { get; }
    void LoadBGMData(int stageNo);
    void StartMusic();
    void EndMusic();
}
