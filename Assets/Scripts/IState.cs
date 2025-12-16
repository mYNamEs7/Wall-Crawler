using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IState<out TInitializer>
{
    public TInitializer Initializer { get; }
    public void OnEnter() { }
    public void OnRun() { }
    public void OnExit() { }
}
