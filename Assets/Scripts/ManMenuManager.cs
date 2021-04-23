using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using DG.Tweening;

public class ManMenuManager : MonoBehaviour
{
    public GameObject nav, credits;

    public Image fade;
    public Material fogMaterial;
    [Range(0,1f)]
    public float speed;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        fogMaterial.mainTextureOffset = new Vector2(Time.time * speed, 0);
    }

    public void StartButton()
    {
        fade.gameObject.SetActive(true);
        fade.DOFade(1, 2f).OnComplete(()=> { SceneManager.LoadScene(1); });
    }

    public void CreditsButton()
    {
        nav.SetActive(false);
        credits.SetActive(true);
    }

    public void CreditsBackButton()
    {
        credits.SetActive(false);
        nav.SetActive(true);
    }

    public void QuitButton()
    {
        Application.Quit();
    }
}
