using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomSkill : MonoBehaviour
{
    //total number of skills
    private int skillNum = 3;
    //Skill number to draw randomly
    public int skillRan { get; set; } = 0;

    // Random skill payment
    public void GetRandomSkill(GameObject player)
    {
        //draw skill number
        skillRan = Random.Range(0, skillNum);

        // Choose the skill you want to test
        skillRan = 0;
       
        if (skillRan == 0) player.AddComponent<BloodField>();
        else if (skillRan == 1) player.AddComponent<ArrowRain>();
        else if (skillRan == 2) player.AddComponent<Bash>();

        else
            Debug.Log("Player didn't get any skil");
        //  GameMgr.Instance.uIMgr.SkillUI(skillRan);

        Debug.Log(skillRan);
    }
}
