

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Photon.Pun;

public class ConnectionToServer : MonoBehaviourPunCallbacks
{
    [SerializeField]
    private InputField userNameInput;
    [SerializeField]
    private Text feedbackText;
    [SerializeField]
    private Button connectButton;

    // Start is called before the first frame update
    void Start()
    {
        feedbackText.gameObject.SetActive(false);
        connectButton.onClick.AddListener(() => Connect());
        userNameInput.onValueChanged.AddListener((e) => HideFeedbackText());
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Connect()
    {
        if (userNameInput.text.Length >= 3)
        {
            string nickname = userNameInput.text;
            userNameInput.text = "";
            PhotonNetwork.NickName = nickname;
            PhotonNetwork.QuickResends = 3;
            PhotonNetwork.MaxResendsBeforeDisconnect = 7;
            feedbackText.gameObject.SetActive(true);
            feedbackText.text = "Connecting to server...";
            PhotonNetwork.ConnectUsingSettings();
        }
        else if (userNameInput.text.Length < 3)
        {
            feedbackText.gameObject.SetActive(true);
            feedbackText.text = "Code Name need to have at least 3 characters...";
        }
    }

    public void HideFeedbackText()
    {
        feedbackText.gameObject.SetActive(false);
    }


    public override void OnConnectedToMaster()
    {
        SceneManager.LoadScene("Menu");
    }

}
