using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;

public class GameUIManager : MonoBehaviour
{
    [SerializeField] PlayerStats playerStats;
    [SerializeField] Image hpFill;
    [SerializeField] ParticleSystem fillParticles;
    [SerializeField] GameObject CheckPointText;
    [SerializeField] float fillRate;
    [SerializeField] Image fadeImage;
    [SerializeField] TextMeshProUGUI relicText;
    [SerializeField] TextMeshProUGUI offeringText;
    [SerializeField] TextMeshProUGUI giveOfferingPrompt, noOfferingText, giveOfferingText, deathText;
    [SerializeField] GameObject pauseMenu;
    [SerializeField] Image[] hpBarFlashImages;
    [SerializeField] GameObject credits;
    [SerializeField] Image whiteFade;
    [SerializeField] AudioClip deathSound;

    float fillAmount = 1;
    [SerializeField]float hpParticlesMinPosX, hpParticlesMaxPosX;

    bool giveOffering;
    bool paused;
    float cachedHP;

    [System.Serializable]
    public struct AnimParameters
    {
        public string DeathTriggerName;
        public string PowerUseBoolName;
    }

    public AnimParameters animationParameters;
    Animator anim;

    private void Start()
    {
        cachedHP = playerStats.HP;
        anim = GetComponentInChildren<Animator>();

        playerStats.OnDeath += Death;

        fadeImage.gameObject.SetActive(true);
        fadeImage.DOFade(0, 2f);
        fillParticles.Simulate(0.01f);
    }

    private void Update()
    {
        if (!GameManager.instance.isAlive)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                GameManager.instance.ToMenu();
            }

            return;
        }

        else if (playerStats.HP < 0)
        {
            GameManager.instance.GameOver();
            return;
        }

        if (playerStats.HP > cachedHP)
        {
            //Health has been added, do something
            FlashHP();
        }

        cachedHP = playerStats.HP;

        //HP has reduced
        if (fillAmount > playerStats.HP / 100)
        {
            fillAmount = Mathf.MoveTowards(fillAmount, playerStats.HP / 100, fillRate * Time.deltaTime);
            hpFill.fillAmount = fillAmount;

            fillParticles.transform.localPosition = new Vector2(Mathf.Lerp(hpParticlesMinPosX, hpParticlesMaxPosX, fillAmount), fillParticles.transform.localPosition.y);
            fillParticles.Play(true);
        }

        //HP has been added
        if (fillAmount < playerStats.HP / 100)
        {
            fillAmount = Mathf.MoveTowards(fillAmount, playerStats.HP / 100, Time.deltaTime);
            hpFill.fillAmount = fillAmount;
            fillParticles.transform.localPosition = new Vector2(Mathf.Lerp(hpParticlesMinPosX, hpParticlesMaxPosX, fillAmount), fillParticles.transform.localPosition.y);
        }

        if (giveOffering)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                GameManager.instance.UseOffering();
            }
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!paused) PauseGame();
            else UnPause();
        }
    }

    public void WinGame()
    {
        fillParticles.gameObject.SetActive(false);

        whiteFade.DOFade(1, 3f).OnComplete(
            () => {
                credits.SetActive(true);
            }
            );

        Debug.Log("winskydoodle");
    }
    
    void FlashHP()
    {
        for (int i = 0; i < hpBarFlashImages.Length; i++)
        {
            hpBarFlashImages[i].DOComplete();
        }

        for (int i = 0; i < hpBarFlashImages.Length; i++)
        {
            hpBarFlashImages[i].DOFade(0.7f, 0.5f).SetLoops(2, LoopType.Yoyo);
        }
    }

    public void PauseGame()
    {
        paused = true;
        pauseMenu.SetActive(true);
        Time.timeScale = 0;
    }

    public void UnPause()
    {
        paused = false;
        pauseMenu.SetActive(false);
        Time.timeScale = 1;
    }

    public void ShowAcceptOffering()
    {
        giveOfferingPrompt.gameObject.SetActive(false);
        noOfferingText.gameObject.SetActive(false);
        giveOfferingText.gameObject.SetActive(true);
    }

    public void HideOfferingTexts()
    {
        giveOfferingPrompt.gameObject.SetActive(false);
        noOfferingText.gameObject.SetActive(false);
        giveOfferingText.gameObject.SetActive(false);
    }

    public void ShowGiveOfferingPrompt()
    {
        giveOfferingPrompt.gameObject.SetActive(true);
        giveOffering = true;
    }

    public void HideGiveOfferingPrompt()
    {
        giveOfferingPrompt.gameObject.SetActive(false);
        giveOffering = false;
    }

    public void ShowNoOffering()
    {
        noOfferingText.gameObject.SetActive(true);
        giveOfferingPrompt.gameObject.SetActive(false);
        giveOffering = false;
    }

    public void UpdateRelicText()
    {
        relicText.text = GameManager.instance.relicAmount.ToString();
    }

    public void UpdateOfferingText()
    {
        offeringText.text = GameManager.instance.offeringAmount.ToString();
    }

    public void Death()
    {
        AudioSystem.instance.PlayOneShot(deathSound);
        CallAnimation(animationParameters.DeathTriggerName);
        fillAmount = 0;
        hpFill.fillAmount = 0;
        fillParticles.transform.localPosition = new Vector2(Mathf.Lerp(hpParticlesMinPosX, hpParticlesMaxPosX, fillAmount), fillParticles.transform.localPosition.y);
        deathText.gameObject.SetActive(true);
        fadeImage.DOFade(1, 3).SetDelay(3).OnComplete(() => {
            GameManager.instance.ReloadGame();
        });
    }

    public void QuitToMenu()
    {
        Time.timeScale = 1;
        UnityEngine.SceneManagement.SceneManager.LoadScene(0);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void OnCheckPoint()
    {
        CheckPointText.SetActive(false);
        CheckPointText.SetActive(true);
    }

    void CallAnimation(string paramName, bool state = true)
    {
        if (!string.IsNullOrEmpty(paramName))
        {
            if (paramName.Contains("Bool"))
            {
                anim.SetBool("paramName", state);
            }
            else {
                anim.SetTrigger("paramName");
            }
        }
    }

    private void OnDestroy()
    {
        playerStats.OnDeath -= Death;
    }
}
