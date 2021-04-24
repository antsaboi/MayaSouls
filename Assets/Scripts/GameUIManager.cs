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
    [SerializeField] TextMeshProUGUI giveOfferingPrompt, noOfferingText, giveOfferingText;
    [SerializeField] GameObject pauseMenu;

    float fillAmount = 1;
    [SerializeField]float hpParticlesMinPosX, hpParticlesMaxPosX;

    bool giveOffering;
    bool paused;

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
        anim = GetComponentInChildren<Animator>();

        playerStats.OnDeath += Death;

        fadeImage.gameObject.SetActive(true);
        fadeImage.DOFade(0, 2f);
        fillParticles.Simulate(0.01f);
    }

    private void Update()
    {
        if (!GameManager.instance.isAlive) return;
        else if (playerStats.HP < 0)
        {
            GameManager.instance.GameOver();
            return;
        }

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
        CallAnimation(animationParameters.DeathTriggerName);
        fillAmount = 0;
        hpFill.fillAmount = 0;
        fillParticles.transform.localPosition = new Vector2(Mathf.Lerp(hpParticlesMinPosX, hpParticlesMaxPosX, fillAmount), fillParticles.transform.localPosition.y);
        fadeImage.DOFade(1, 2).SetDelay(3).OnComplete(() => {
            GameManager.instance.ReloadGame();
        });
    }

    public void QuitToMenu()
    {
        GameManager.instance.ToMenu();
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
