using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using RPG_Indicator;

public class PlayerSpawner : MonoBehaviourPunCallbacks
{
    [SerializeField] private GameObject[] spawnPoint;
    private bool[] playerOn;
    private int spawnPointNum;
    List<GameObject> randomSpawnPoint = new List<GameObject>();

    private void Start()
    {
        playerOn = new bool[spawnPoint.Length];
        Player[] playerList = PhotonNetwork.PlayerList;


        //지우지 마시오!
        /* if (PhotonNetwork.IsMasterClient)
         {
             for (int i = 0; i < playerList.Length; i++)
             {
                 randomSpawnPoint.Add(spawnPoint[RandomArrangeList()]);
             }
          //   photonView.RPC("RandomSpawnPointDistrubuter", RpcTarget.MasterClient, randomSpawnPoint);
         }*/


        Debug.Log(playerList.Length + "vvvvvvvvvvvv");
        for (int i = 0; i < playerList.Length; i++)
        {
            if (playerList[i].NickName == PhotonNetwork.NickName) spawnPointNum = i;
        }
        GameObject player = PhotonNetwork.Instantiate("Player", spawnPoint[spawnPointNum].transform.position, Quaternion.identity);
        GameMgr.Instance.followCam.SetPlayerPos(player.transform);
        GameMgr.Instance.codeExample.PlayerIndicator = player.GetComponentInChildren<RpgIndicator>();

        Debug.Log("생성완료");
    }

    [PunRPC]
    public void RandomSpawnPointDistrubuter(List<GameObject> list)
    {
        randomSpawnPoint = list;
    }

    public int RandomArrangeList()
    {
        int ran = 0;

        while (true)
        {
            ran = Random.Range(0, spawnPoint.Length);
            if (playerOn[ran] == false)
            {
                playerOn[ran] = true;
                break;
            }
        }
        return ran;
    }


    public int PlayerInstantiateLogic()
    {
        int ran = -1;
        while (true)
        {
            ran = Random.Range(0, spawnPoint.Length);
            if (playerOn[ran] == false)
            {
                playerOn[ran] = true;
                break;
            }
        }
        return ran;
    }
}
