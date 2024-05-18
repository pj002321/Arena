using Arena.Core;
using Arena.Characters;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeAttack_Behaviour : AttackBehaviour
{
    public ManualCollision attackCollision;

    public override void ExecuteAttack(GameObject target = null, Transform startPoint = null)
    {
        Collider[] colliders = attackCollision?.CheckOverlapBox(targetMask);

        foreach (Collider col in colliders)
        {
            Debug.Log(col.gameObject);
            col.gameObject.GetComponent<IDamagable>()?.TakeDamage(damage, effectPrefab);
        }

        calcCoolTime = 0.0f;
    }
}
