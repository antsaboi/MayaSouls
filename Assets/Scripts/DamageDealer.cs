using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageDealer : MonoBehaviour
{
    [Range(0,100)]
    public int damage;
    public Vector2 knockBack;
}
