using UnityEngine;
using System.Collections;

/// <summary>
/// Listens for the MetisSceneController to change waiting status.
/// </summary>
public interface IWaitListener
{
    /// <summary>
    /// Called when the MetisSceneController stops waiting.
    /// </summary>
    void OnStopWaiting();
    /// <summary>
    /// Called when the MetisSceneController starts waiting.
    /// </summary>
    void OnStartWaiting();
}
