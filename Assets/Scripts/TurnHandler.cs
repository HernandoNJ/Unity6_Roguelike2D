using UnityEngine;

public class TurnHandler
{
    private int m_TurnCount;

    public TurnHandler()
    {
        m_TurnCount = 1;
    }

    public void Tick()
    {
        m_TurnCount++;
        Debug.Log("Player tick: " + m_TurnCount);
    }
}