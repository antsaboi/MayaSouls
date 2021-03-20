using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameUIManager : MonoBehaviour
{
    [SerializeField] PlayerStats playerStats;
    [SerializeField] RectTransform hpFill;
    [SerializeField] ParticleSystem fillParticles;
    float fillRate;
    float fillAmount = 1;
    float fillStartWidth;
    Coroutine fillRoutine, reduceRoutine;
    Vector2 hpFillStartPos;

    [System.Serializable]
    public struct AnimParameters
    {
        public string DeathTriggerName;
        public string HPReduceBoolName;
    }

    public AnimParameters animationParameters;
    Animator anim;

    private void Start()
    {
        anim = GetComponentInChildren<Animator>();
        fillStartWidth = hpFill.rect.width;
        hpFillStartPos = hpFill.anchoredPosition;
        fillRate = playerStats.HPReduceRatePerSecond / 100;
        playerStats.OnHPReduced += SetHPReduceSmooth;
        playerStats.OnDeath += Death;
        playerStats.OnReset += SetHPFillSmooth;
    }

    void SetHPReduceSmooth()
    {
        if (fillRoutine != null) StopCoroutine(fillRoutine);
        if (reduceRoutine != null) StopCoroutine(reduceRoutine);
        reduceRoutine = StartCoroutine(HPreduceSmooth());
    }

    void SetHPFillSmooth()
    {
        if (reduceRoutine != null) StopCoroutine(reduceRoutine);
        if (fillRoutine != null) StopCoroutine(fillRoutine);
        fillRoutine = StartCoroutine(HPFillSmooth());
    }

    IEnumerator HPreduceSmooth()
    {
        CallAnimation(animationParameters.HPReduceBoolName, true);
        while (fillAmount > playerStats.HP / 100)
        {
            fillAmount = Mathf.MoveTowards(fillAmount, playerStats.HP / 100, fillRate * Time.deltaTime);
            hpFill.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, fillStartWidth * fillAmount);
            hpFill.ForceUpdateRectTransforms();
            float delta = fillStartWidth - (fillStartWidth * fillAmount);
            hpFill.anchoredPosition = new Vector2(hpFillStartPos.x - delta/2, hpFillStartPos.y);
            fillParticles.transform.position = new Vector2(hpFill.transform.position.x + (3.4f * fillAmount), hpFill.transform.position.y);
            fillParticles.Play(true);

            if (playerStats.HP <= 0) {
                hpFill.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 0);
                hpFill.ForceUpdateRectTransforms();
                fillParticles.transform.position = new Vector2(hpFill.transform.position.x, hpFill.transform.position.y);
                break;
            }
            yield return new WaitForEndOfFrame();
        }

        CallAnimation(animationParameters.HPReduceBoolName, false);
    }

    IEnumerator HPFillSmooth()
    {
        while (fillAmount < playerStats.HP / 100)
        {
            fillAmount = Mathf.MoveTowards(fillAmount, playerStats.HP / 100, 0.2f * Time.deltaTime);
            hpFill.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, fillStartWidth * fillAmount);
            hpFill.ForceUpdateRectTransforms();
            float delta = fillStartWidth - (fillStartWidth * fillAmount);
            hpFill.anchoredPosition = new Vector2(hpFillStartPos.x - delta / 2, hpFillStartPos.y);
            fillParticles.transform.position = new Vector2(hpFill.transform.position.x + (3.4f * fillAmount), hpFill.transform.position.y);
            yield return new WaitForEndOfFrame();
        }
        fillParticles.Simulate(0.01f);
    }

    void Death()
    {
        CallAnimation(animationParameters.DeathTriggerName);
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
