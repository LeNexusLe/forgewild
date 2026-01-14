using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    public Item item;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        ItemManager manager = collision.GetComponent<ItemManager>();
        if (manager != null)
        {
            manager.AddItem(item);
            Destroy(gameObject);
        }
    }
}
