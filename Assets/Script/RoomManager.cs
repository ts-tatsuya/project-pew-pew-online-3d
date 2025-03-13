using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

public class RoomManager : MonoBehaviourPunCallbacks
{
    [Header("Room Info")]
    [SerializeField]
    private Text _roomNameDisplay;
    [SerializeField]
    private Text _roomPlayerDisplay;
    [Header("Player List")]
    [SerializeField]
    private GameObject _playerItemPrefab;
    [SerializeField]
    private GameObject _playerListViewportContent;
    [Header("Buttons")]
    [SerializeField]
    private Button _startReadyButton;
    [SerializeField]
    private Button _leaveButton;
    [SerializeField]
    private Button _avatarSelectionButton;
    [Header("View")]
    [SerializeField]
    private GameObject _mainMenu;
    [SerializeField]
    private GameObject _joinRoomMenu;
    [SerializeField]
    private GameObject _hostLeaveRoomNotification;
    [SerializeField]
    private GameObject _avatarSelectionMenu;
    private int _roomPlayerId;
    private Color _startReadyButtonDefaultColor;
    // private Dictionary<int, Player> _listPlayer;
    private Dictionary<int, Player> _listPlayer;
    public List<string> playerListTemp = new List<string>();
    private List<GameObject> _playerInfoItem = new List<GameObject>();
    [Header("PlayerReadyData Visual")]
    [SerializeField]
    private bool[] _playerReadyData;
    [Header("PlayerAvatarIdData Visual")]
    [SerializeField]
    private int[] _avatarIdData;
    void Start()
    {
        _startReadyButton.onClick.AddListener(() => StartReadyButton());
        _leaveButton.onClick.AddListener(() => LeaveRoomButton());
        _startReadyButtonDefaultColor = _startReadyButton.GetComponent<Image>().color;
        _avatarSelectionButton.onClick.AddListener(() =>
        {
            _avatarSelectionMenu.SetActive(true);
        });


    }
    public override void OnEnable()
    {
        //Debug.Log(PhotonNetwork.CurrentRoom.Players.Keys);
        base.OnEnable();
        // if (PhotonNetwork.IsMasterClient)
        // {
        //     _startReadyButton.GetComponentInChildren<Text>().text = "Start";
        // }
        // else
        // {
        //     _startReadyButton.GetComponentInChildren<Text>().text = "Ready";
        // }
        Debug.Log(PhotonNetwork.CurrentRoom);


    }

