using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.FinalBossScene;
using Assets.Scripts.Shared;

public class DebugManager : MonoBehaviour
{
    public FinalBossScript FinalBossScript;
    public PlayerScript PlayerScript;

    public void Attack_Player()
    {
        FinalBossScript.InternalAttackPlayer();
    }
    public void BossActive()
    {
        FinalBossScript.SetActive();
    }
    public void Knockback()
    {
        PlayerScript.KnockBack(true);
    }
}
