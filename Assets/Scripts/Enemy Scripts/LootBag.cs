using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LootBag : MonoBehaviour
{
    public GameObject droppedItemPrefab;
    public List<Loot> lootList = new List<Loot>();

    Loot GetDroppedItem() 
    {
        // Randomizes the loot number, determined by the percentage chance of the prefab in the unity editor
        int randomNumber = Random.Range(1, 101); 
        List<Loot> PossibleItems = new List<Loot>();
        foreach (Loot item in lootList)
        {
            if (randomNumber <= item.dropChance)
            {
                // If the number is within range, add the item to a list
                PossibleItems.Add(item);
            }
        }
        // If an item is valid, pick a random item and return it 
        if(PossibleItems.Count > 0)
        {
            Loot droppedItem = PossibleItems[Random.Range(0, PossibleItems.Count)];
            return droppedItem;
        }

        // In case no item is chosen 
        Debug.Log("No loot dropped");
        return null;
    }

    // Spawning the loot 
    public void InstantiateLoot(Vector3 spawnPosition)
    { 
        Loot droppedItem = GetDroppedItem();
        if(droppedItem != null)
        {
            GameObject lootGameObject = Instantiate(droppedItemPrefab.gameObject, spawnPosition, Quaternion.identity);
            lootGameObject.GetComponent<SpriteRenderer>().sprite = droppedItem.lootSprite;


            // Add flair to how the item is dropped. If items fly off screen, remove this line of code
            float dropForce = 300f;
            Vector2 dropDirection = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f));
            lootGameObject.GetComponent<Rigidbody2D>().AddForce(dropDirection * dropForce, ForceMode2D.Impulse);
        }
    }
}
