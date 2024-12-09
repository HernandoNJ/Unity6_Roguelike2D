using UnityEngine;

public class CellObject : MonoBehaviour
{
    public virtual void PlayerEntered()
    {
        Debug.Log($"Food increased from CelloObject. object: {gameObject.name}.");
    }
}