    public void LeaveRoomButton()
    {
        Debug.Log("leave room");

        if (PhotonNetwork.IsConnectedAndReady)
        {

            if (PhotonNetwork.IsMasterClient)
            {
                _mainMenu.SetActive(true);
                PhotonNetwork.LeaveRoom();
            }
            else
            {
                _joinRoomMenu.SetActive(true);
                PhotonNetwork.LeaveRoom();
                //playerListTemp.Clear();

            }
        }
        //gameObject.SetActive(false);

    }
    public void StartReadyButton()
    {
        PhotonNetwork.AutomaticallySyncScene = true;
        // ExitGames.Client.Photon.Hashtable currProp = PhotonNetwork.LocalPlayer.CustomProperties;
        // currProp["IsReady"] = true;
        // PhotonNetwork.LocalPlayer.SetCustomProperties(currProp);
        //CheckPlayerReadyCount();

        if (PhotonNetwork.IsMasterClient)
        {

            // ExitGames.Client.Photon.Hashtable roomHash = PhotonNetwork.CurrentRoom.CustomProperties as ExitGames.Client.Photon.Hashtable;
            // bool[] temp = (bool[])roomHash["playerReadyData"];
            // Debug.Log(temp[1]);
            // temp[1] = true;
            // roomHash["playerReadyData"] = temp;
            // PhotonNetwork.CurrentRoom.SetCustomProperties(roomHash);
            _playerReadyData = (bool[])PhotonNetwork.CurrentRoom.CustomProperties["playerReadyData"];
            bool isAllReady = true;
            if (playerListTemp.Count != _playerReadyData.Length)
            {
                Debug.Log("NOT ENOUGH PLAYER");
                return;
            }
            foreach (var isReady in _playerReadyData)
            {
                if (isReady == false)
                {
                    isAllReady = false;
                    break;
                }
            }
            if (isAllReady)
            {
                Debug.Log("GAME STARTING");
                PhotonNetwork.LoadLevel("Game");
            }
            else
            {
                Debug.Log("NOT ALL PLAYER IS READY");
                Debug.Log(_playerReadyData);
            }
        }
        else
        {

            _playerReadyData = (bool[])PhotonNetwork.CurrentRoom.CustomProperties["playerReadyData"];
            if (_playerReadyData[playerListTemp.IndexOf(PhotonNetwork.LocalPlayer.ActorNumber.ToString())])
            {
                _playerReadyData[playerListTemp.IndexOf(PhotonNetwork.LocalPlayer.ActorNumber.ToString())] = false;
                _startReadyButton.GetComponent<Image>().color = _startReadyButtonDefaultColor;
            }
            else
            {
                _playerReadyData[playerListTemp.IndexOf(PhotonNetwork.LocalPlayer.ActorNumber.ToString())] = true;
                _startReadyButton.GetComponent<Image>().color = Color.green;
            }
            ExitGames.Client.Photon.Hashtable roomHash = new ExitGames.Client.Photon.Hashtable();
            roomHash.Add("playerReadyData", _playerReadyData);
            PhotonNetwork.CurrentRoom.SetCustomProperties(roomHash);
            UpdatePlayerList();
        }

    }
    public void AvatarChangeButton()
    {
        Debug.Log(GameManager.PlayerAvatarId);
        _avatarIdData = (int[])PhotonNetwork.CurrentRoom.CustomProperties["playerAvatarIdData"];
        _avatarIdData[playerListTemp.IndexOf(PhotonNetwork.LocalPlayer.ActorNumber.ToString())] = GameManager.PlayerAvatarId;
        ExitGames.Client.Photon.Hashtable roomHash = PhotonNetwork.CurrentRoom.CustomProperties;
        roomHash["playerAvatarIdData"] = _avatarIdData;
        PhotonNetwork.CurrentRoom.SetCustomProperties(roomHash);
        UpdatePlayerList();
    }
    private void CheckPlayerReadyCount()
    {
        int playerReadyCount = 0;
        foreach (Player player in PhotonNetwork.CurrentRoom.Players.Values)
        {

            if ((bool)player.CustomProperties["IsReady"])
            {
                playerReadyCount++;
            }

        }
        Debug.Log(playerReadyCount);
    }
    private void UpdatePlayerReadyData()
    {
        for (int i = 0; i < _playerReadyData.Length; i++)
        {
            if (i < PhotonNetwork.CurrentRoom.Players.Count)
            {
                if (
                    PhotonNetwork.CurrentRoom.GetPlayer(int.Parse(playerListTemp[i])) != null &&
                    PhotonNetwork.CurrentRoom.GetPlayer(int.Parse(playerListTemp[i])).IsMasterClient
                )
                {
                    _playerReadyData[i] = true;
                }
                else if (i != _playerReadyData.Length - 1)
                {
                    _playerReadyData[i] = _playerReadyData[i + 1];
                }
                else
                {
                    _playerReadyData[i] = false;
                }

            }
            else
            {
                _playerReadyData[i] = false;
            }
        }
        ExitGames.Client.Photon.Hashtable roomHash = PhotonNetwork.CurrentRoom.CustomProperties as ExitGames.Client.Photon.Hashtable;
        // roomHash.Add("playerReadyData", _playerReadyData);
        roomHash["playerReadyData"] = _playerReadyData;
        PhotonNetwork.CurrentRoom.SetCustomProperties(roomHash);
    }
    void Update()
    {
        if (PhotonNetwork.CurrentRoom != null)
        {
            // Debug.Log("Player count: " + PhotonNetwork.CurrentRoom.PlayerCount);
            // if (_listPlayer != PhotonNetwork.CurrentRoom.Players)
            // {
            //     //SetRoomPlayerId();
            //     Debug.Log("UPDATING LIST");
            //     //UpdatePlayerList();
            // }

            if (
                _playerInfoItem.Count != playerListTemp.Count ||
                _playerReadyData != (bool[])PhotonNetwork.CurrentRoom.CustomProperties["playerReadyData"]
            )
            {
                UpdatePlayerList();
            }

            if (_roomNameDisplay.text != PhotonNetwork.CurrentRoom.Name)
            {
                _roomNameDisplay.text = PhotonNetwork.CurrentRoom.Name;
            }
            string playerCountTextTemplate =
                PhotonNetwork.CurrentRoom.Players.Count +
                "/" +
                (int)PhotonNetwork.CurrentRoom.MaxPlayers +
                " Players";
            if (_roomPlayerDisplay.text != playerCountTextTemplate)
            {
                _roomPlayerDisplay.text = playerCountTextTemplate;

            }
        }
    }

    private void SetRoomPlayerId()
    {
        for (int i = 0; i < PhotonNetwork.CurrentRoom.Players.Count; i++)
        {
            if (PhotonNetwork.CurrentRoom.Players[i + 1] == PhotonNetwork.LocalPlayer)
            {
                _roomPlayerId = i;
            }
        }
    }

