using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;


public class RoomListManager : MonoBehaviourPunCallbacks
{
    [Header("Necessary GameObject")]
    [SerializeField]
    private GameObject _roomItemPrefab;
    [SerializeField]
    private GameObject _joinRoomScreen;
    [SerializeField]
    private GameObject _roomItemInfoPopUp;
    [SerializeField]
    private GameObject _mainMenuScreen;
    [Header("Buttons")]
    [SerializeField]
    private Button _refreshButton;
    [SerializeField]
    private Button _backButton;
    [Header("SpawnPoint")]
    [SerializeField]
    private GameObject _roomItemContent;
    private List<RoomInfo> _roomInfoList = new List<RoomInfo>();
    private List<GameObject> _roomItemsList = new List<GameObject>();

    void Start()
    {
        _refreshButton.onClick.AddListener(() => UpdateRoomItemList());
        _backButton.onClick.AddListener(() =>
        {
            bool isLeftFromLobby = PhotonHelperManager.LeaveLobby();
            if (isLeftFromLobby)
            {
                _mainMenuScreen.SetActive(true);
                ClearRoomItemsList();
                gameObject.SetActive(false);
            }
        });

    }

    public override void OnEnable()
    {
        base.OnEnable();
        UpdateRoomItemList();
    }

    public void ClearRoomItemsList()
    {
        foreach (GameObject item in _roomItemsList)
        {
            Destroy(item);
        }
    }
    public void UpdateRoomItemList()
    {
        //Debug.Log(roomList[0]);
        ClearRoomItemsList();
        //Debug.Log(roomList[0]);
        foreach (RoomInfo roomInfo in _roomInfoList)
        {
            // if (roomInfo.IsVisible)
            // {
            string roomStatus;
            switch (roomInfo.IsOpen)
            {
                case true:
                default:
                    roomStatus = "Open";
                    break;
                case false:
                    roomStatus = "Closed";
                    break;
            }
            //Debug.Log(roomStatus);
            _roomItemsList.Add(Instantiate(_roomItemPrefab, _roomItemContent.transform));
            _roomItemsList[_roomItemsList.Count - 1].GetComponent<RoomItem>()
            .SetDisplay(_roomItemInfoPopUp);
            _roomItemsList[_roomItemsList.Count - 1].GetComponent<RoomItem>()
            .SetRoomInfo(roomInfo.Name, roomInfo.PlayerCount, roomInfo.MaxPlayers, roomStatus);

            //}

        }
    }
    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        _roomInfoList = roomList;
        UpdateRoomItemList();
    }

}
