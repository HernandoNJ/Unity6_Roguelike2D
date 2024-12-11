using System;
using UnityEngine;

public class TurnHandler
{
    private int m_TurnCount;
    
    // Only the class declaring the OnTick event can trigger it
    public event Action OnTick;

    public void Tick()
    {
        m_TurnCount++;
        
        // Call, Invoke the callback methods registered to the OnTick event
        OnTick?.Invoke();
        
        Debug.Log("Player tick: " + m_TurnCount);
    }
}