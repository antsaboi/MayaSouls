using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

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

        GameManager.instance.PickUpItem(type, transform.position);
        pickedUp = true;
        transform.DOPunchScale(new Vector3(0.1f, 0.1f, 0f), 0.3f);
        transform.DOScale(0, 0.5f).OnComplete(()=> {
            gameObject.SetActive(false);
        });
    }
}
