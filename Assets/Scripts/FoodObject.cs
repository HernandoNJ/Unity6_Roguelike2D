using UnityEngine;

public class FoodObject : CellObject
{
    [SerializeField] private int foodPoints;
    
    public override void PlayerEntered()
    {
        Destroy(gameObject);
        GameManager.Instance.UpdateFood(foodPoints);
    }
}