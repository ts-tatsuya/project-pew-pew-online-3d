using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;

public class CreateRoomManager : MonoBehaviourPunCallbacks
{

    [SerializeField]
    private InputField _roomNameInputField;
    [Header("Display")]
    [SerializeField]
    private GameObject _roomScreen;
    [SerializeField]
    private GameObject _createRoomScreen;
    [SerializeField]
    private GameObject _mainMenuScreen;
    [Header("Buttons")]
    [SerializeField]
    private Button _createRoomButton;
    [SerializeField]
    private Button _backButton;

    void Start()
    {
        //PhotonNetwork.JoinLobby();
        _createRoomButton.onClick.AddListener(() => CreateRoom());
        _backButton.onClick.AddListener(() =>
        {
            bool isLeftFromLobby = PhotonHelperManager.LeaveLobby();
            if (isLeftFromLobby)
            {
                _mainMenuScreen.SetActive(true);
                gameObject.SetActive(false);
            }
        });
    }

    public void CreateRoom()
    {
        var roomOption = new RoomOptions
        {
            MaxPlayers = 4,
            EmptyRoomTtl = 0,
            IsVisible = true,
            IsOpen = true
        };
        roomOption.CustomRoomProperties = new ExitGames.Client.Photon.Hashtable();
        roomOption.CustomRoomProperties.Add("playerReadyData", new bool[4] { true, false, false, false });
        roomOption.CustomRoomProperties.Add("CreatorId", PhotonNetwork.LocalPlayer.UserId);
        if (_roomNameInputField.text.Length > 0)
        {
            _roomScreen.SetActive(true);
            string roomName = _roomNameInputField.text;
            _roomNameInputField.text = "";
            PhotonNetwork.CreateRoom(roomName, roomOption);
            _createRoomScreen.SetActive(false);
        }
    }

    public override void OnJoinedRoom()
    {


    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        base.OnJoinRoomFailed(returnCode, message);
        Debug.Log(returnCode);
        Debug.Log(message);
    }


}
