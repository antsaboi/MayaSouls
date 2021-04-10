using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameUIManager : MonoBehaviour
{
    [SerializeField] PlayerStats playerStats;
    [SerializeField] Image hpFill;
    [SerializeField] ParticleSystem fillParticles;
    [SerializeField] GameObject CheckPointText;
    [SerializeField] float fillRate;
    float fillAmount = 1;
    [SerializeField]float hpParticlesMinPosX, hpParticlesMaxPosX;

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

        fillParticles.Simulate(0.01f);
    }

    private void Update()
    {
        if (!GameManager.instance.isAlive) return;
        else if (playerStats.HP == 0)
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
    }

    public void Death()
    {
        CallAnimation(animationParameters.DeathTriggerName);
        fillAmount = 0;
        hpFill.fillAmount = 0;
        fillParticles.transform.localPosition = new Vector2(Mathf.Lerp(hpParticlesMinPosX, hpParticlesMaxPosX, fillAmount), fillParticles.transform.localPosition.y);
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
