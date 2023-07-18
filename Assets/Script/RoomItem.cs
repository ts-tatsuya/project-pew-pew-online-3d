using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class RoomItem : MonoBehaviour
{
    [SerializeField]
    private Text _roomNameDisplay;
    [SerializeField]
    private Text _currPlayerDisplay;
    private GameObject _roomItemInfoPopUp;
    private string _roomStatus;

    void Start()
    {
        GetComponent<Button>().onClick.AddListener(() => ToggleDisplayRoomInfo());
    }

    public void SetRoomInfo(string roomName, int currPlayer, int maxPlayer, string roomStatus)
    {
        _roomStatus = roomStatus;
        _roomNameDisplay.text = roomName;
        _currPlayerDisplay.text = currPlayer + "/" + maxPlayer;
    }

    public void SetDisplay(GameObject roomItemInfoPopUp)
    {
        _roomItemInfoPopUp = roomItemInfoPopUp;
    }

    // public void JoinRoom()
    // {
    //     PhotonNetwork.JoinRoom(_roomNameDisplay.text);
    //     //_roomItemInfoPopUp.SetActive(true);
    // }

    public void ToggleDisplayRoomInfo()
    {
        PlayerManager.selectedRoomName = _roomNameDisplay.text;
        Debug.Log("togglepopuptriggered");
        Debug.Log(_roomStatus);
        _roomItemInfoPopUp.GetComponent<RoomItemPopUpDisplay>()
        .SetPopUpData(_roomNameDisplay.text, _currPlayerDisplay.text, _roomStatus);
        switch (_roomItemInfoPopUp.activeSelf)
        {
            case true:
                _roomItemInfoPopUp.SetActive(false);
                break;
            case false:
                _roomItemInfoPopUp.SetActive(true);
                break;
        }
    }

}
