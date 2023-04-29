using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerFire : MonoBehaviour
{
    [SerializeField] Enemy enemy;

    public void TriggerFiring()
    {
        enemy.Fire();
    }
}
