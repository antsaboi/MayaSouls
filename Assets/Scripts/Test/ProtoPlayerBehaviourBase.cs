using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProtoPlayerBehaviourBase : ScriptableObject
{
    protected GameObject playerInstance;

    public void Initialize(GameObject instance)
    {
        playerInstance = instance;
    }

    public virtual void StartBehaviour()
    {
        
    }

    public virtual void UpdateBehaviour()
    {
        
    }

    public virtual void FixedUpdateBehaviour()
    {

    }

    public virtual void OnDestroyBehaviour()
    {
        
    }
}