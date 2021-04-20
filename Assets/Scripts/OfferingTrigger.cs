using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OfferingTrigger : MonoBehaviour
{
    public GameEvent OfferingPromptEvent;
    public GameEvent EndOfferingPromptEvent;
    public GameObject movingTiles;

    private void Start()
    {
        movingTiles.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            OfferingPromptEvent?.Raise();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            EndOfferingPromptEvent?.Raise();
        }
    }

    public void ShowPath()
    {
        movingTiles.SetActive(true);
    }
}
