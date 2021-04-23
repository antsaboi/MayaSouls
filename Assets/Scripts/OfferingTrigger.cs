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

    private void Start()
    {
        movingTiles.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !isActivated)
        {
            OfferingPromptEvent?.Raise();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !isActivated)
        {
            EndOfferingPromptEvent?.Raise();
        }
    }

    public void ShowPath()
    {
        altarOffer.SetActive(true);
        isActivated = true;
        movingTiles.SetActive(true);
    }
}
