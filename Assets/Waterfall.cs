using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Waterfall : MonoBehaviour
{
    AudioSource source;
    Transform player;

    void Start()
    {
        source = GetComponent<AudioSource>();
        player = FindObjectOfType<ProtoPlayer2D>().transform;
        source.volume = 0;
    }

    private void Update()
    {
        if (Vector2.Distance(transform.position, player.position) < 20f)
        {
            if(source.volume < 0.3f) source.volume = Mathf.MoveTowards(source.volume, 0.3f, 0.5f * Time.deltaTime);
        }
        else {
            if(source.volume > 0) source.volume = Mathf.MoveTowards(source.volume, 0, 0.5f * Time.deltaTime);
        }
    }
}
