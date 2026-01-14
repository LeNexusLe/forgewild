using System.Collections.Generic;
using UnityEngine;

public class Chest : MonoBehaviour
{
    public Item[] possibleItems;
    public bool isOpened = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isOpened) return;

        ItemManager manager = collision.GetComponent<ItemManager>();
        if (manager == null) return;

        List<Item> chosenItems = GetWeightedRandomItems(3);

        ItemChooseUIController.Instance.ShowChoose(chosenItems, manager);

        isOpened = true;
        Destroy(gameObject);
    }

    List<Item> GetWeightedRandomItems(int count)
    {
        List<Item> pool = new(possibleItems);
        List<Item> result = new();

        for (int i = 0; i < count && pool.Count > 0; i++)
        {
            float totalWeight = 0f;
            foreach (Item item in pool)
                totalWeight += item.weight;

            float randomValue = Random.Range(0f, totalWeight);
            float cumulative = 0f;

            foreach (Item item in pool)
            {
                cumulative += item.weight;
                if (randomValue <= cumulative)
                {
                    result.Add(item);
                    pool.Remove(item);
                    break;
                }
            }
        }

        return result;
    }
}
