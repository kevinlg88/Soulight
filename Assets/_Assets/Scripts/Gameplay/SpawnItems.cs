using System.Collections.Generic;
using UnityEngine;

public class SpawnItems : MonoBehaviour
{
    [SerializeField] private GameObject itemsPrefab;


    void Update()
    {
        CheckChildrenCount();
    }

    private void CheckChildrenCount()
    {
        if (transform.childCount <= 0)
        {
            SpawnItem();
        }
    }

    private void SpawnItem()
    {
        Instantiate(itemsPrefab, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}
