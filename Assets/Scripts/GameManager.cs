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
    [SerializeField] GameEvent OfferingGiven;
    [SerializeField] GameEvent NoOffering;
    [SerializeField] GameEvent winEvent;
    public GameEvent SoulPickedUp;
    public bool isAlive = true;
    public static Vector3 lastCheckPoint;
    public int soulPickUpHealAmount;

    //inventory
    public int relicAmount;
    public int offeringAmount;

    [SerializeField] AudioClip soulAudio, relicAudio, offeringAudio;
    [SerializeField] ParticleSystem soulPickupParticles, relicPickupParticles;

    private void Awake()
    {
        instance = this;
        StartGame();
    }

    private void Update()
    {
    }

    public void WinGame()
    {
        CameraController.instance.StopFollow();
        isAlive = false;
        winEvent?.Raise();
    }

    public void ReloadGame()
    {
        SceneManager.LoadScene(1);
    }

    public void ToMenu()
    {
        SceneManager.LoadScene(0);
    }

    public void PickUpItem(PickUp.PickUpType type, Vector3 position)
    {
        switch (type)
        {
            case PickUp.PickUpType.Relic:
                relicAmount++;
                AudioSystem.instance.PlayOneShot(relicAudio);
                RelicPickedUp?.Raise();
                relicPickupParticles.transform.position = position;
                relicPickupParticles.Play();
                break;
            case PickUp.PickUpType.Offering:
                AudioSystem.instance.PlayOneShot(offeringAudio);
                offeringAmount++;
                OfferingPickedUp?.Raise();
                break;
            case PickUp.PickUpType.Soul:
                AudioSystem.instance.PlayOneShot(soulAudio);
                SoulPickedUp?.Raise();
                soulPickupParticles.transform.position = position;
                soulPickupParticles.Play();
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
        OfferingPickedUp.Raise();
        AudioSystem.instance.PlayOneShot(offeringAudio);
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
