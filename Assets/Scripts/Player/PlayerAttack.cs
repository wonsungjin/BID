using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Photon.Pun;

public class PlayerAttack : MonoBehaviourPun
{
    private Animator myAnimator;
    private PlayerInfo playerInfo;
    private bool isAttack = true;
    private int motionNum;
    private AudioSource sound;
    HitBoxInfo hitBoxInfo;
    private void Start()
    {
        //sound = GetComponent<AudioSource>();
        myAnimator = GetComponent<Animator>();
        playerInfo = GetComponent<PlayerInfo>();
        hitBoxInfo.attackType = AttackType.Shot;
        hitBoxInfo.interval = 0;
        hitBoxInfo.damageInfo.attackState = state.None;
        hitBoxInfo.damageInfo.attackDamage = playerInfo.basicAttackDamage;
        hitBoxInfo.damageInfo.attackerViewID = gameObject.GetPhotonView().ViewID;
        hitBoxInfo.damageInfo.slowDownRate = 0;
        hitBoxInfo.damageInfo.timer = 0;
    }

    private void Update()
    {
        if (photonView.IsMine != true) return;
        if (GameMgr.Instance.playerInput.inputKey == KeyCode.A) Attack();
    }

    public void Attack()
    {
        //모션 
        if (isAttack == true)
        {
            isAttack = false;
            //모션 랜덤 설정 
            motionNum = Random.Range(0, 3);
            switch (motionNum)
            {
                case 0:
                    {
                        myAnimator.SetTrigger("isAttack1");
                        break;
                    }
                case 1:
                    {
                        myAnimator.SetTrigger("isAttack2");
                        break;
                    }
                case 2:
                    {
                        myAnimator.SetTrigger("isAttack3");
                        break;
                    }
            }
            StartCoroutine(Attack_Delay(motionNum));
        }

    }

    IEnumerator Attack_Delay(int num)
    {
        playerInfo.StayPlayer(0.7f);
        yield return new WaitForSeconds(0.2f);
        GameObject eff = PhotonNetwork.Instantiate("BasicAttackEff", transform.position + new Vector3(0, 1, 0), Quaternion.identity);
        if (num == 0 || num == 1) eff.transform.Rotate(0, 0, -45);
        eff.AddComponent<HitBox>().hitBoxInfo = hitBoxInfo;
        GameMgr.Instance.DestroyTarget(eff, 1f);
        yield return new WaitForSeconds(0.2f);
        Destroy(GetComponent<SphereCollider>());
        //
        yield return new WaitForSeconds(playerInfo.basicAttackSpeed-0.4f);
        isAttack = true;
    }


}
