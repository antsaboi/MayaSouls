using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameUIManager : MonoBehaviour
{
    [SerializeField] PlayerStats playerStats;
    [SerializeField] RectTransform hpFill;
    [SerializeField] ParticleSystem fillParticles;
    [SerializeField] GameObject CheckPointText;
    [SerializeField] float fillRate;
    float fillAmount = 1;
    float fillStartWidth;
    Coroutine fillRoutine, reduceRoutine;
    Vector2 hpFillStartPos;

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
        fillStartWidth = hpFill.rect.width;
        hpFillStartPos = hpFill.anchoredPosition;

        playerStats.OnHPReduced += SetHPReduceSmooth;
        playerStats.OnDeath += Death;
        playerStats.OnReset += SetHPFillSmooth;

        fillParticles.Simulate(0.01f);
    }

    private void Update()
    {
        //HP has reduced
        if (playerStats.HP / 100 < fillAmount)
        {
            fillAmount = Mathf.MoveTowards(fillAmount, playerStats.HP / 100, fillRate * Time.deltaTime);
            hpFill.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, fillStartWidth * fillAmount);
            hpFill.ForceUpdateRectTransforms();
            float delta = fillStartWidth - (fillStartWidth * fillAmount);
            hpFill.anchoredPosition = new Vector2(hpFillStartPos.x - delta / 2, hpFillStartPos.y);

            fillParticles.transform.position = new Vector2(hpFill.transform.position.x + (3.4f * fillAmount), hpFill.transform.position.y);
            fillParticles.Play(true);

            if (playerStats.HP <= 0)
            {
                hpFill.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 0);
                hpFill.ForceUpdateRectTransforms();
                fillParticles.transform.position = new Vector2(hpFill.transform.position.x, hpFill.transform.position.y);
            }
        }

        //HP has been added
        if (playerStats.HP / 100 > fillAmount)
        {
            fillAmount = Mathf.MoveTowards(fillAmount, playerStats.HP / 100, 0.2f * Time.deltaTime);
            hpFill.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, fillStartWidth * fillAmount);
            hpFill.ForceUpdateRectTransforms();
            float delta = fillStartWidth - (fillStartWidth * fillAmount);
            hpFill.anchoredPosition = new Vector2(hpFillStartPos.x - delta / 2, hpFillStartPos.y);
            fillParticles.transform.position = new Vector2(hpFill.transform.position.x + (3.4f * fillAmount), hpFill.transform.position.y);
        }
    }

    void SetHPReduceSmooth(bool usePower)
    {

    }

    void SetHPFillSmooth()
    {

    }

    void Death()
    {
        CallAnimation(animationParameters.DeathTriggerName);
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
        playerStats.OnHPReduced -= SetHPReduceSmooth;
        playerStats.OnDeath -= Death;
        playerStats.OnReset -= SetHPFillSmooth;
    }
}
