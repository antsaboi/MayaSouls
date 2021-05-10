using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MenuButton : MonoBehaviour, IPointerEnterHandler
{
    [SerializeField] AudioClip onHoverClip;

    public void OnPointerEnter(PointerEventData eventData)
    {
        AudioSystem.instance.PlayOneShot(onHoverClip);
    }
}
