using UnityEngine;

public class FoodObject : CellObject
{
    public override void PlayerEntered()
    {
        Destroy(gameObject);
      
        //increase food
        Debug.Log("Food increased");
        Debug.Log($"Gameobject name: {gameObject.name}");
        Debug.Log($"****************");
        base.PlayerEntered();
    }
}