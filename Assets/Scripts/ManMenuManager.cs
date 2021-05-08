using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;

public class ManMenuManager : MonoBehaviour
{
    public GameObject nav, credits;

    public Image fade;
    public RawImage fogMaterial;
    [Range(0,1f)]
    public float speed;
    public TextMeshProUGUI startText;
    public AudioClip buttonSound, heartBeat;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        fogMaterial.materialForRendering.mainTextureOffset = new Vector2(Time.time * speed, 0);
    }

    public void StartButton()
    {
        AudioSystem.instance.PlayOneShot(buttonSound);
        fade.gameObject.SetActive(true);
        fade.DOFade(1, 2f).OnComplete(()=> {
            startText.gameObject.SetActive(true);
            AudioSystem.instance.PlayGameMusic();
            AudioSystem.instance.PlayOneShot(heartBeat);
        });
    }

    public void StartGame()
    {
        SceneManager.LoadScene(1);
    }

    public void CreditsButton()
    {
        AudioSystem.instance.PlayOneShot(buttonSound);
        nav.SetActive(false);
        credits.SetActive(true);
    }

    public void CreditsBackButton()
    {
        AudioSystem.instance.PlayOneShot(buttonSound);
        credits.SetActive(false);
        nav.SetActive(true);
    }

    public void QuitButton()
    {
        AudioSystem.instance.PlayOneShot(buttonSound);
        Application.Quit();
    }
}
