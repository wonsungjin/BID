using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomSkill : MonoBehaviour
{
    //total number of skills
    private int skillType = 5;
    //skills random
    public int skillRan { get; private set; } = 0;
    //Distributing random skills
    public void GetRandomSkill(GameObject player)
    {
        //Random skill number
        skillRan = Random.Range(1, skillType+1);
        GameMgr.Instance.uIMgr.SetSkillIcon(skillRan,0);
        if (skillRan == 1) player.AddComponent<Immediate_BloodField>().SetSkillNum(skillRan);
        else if (skillRan == 2) player.AddComponent<Projectile_Bash>().SetSkillNum(skillRan);
        else if (skillRan == 3) player.AddComponent<NonTarget_FallTheRain>().SetSkillNum(skillRan);
        else if (skillRan == 4) player.AddComponent<Projectile_MysticArrow>().SetSkillNum(skillRan);
        else if (skillRan == 5) player.AddComponent<Projectile_EnergyShoot>().SetSkillNum(skillRan);
        else
            Debug.Log("Player didn't get any skill");
    }
}
