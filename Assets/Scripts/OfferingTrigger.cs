using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OfferingTrigger : MonoBehaviour
{
    public GameEvent OfferingPromptEvent;
    public GameEvent EndOfferingPromptEvent;
    public GameObject movingTiles;
    public GameObject altarOffer;
    bool isActivated;
    bool playerPresent;

    private void Start()
    {
        movingTiles.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !isActivated)
        {
            if (collision.GetComponent<ProtoPlayer2D>().isAttacking) return;

            playerPresent = true;
            OfferingPromptEvent?.Raise();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !isActivated)
        {
            if (collision.GetComponent<ProtoPlayer2D>().isAttacking) return;

            playerPresent = false;
            EndOfferingPromptEvent?.Raise();
        }
    }

    public void ShowPath()
    {
        if (!playerPresent) return;

        altarOffer.SetActive(true);
        isActivated = true;
        movingTiles.SetActive(true);
    }
}
