using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuController : MonoBehaviour
{

    [Header("Menu Button")]
    [SerializeField]
    private Button _createRoomButton;
    [SerializeField]
    private Button _joinRoomButton;
    [SerializeField]
    private Button _settingButton;
    [Header("Necessary GameObject")]
    [SerializeField]
    private GameObject _createRoomMenu;
    [SerializeField]
    private GameObject _joinRoomMenu;
    [SerializeField]
    private GameObject _settingMenu;

    // Start is called before the first frame update
    void Start()
    {

        _createRoomButton.onClick.AddListener(() =>
        {
            bool isJoinnedLobby = PhotonHelperManager.JoinLobby();
            if (isJoinnedLobby)
            {
                _createRoomMenu.SetActive(true);
                gameObject.SetActive(false);
            }
            else
            {
                Debug.Log("Join Lobby not successful");
            }
        });
        _joinRoomButton.onClick.AddListener(() =>
        {
            bool isJoinnedLobby = PhotonHelperManager.JoinLobby();
            if (isJoinnedLobby)
            {
                _joinRoomMenu.SetActive(true);
                gameObject.SetActive(false);
            }
            else
            {
                Debug.Log("Join Lobby not successful");
            }
        });
        _settingButton.onClick.AddListener(() =>
        {

            _settingMenu.SetActive(true);
            gameObject.SetActive(false);

        });

    }
}
