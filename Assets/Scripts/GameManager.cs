using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    [SerializeField] GameEvent GameStarted;
    [SerializeField] GameEvent PlayerDeath;
    public bool isAlive = true;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        //Do something
        
    }

    public void GameOver()
    {
        isAlive = false;
        PlayerDeath?.Raise();
    }
}
