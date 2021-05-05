using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class OfferingTrigger : MonoBehaviour
{
    public GameEvent OfferingPromptEvent;
    public GameEvent EndOfferingPromptEvent;
    public GameObject movingTiles;
    public GameObject altarOffer;
    public PickUp[] relics;
    bool isActivated;
    bool playerPresent;

    bool checkForRelics;
    float relicTimestamp;
    public TextMeshProUGUI relicsText;

    private void Start()
    {
        movingTiles.SetActive(false);
        if (relics == null || relics.Length < 1) checkForRelics = false;
        else checkForRelics = true;
    }

    private void Update()
    {
        if (checkForRelics)
        {
            if (relicTimestamp < Time.time)
            {
                relicTimestamp = Time.time + 1f;

                int activeCount = 0;

                for (int i = 0; i < relics.Length; i++)
                {
                    if (relics[i].gameObject.activeInHierarchy) activeCount++;
                }

                relicsText.text = "Relics " + (relics.Length - activeCount).ToString() + "/" + relics.Length;

                if ((relics.Length - activeCount) == relics.Length) checkForRelics = false;
            }
        }
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
