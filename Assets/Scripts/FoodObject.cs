using UnityEngine;
using UnityEngine.Serialization;

public class FoodObject : CellObject
{
    [SerializeField] private int foodPoints;
    
    public override void PlayerEntered()
    {
        Destroy(gameObject);
        GameManager.Instance.UpdateFoodAmount(foodPoints);
    }
}