using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class GameController : MonoBehaviourPunCallbacks
{


    public bool isGameOver;
    public int winnerId;
    public Dictionary<int, GameObject> players = new Dictionary<int, GameObject>();
    public List<GameObject> playerGameobject = new List<GameObject>();
    [Header("Necessary Gameobject")]
    [SerializeField]
    private GameObject gameOverPopUp;
    [SerializeField]
    private GameObject gameResult;

    private bool gameStarted;

    // Start is called before the first frame update
    void Start()
    {
        gameStarted = false;
        //photonView.RPC("RPC_RegisterPlayer", RpcTarget.AllBuffered, 1, GameManager.SlotNumber);
    }

    // Update is called once per frame
    void Update()
    {
        if (
            gameStarted == false &&
            players.Count == 4
        )
        {
            gameStarted = true;
        }
        UpdatePlayerStatus();
        WinCond_LastManStanding();
    }
    #region PublicMethod
    public void UnregisterPlayer(int actorNumber)
    {
        if (actorNumber == PhotonNetwork.LocalPlayer.ActorNumber)
        {
            gameOverPopUp.SetActive(true);
        }
        photonView.RPC("RPC_UnregisterPlayer", RpcTarget.AllBuffered, actorNumber);
    }

    public void RegisterPlayer(int actorNumber)
    {
        Debug.Log("PLAYER REGISTERED");
        // players.Add(actorNumber, obj);
        // playerGameobject.Add(obj);
        // GameManager.PlayerAlive++;
        photonView.RPC(nameof(RPC_RegisterPlayer), RpcTarget.AllBuffered, actorNumber);
    }

    #endregion
    #region PrivateMethod
    private void UpdatePlayerStatus()
    {
        foreach (KeyValuePair<int, GameObject> obj in players)
        {
            if (obj.Value == null)
            {
                UnregisterPlayer(obj.Key);
            }
        }
    }
    #region RPC
    [PunRPC]
    private void RPC_RegisterPlayer(int actorNumber)
    {
        Debug.Log("PLAYER REGISTERED");
        GameObject obj = new GameObject();
        PlayerController[] tempPlayerControllers = FindObjectsOfType<PlayerController>();
        for (int i = 0; i < tempPlayerControllers.Length; i++)
        {
            if (tempPlayerControllers[i].photonView.OwnerActorNr == actorNumber)
            {
                obj = tempPlayerControllers[i].gameObject;
                break;
            }
        }
        players.Add(actorNumber, obj);
        playerGameobject.Add(obj);
        GameManager.PlayerAlive++;
    }

    [PunRPC]
    private void RPC_UnregisterPlayer(int actorNumber)
    {
        players.Remove(actorNumber);
        GameManager.PlayerAlive--;
    }

    #endregion
    #endregion


    #region GameWinCondition

    private bool WinCond_LastManStanding()
    {
        Debug.Log(players.Count);
        if (GameManager.PlayerAlive == 1 && gameStarted == true)
        {
            gameStarted = false;
            isGameOver = true;
            gameResult.SetActive(true);
            return true;
        }
        return false;
    }

    #endregion


    // public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    // {
    //     if (stream.IsWriting)
    //     {
    //         stream.SendNext(players.Keys);
    //         stream.SendNext(players.Values);
    //         stream.SendNext(playerGameobject.ToArray());
    //     }
    //     else if (stream.IsReading)
    //     {
    //         int key = (int)stream.ReceiveNext();
    //         GameObject value = (GameObject)stream.ReceiveNext();
    //         Debug.Log(value.name);
    //         GameObject[] tempObjects = (GameObject[])stream.ReceiveNext();
    //         foreach (GameObject obj in tempObjects)
    //         {
    //             playerGameobject.Add(obj);
    //         }
    //     }
    // }

}
