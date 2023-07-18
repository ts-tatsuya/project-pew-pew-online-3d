using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

public class RoomItemPopUpDisplay : MonoBehaviourPunCallbacks
{
    [SerializeField]
    private Text _roomNameText;
    [SerializeField]
    private Text _roomStatusText;
    [SerializeField]
    private Text _playerCountText;
    [SerializeField]
    private Button _joinRoomButton;
    [SerializeField]
    private GameObject _joinRoomMenu;
    [SerializeField]
    private GameObject _roomLobbyMenu;

    void Start()
    {
        _joinRoomButton.onClick.AddListener(() =>
        {
            _roomLobbyMenu.SetActive(true);
            PhotonNetwork.JoinRoom(PlayerManager.selectedRoomName);
            _joinRoomMenu.GetComponent<RoomListManager>().ClearRoomItemsList();
            gameObject.SetActive(false);
            _joinRoomMenu.SetActive(false);

        });
        GetComponent<Button>().onClick.AddListener(() =>
        {
            PlayerManager.selectedRoomName = null;
            gameObject.SetActive(false);
        });
    }

    public override void OnJoinedRoom()
    {


        // Dictionary<int, bool> playerReadyData = HelperManager.RoomPlayerReadyCast(PhotonNetwork.CurrentRoom.CustomProperties["playerReadyData"]);
        // playerReadyData.Add(PhotonNetwork.LocalPlayer.ActorNumber, false);
        // ExitGames.Client.Photon.Hashtable roomHash = new ExitGames.Client.Photon.Hashtable();
        // roomHash.Add("playerReadyData", playerReadyData);
        // PhotonNetwork.CurrentRoom.SetCustomProperties(roomHash);

        // string thisPlayer = "";
        // foreach (Player player in PhotonNetwork.CurrentRoom.Players.Values)
        // {
        //     Debug.Log(player.NickName);
        //     if (player.NickName != PhotonNetwork.LocalPlayer.NickName)
        //     {
        //         PlayerManager.playerListTemp.Add(player.NickName);

        //     }
        //     else
        //     {
        //         thisPlayer = player.NickName;
        //     }
        // }
        // PlayerManager.playerListTemp.Add(thisPlayer);

    }

    public void SetPopUpData(string roomName, string playerCount, string roomStatus)
    {
        _roomStatusText.text = "Status: " + roomStatus;
        _roomNameText.text = roomName;
        _playerCountText.text = playerCount + " Players";
    }
}
