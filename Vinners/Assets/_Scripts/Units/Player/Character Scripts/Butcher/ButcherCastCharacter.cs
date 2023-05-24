using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FishNet.Object;
using System;
using UnityEngine.InputSystem;

public class ButcherCastCharacter : CastCharacter
{
    # region Taunt skill
    [Header("Taunt Skill")]
    [SerializeField] private GameObject tauntSpellPrefab;
    public void OnSkill()
    {
        if (!IsOwner) return;
        if (base.canCast[0])
        {
            StartCoroutine(Cooldown(0));
            CastTauntSkill();
            Debug.Log("Spell casted");
        }
        else
        {
            Debug.Log("Spell is on cooldown");
        }
        
    }

    [ServerRpc]
    public void CastTauntSkill()
    {
        GameObject obj = Instantiate(tauntSpellPrefab, transform);
        obj.GetComponent<CharacterDamager>().lifetime = spellData[0].lifetime;
        obj.GetComponent<CharacterDamager>().damage = spellData[0].damage * character.currAttack;
        ServerManager.Spawn(obj);
        Debug.Log("Spell casted");
    }

    #endregion

    # region Charge skill
    [Header("Charge Skill")]
    public float chargeSpeed = 3f;
    public float chargeTime = 0.8f;
    public float windUp = 0.3f;
    public void OnDash()
    {
        if (!IsOwner) return;
        if (canCast[1])
        {
            StartCoroutine(Cooldown(1));
            // CastChargeSkill();
            StartCoroutine(Charge());
            Debug.Log("Charge spell casted");
        }
        else
        {
            Debug.Log("Spell is on cooldown");
        }
    }

    [ServerRpc]
    public void CastChargeSkill()
    {
        // spawn a damager on the player to do the aoe

    }

    public IEnumerator Charge()
    {
        movement.interrupted = true;
        yield return new WaitForSeconds(windUp);
        rigidBody.velocity = 3 * chargeSpeed * input.targetDirection;
        yield return new WaitForSeconds(chargeTime);
        movement.interrupted = false;
    }

    #endregion


}