    public override void OnJoinedRoom()
    {
        Debug.Log(PhotonNetwork.CurrentRoom.CustomProperties["CreatorId"]);
        Debug.Log("OnJoinROOM");
        string thisPlayer = "";
        foreach (Player player in PhotonNetwork.CurrentRoom.Players.Values)
        {
            Debug.Log(player.ActorNumber.ToString());
            if (player.ActorNumber.ToString() != PhotonNetwork.LocalPlayer.ActorNumber.ToString())
            {
                playerListTemp.Add(player.ActorNumber.ToString());

            }
            else
            {
                thisPlayer = player.ActorNumber.ToString();
            }
        }
        GameManager.SlotNumber = playerListTemp.Count;
        playerListTemp.Add(thisPlayer);
        _playerReadyData = (bool[])PhotonNetwork.CurrentRoom.CustomProperties["playerReadyData"];
        if (PhotonNetwork.IsMasterClient)
        {
            _startReadyButton.GetComponentInChildren<Text>().text = "Start";
        }
        else
        {
            _startReadyButton.GetComponentInChildren<Text>().text = "Ready";
        }
        UpdatePlayerList();

    }
    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        //base.OnPlayerEnteredRoom(newPlayer);
        Debug.Log(newPlayer.NickName);
        playerListTemp.Add(newPlayer.ActorNumber.ToString());
        UpdatePlayerList();
    }
    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        base.OnPlayerLeftRoom(otherPlayer);
        RemoveFromPlayerList(otherPlayer.ActorNumber);
        UpdatePlayerReadyData();
        UpdatePlayerList();
        Debug.Log(otherPlayer.ActorNumber.ToString() + "is left");

    }
    private void RemoveFromPlayerList(int actorNumber)
    {
        _playerReadyData[(int)playerListTemp.IndexOf(actorNumber.ToString())] = false;
        // if (playerListTemp.IndexOf(actorNumber.ToString()) == 0)
        // {
        //     _hostLeaveRoomNotification.SetActive(true);
        //     PhotonNetwork.LeaveLobby();
        //     _joinRoomMenu.SetActive(true);
        //     playerListTemp.Clear();
        //     gameObject.SetActive(false);
        // }
        playerListTemp.Remove(actorNumber.ToString());
        UpdatePlayerList();
        if (PhotonNetwork.IsMasterClient)
        {
            _startReadyButton.GetComponentInChildren<Text>().text = "Start";
        }
        else
        {
            _startReadyButton.GetComponentInChildren<Text>().text = "Ready";
        }
    }
    public override void OnLeftRoom()
    {
        base.OnLeftRoom();
        playerListTemp.Clear();
        ClearPlayerInfoItemObjects();
        _playerReadyData = null;
        gameObject.SetActive(false);


    }
    private void ClearPlayerInfoItemObjects()
    {
        foreach (GameObject item in _playerInfoItem)
        {
            Destroy(item);
        }
    }
    void UpdatePlayerList()
    {
        ClearPlayerInfoItemObjects();
        _playerInfoItem.Clear();
        foreach (string playerId in playerListTemp)
        {
            Player tempPlayer = PhotonNetwork.CurrentRoom.GetPlayer(int.Parse(playerId));
            _playerReadyData = (bool[])PhotonNetwork.CurrentRoom.CustomProperties["playerReadyData"];
            _avatarIdData = (int[])PhotonNetwork.CurrentRoom.CustomProperties["playerAvatarIdData"];
            _playerInfoItem.Add(Instantiate(_playerItemPrefab));
            _playerInfoItem[_playerInfoItem.Count - 1].GetComponent<PlayerItemDisplay>().text.text = tempPlayer.NickName;
            _playerInfoItem[_playerInfoItem.Count - 1].transform.SetParent(_playerListViewportContent.transform);
            _playerInfoItem[_playerInfoItem.Count - 1].transform.localScale = new Vector3(1, 1, 1);
            Debug.Log("PLAYER INDEX: " + playerListTemp.IndexOf(tempPlayer.ActorNumber.ToString()));
            switch (_playerReadyData[playerListTemp.IndexOf(tempPlayer.ActorNumber.ToString())])
            {
                case true:
                    _playerInfoItem[_playerInfoItem.Count - 1].GetComponent<PlayerItemDisplay>().isReadyIndicator.color = Color.green;
                    break;
                case false:
                    _playerInfoItem[_playerInfoItem.Count - 1].GetComponent<PlayerItemDisplay>().isReadyIndicator.color = Color.red;
                    break;
            }
            _playerInfoItem[_playerInfoItem.Count - 1].GetComponent<PlayerItemDisplay>().avatarUsedName.text =
            "As: " +
            GameMetaDataManager.avatarNames[_avatarIdData[playerListTemp.IndexOf(tempPlayer.ActorNumber.ToString())]];
        }
    }
}
