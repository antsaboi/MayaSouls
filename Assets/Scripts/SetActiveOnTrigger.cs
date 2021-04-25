using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetActiveOnTrigger : MonoBehaviour
{
    [SerializeField] GameObject toActivate;
    [SerializeField] string tagToCompare;

    void Start()
    {
        toActivate.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(tagToCompare))
        {
            toActivate.SetActive(true);
        }
    }
}
