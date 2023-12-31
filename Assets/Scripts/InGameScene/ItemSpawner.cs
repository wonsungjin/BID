using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class ItemSpawner : MonoBehaviourPun
{
    [Header("아이템 위치 좌표")]
    [SerializeField] private List<SpawnArea_Ver2> itemAreaPos = null;
    [SerializeField] private List<SpawnArea_Ver2> itemAreaPos2 = null;

    [Header("스페셜 위치 좌표")]
    [SerializeField] private SpawnArea_Ver2 itemSpecialAreaPos = null;

    [Header("아이템들모음")]
    [SerializeField] private GameObject items;
    //SpawnArea_Ver2[] itemSpwanPos;
    //아이템 개수
    [Header("아이템 개수")]
    public int itemMaxCount = 0;

    [SerializeField] private GameObject itemObj;
    //아이템 풀을 담을 큐
    Queue<GameObject> itemQueue = new Queue<GameObject>();

    private List<SpawnArea_Ver2> newSpawnArea = null;
    //아이템
    private int itemCount = 0;
    private int randomItemPos;
    [System.Obsolete]
    private void Start()
    {
        itemObj = new GameObject("ItemObj");
        newSpawnArea = new List<SpawnArea_Ver2>(itemMaxCount);
        if (PhotonNetwork.IsMasterClient)
        {
            RandomItemSpawn(itemMaxCount);
        }

        for (int i = 0; i < itemAreaPos.Count; i++)
        {
            newSpawnArea.Add(itemAreaPos[i]);
        }
        for (int i = 0; i < itemAreaPos2.Count; i++)
        {
            newSpawnArea.Add(itemAreaPos2[i]);
        }


    }
    [PunRPC]
    [System.Obsolete]
    public void RandomItemSpawn(int value)
    {
        for (int i = 0; i < 4; i++)
        {
            Debug.Log("생성됨");
            GameObject box = PhotonNetwork.InstantiateSceneObject("ItemBox", itemSpecialAreaPos.getRandomPos(), Quaternion.identity);
            box.transform.SetParent(itemObj.transform);
            Debug.Log("위치함");
            itemCount++;
        }
        for (int i = 4; i < value; i++)
        {
            int ran = Random.Range(0, 4);
            if (ran == 0 || ran == 1 || ran == 2)
            {
                randomItemPos = Random.Range(0, itemAreaPos.Count);
                GameObject box = PhotonNetwork.InstantiateSceneObject("ItemBox", itemAreaPos[randomItemPos].getRandomPos(), Quaternion.identity);
                box.transform.SetParent(itemObj.transform);
                itemCount++;
            }
            else
            {
                randomItemPos = Random.Range(0, itemAreaPos2.Count);
                GameObject box = PhotonNetwork.InstantiateSceneObject("ItemBox", itemAreaPos2[randomItemPos].getRandomPos(), Quaternion.identity);
                box.transform.SetParent(itemObj.transform);
                itemCount++;
            }
        }
    }



    public void RemoveItemList(GameObject Pos)
    {
        newSpawnArea.Remove(Pos.GetComponent<SpawnArea_Ver2>());
    }
    [PunRPC]
    void ItemRespawn()
    {
        if(itemMaxCount > itemCount)
        {
            newSpawnArea.Clear();
            newSpawnArea.AddRange(FindObjectsOfType<SpawnArea_Ver2>());
            GameObject obj;
            obj = itemQueue.Dequeue();
            int num = Random.Range(0, itemAreaPos.Count);
            randomItemPos = Random.Range(0, newSpawnArea.Count + 1);
            obj.transform.position = newSpawnArea[randomItemPos].getRandomPos();
            obj.gameObject.SetActive(true);
            itemCount++;
        }
        else
        {
            return;
        }

    }


    [PunRPC]
    void ReleasePool(int viewID)
    {
        Debug.Log("ReleasePool");
        if (GameMgr.Instance.PunFindObject(viewID) != null) Release(GameMgr.Instance.PunFindObject(viewID));
    }
    [PunRPC]
    public void Release(GameObject obj)
    {   //큐로 다시 보낸다
        Debug.Log("Release");
        obj.gameObject.SetActive(false);   //플레이어에 닿으면 false시키고 큐에 저장
        itemQueue.Enqueue(obj);
        itemCount--;
        StartCoroutine(TenSec());    //x초 코루틴 실행
    }

    IEnumerator TenSec()
    {
        yield return new WaitForSeconds(30f);

        if (PhotonNetwork.IsMasterClient)
        {
            photonView.RPC("ItemRespawn", RpcTarget.All);
        }
    }
}
