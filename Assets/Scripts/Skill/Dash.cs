using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Dash : Skill
{

    private void Awake()
    {
        skillInfo.type = SkillType.Skill;
        skillInfo.cooltime = 5;
        skillInfo.skillNum = 0;
        skillInfo.skillType = SkillType.Buff;
        GameMgr.Instance.uIMgr.SetSkillIcon(0, 1);
    }
    private void FixedUpdate()
    {
        if (GameMgr.Instance.GameState == false) return;
        if (GameMgr.Instance.playerInput.inputKey == KeyCode.F) SkillUse();
    }
    protected override void SkillFire()
    {
        animator.SetTrigger("isBasicDash");
        playerInfo.SetChangeMoveSpeed(300f, 0.5f);
        gameObject.GetPhotonView().RPC("SetGhostEff", RpcTarget.All, 0.5f);
        Debug.Log("�뽬");
        if (skillInfo.cooltime != 0) GameMgr.Instance.uIMgr.SkillCooltime(skillInfo.cooltime, skillInfo.skillNum,1);
    }
}
