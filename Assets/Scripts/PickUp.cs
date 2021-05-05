using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUp : MonoBehaviour
{
    bool pickedUp;
    public enum PickUpType
    {
        Relic,
        Offering,
        Soul
    }
    public PickUpType type;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player")) return;
        if(!pickedUp)

        GameManager.instance.PickUpItem(type);
        pickedUp = true;
        gameObject.SetActive(false);
    }
}
