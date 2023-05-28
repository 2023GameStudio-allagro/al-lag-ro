using System;
using UnityEngine;
using UnityEngine.Events;

public interface IMusicManager
{
    UnityEvent noteEvent { get; }
    UnityEvent beatEvent { get; }
    void LoadBGMData(int stageNo);
    void StartMusic();
    void EndMusic();
}
