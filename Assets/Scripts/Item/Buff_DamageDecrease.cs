using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Buff_DamageDecrease : Skill
{
    [SerializeField] private int itemNum = 0;
    public void GetItem(int itemType,int itemnum)//inventory order
    {
        if (itemNum == 0)
        {
            itemNum = itemnum;
            skillInfo.itemNum = itemNum;
            skillInfo.skillNum = itemType;
            GameMgr.Instance.uIMgr.SetItemIcon(itemType,itemNum);
        }
    }
    private void Awake()
    {
        skillInfo.type = SkillType.Item;
        skillInfo.skillType = SkillType.Buff;
    }
    private void Update()
    {
        if (GameMgr.Instance.GameState == false) return;
        if (itemNum == 0 && GameMgr.Instance.playerInput.inputKey == KeyCode.Q) SkillUse();
        else if (itemNum == 1 && GameMgr.Instance.playerInput.inputKey == KeyCode.W) SkillUse();
        else if (itemNum == 2 && GameMgr.Instance.playerInput.inputKey == KeyCode.E) SkillUse();
        else if (itemNum == 3 && GameMgr.Instance.playerInput.inputKey == KeyCode.R) SkillUse();
        else if (GameMgr.Instance.playerInput.inputKey == KeyCode.Mouse0) SkillClick(Input.mousePosition);
    }
    protected override void SkillFire()
    {
        if (skillInfo.skillType == SkillType.Immediate) GameMgr.Instance.uIMgr.onSetItemDescription -= ItemRadius;
        if (skillInfo.skillType != SkillType.Buff && skillInfo.skillType != SkillType.Passive) GameMgr.Instance.codeExample.onChangeSkillType -= UnClick;

        GameObject eff1 = PhotonNetwork.Instantiate("DamageDecrease", transform.position, Quaternion.identity);

        GameMgr.Instance.DestroyTarget(eff1, 5f);
        MyPosInfo myPosInfo;
        myPosInfo.myPos = gameObject.transform;
        myPosInfo.yPos = 3;
        myPosInfo.xPos = 0;
        myPosInfo.zPos = 0;        
        eff1.AddComponent<MyPos>().myPosInfo = myPosInfo;
        playerInfo.SetDamageDecrease(50f, 5f);

        GameMgr.Instance.uIMgr.UseItem(itemNum);
        Destroy(GetComponent<Buff_DamageDecrease>());
    }
}
