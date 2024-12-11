using UnityEngine;

public class FoodObject : CellObject
{
    [SerializeField] private int foodPoints;
    
    public override void PlayerEntered()
    {
        Debug.Log("Player entered food object");
        Destroy(gameObject);
        GameManager.Instance.UpdateFood(foodPoints);
    }

}