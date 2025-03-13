using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Voice.PUN;

[RequireComponent(typeof(PhotonView))]
public class PlayerSpawnerController : MonoBehaviourPunCallbacks, IPunObservable
{
    [Header("Necessary Component")]
    [SerializeField]
    private GameController gameController;
    [Header("Spawn Config")]
    [SerializeField]
    private List<Transform> spawnPointList = new List<Transform>();
    private List<int> playerSpawnPoint = new List<int>();
    private bool isRandomized;
    private bool isSpawnedMine;
    // Start is called before the first frame update
    void Start()
    {
        isSpawnedMine = false;
        if (photonView.IsMine)
        {
            if (PhotonNetwork.IsMasterClient)
            {
                photonView.RPC("RPC_RandomizeSpawn", RpcTarget.AllBuffered);
            }
        }
    }

    [PunRPC]
    private void RPC_RandomizeSpawn()
    {
        for (int i = 0; i < PhotonNetwork.CurrentRoom.MaxPlayers; i++)
        {
            playerSpawnPoint.Add(RandomSpawnIndex());
        }
    }
    private int RandomSpawnIndex()
    {
        int spawnIndex = Random.Range(1, spawnPointList.Count + 1);
        if (playerSpawnPoint.IndexOf(spawnIndex) != -1)
        {
            return RandomSpawnIndex();
        }
        else
        {
            return spawnIndex;
        }
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            Debug.Log("ISWRITING");
            int[] playerSpawnPointTemp = new int[4]{
                playerSpawnPoint[0],
                playerSpawnPoint[1],
                playerSpawnPoint[2],
                playerSpawnPoint[3]
            };
            stream.SendNext(playerSpawnPointTemp);
        }
        else if (stream.IsReading)
        {
            int[] playerSpawnPointTemp = (int[])stream.ReceiveNext();
            playerSpawnPoint = new List<int>(){
                playerSpawnPointTemp[0],
                playerSpawnPointTemp[1],
                playerSpawnPointTemp[2],
                playerSpawnPointTemp[3],
            };

        }
        if (isSpawnedMine == false)
        {
            Transform spawnPointBeingUsed = spawnPointList[playerSpawnPoint[GameManager.SlotNumber] - 1];
            int[] avatarIdData = (int[])PhotonNetwork.CurrentRoom.CustomProperties["playerAvatarIdData"];
            GameObject spawnedPlayer = PhotonNetwork.Instantiate(
                GameMetaDataManager.avatarAssetPath[avatarIdData[GameManager.SlotNumber]],
                spawnPointBeingUsed.position,
                spawnPointBeingUsed.rotation
                );
            isSpawnedMine = true;
            Camera.main.GetComponent<CameraController>().player = spawnedPlayer.transform;
            GameManager.VoiceView = spawnedPlayer.GetComponent<PhotonVoiceView>();
            gameController.RegisterPlayer(PhotonNetwork.LocalPlayer.ActorNumber);
        }


    }


}
