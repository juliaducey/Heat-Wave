using UnityEngine;
using System.Collections;
using System.Threading;

/// <summary>
/// Listens for when the scene is done waiting to wake up the Lua thread and to put it to sleep again when it resumes waiting.
/// </summary>
public class LuaWaitListener : IWaitListener
{
    readonly AutoResetEvent _lock = new AutoResetEvent(false);

    /// <summary>
    /// Called when the MetisSceneController stops waiting. Wakes up the Lua thread.
    /// </summary>
    public void OnStopWaiting()
    {
        _lock.Set();
    }

    /// <summary>
    /// Called when the MetisSceneController starts waiting. Puts the Lua thread to sleep.
    /// </summary>
    public void OnStartWaiting()
    {
        _lock.WaitOne();
    }
}
