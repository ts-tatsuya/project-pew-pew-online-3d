using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class GameController : MonoBehaviourPunCallbacks
{

    public bool isGameOver;
    public int winnerId;
    public Dictionary<int, GameObject> players = new Dictionary<int, GameObject>();
    [Header("Necessary Gameobject")]
    [SerializeField]
    private GameObject gameOverPopUp;

    private bool gameStarted;

    // Start is called before the first frame update
    void Start()
    {
        gameStarted = false;
        photonView.RPC("RPC_RegisterPlayer", RpcTarget.AllBuffered, 1, gameObject);
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

    public void RegisterPlayer(int actorNumber, GameObject obj)
    {
        Debug.Log("PLAYER REGISTERED");
        players.Add(actorNumber, obj);
        GameManager.PlayerAlive++;
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
    private void RPC_RegisterPlayer(int actorNumber, GameObject obj)
    {
        Debug.Log("PLAYER REGISTERED");
        players.Add(actorNumber, obj);
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
        if (GameManager.PlayerAlive == 1)
        {
            gameStarted = false;
            isGameOver = true;
            return true;
        }
        return false;
    }

    #endregion


    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(players.Keys);
            stream.SendNext(players.Values);
        }
        else if (stream.IsReading)
        {

        }
    }

}
