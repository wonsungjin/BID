using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class TestSkill : Skill
{
    public void SetSkillNum(int Num)
    {
        skillInfo.skillNum = Num;
    }
    private void Awake()
    {
        skillInfo.type = SkillType.Skill;
        skillInfo.angle = 20;//use Cone
        skillInfo.radius = 6;//use Immediate,NonTarget
        skillInfo.range = 30;//use Projectile,NonTarget,Cone
        skillInfo.length = 6;//use Projectile,
        skillInfo.cooltime = 5;
        skillInfo.skillType = SkillType.Immediate;
        skillInfo.hitReturn = false;

        skillInfo.hitBoxInfo.attackType = AttackType.Shot;
        skillInfo.hitBoxInfo.interval = 1;

        skillInfo.hitBoxInfo.damageInfo.attackState = state.None;
        skillInfo.hitBoxInfo.damageInfo.attackDamage = 10;
        skillInfo.hitBoxInfo.damageInfo.attackerViewID = gameObject.GetPhotonView().ViewID;
        skillInfo.hitBoxInfo.damageInfo.slowDownRate = 0;
        skillInfo.hitBoxInfo.damageInfo.timer = 0;

    }
    private void FixedUpdate()
    {
        if (GameMgr.Instance.playerInput.inputKey == KeyCode.D) SkillUse();
        else if (GameMgr.Instance.playerInput.inputKey == KeyCode.Mouse0) SkillClick(Input.mousePosition);
    }
    protected override void SkillFire()
    {
        GameObject eff = PhotonNetwork.Instantiate("HpRecovery", transform.position, Quaternion.identity);
        eff.AddComponent<HitBox>().skillInfo = skillInfo;

        if (skillInfo.cooltime != 0) GameMgr.Instance.uIMgr.SkillCooltime(skillInfo.cooltime, skillInfo.skillNum,0);
    }
}
