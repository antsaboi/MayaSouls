using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    [SerializeField] GameEvent GameStarted;
    [SerializeField] GameEvent PlayerDeath;
    [SerializeField] GameEvent OfferingPickedUp;
    [SerializeField] GameEvent RelicPickedUp;
    public bool isAlive = true;
    public static Vector3 lastCheckPoint;

    //inventory
    public int relicAmount;
    public int offeringAmount;

    private void Awake()
    {
        instance = this;
        StartGame();
    }

    private void Update()
    {
    }

    public void ReloadGame()
    {
        SceneManager.LoadScene(0);
    }

    public void PickUpItem(PickUp.PickUpType type)
    {
        switch (type)
        {
            case PickUp.PickUpType.Relic:
                relicAmount++;
                RelicPickedUp?.Raise();
                break;
            case PickUp.PickUpType.Offering:
                offeringAmount++;
                OfferingPickedUp?.Raise();
                break;
            default:
                break;
        }
    }

    public void GameOver()
    {
        isAlive = false;
        PlayerDeath?.Raise();
    }

    public void StartGame()
    {
        GameStarted?.Raise();
    }
}
