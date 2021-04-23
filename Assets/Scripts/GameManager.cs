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
    [SerializeField] GameEvent SoulPickedUp;
    [SerializeField] GameEvent OfferingGiven;
    [SerializeField] GameEvent NoOffering;
    public bool isAlive = true;
    public static Vector3 lastCheckPoint;
    public int soulPickUpHealAmount;

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
            case PickUp.PickUpType.Soul:
                SoulPickedUp?.Raise();
                break;
            default:
                break;
        }
    }

    public void UseOffering()
    {
        if (offeringAmount < 1) {
            NoOffering?.Raise();
            return;
        }
        offeringAmount--;
        OfferingGiven?.Raise();
    }

    public void GameOver(bool cameraZoom = true)
    {
        isAlive = false;
        PlayerDeath?.Raise();
        if(cameraZoom) CameraController.instance.ZoomTo(3, 6f);
    }

    public void StartGame()
    {
        GameStarted?.Raise();
    }
}
