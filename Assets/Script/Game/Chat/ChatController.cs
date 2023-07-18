using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Chat;
using Photon.Pun;
using ExitGames.Client.Photon;

public class ChatController : MonoBehaviour, IChatClientListener
{

    [Header("Necessary Component")]
    [SerializeField]
    private Text chatRecord;
    [SerializeField]
    private InputField inputField;
    [SerializeField]
    private Scrollbar scrollbar;
    [SerializeField]
    private ScrollRect scrollRect;
    [Header("Setting for Debug")]
    [SerializeField]
    private bool isDebugging;
    private ChatClient chatClient;
    private string userId;
    private string nickname;
    private string channelNameTemplate;



    #region EssentialUnityMethod
    // Start is called before the first frame update
    void Awake()
    {
        chatRecord.text = "";
    }
    void Start()
    {
        inputField.onEndEdit.AddListener((val) =>
        {
            if (
                Input.GetKeyDown(KeyCode.Return) &&
                (
                    Input.GetKey(KeyCode.LeftShift) == false &&
                    Input.GetKey(KeyCode.RightShift) == false
                )
            )
            {
                if (val != "")
                {
                    SendMessageToChannel(val);
                }
            }
        });

    }

    // Update is called once per frame
    void Update()
    {
        if (PhotonNetwork.IsConnectedAndReady)
        {
            try
            {
                chatClient.Service();
            }
            catch (NullReferenceException ex)
            {
                if (ex is NullReferenceException)
                {
                    PhotonChatInit();
                }
            }

        }
    }
    #endregion

    #region PublicMethod

    public void SendMessageToChannel(string message)
    {

        if (PhotonNetwork.IsConnected && chatClient.CanChat)
        {
            Debug.Log("SENDING MESSAGE");
            chatClient.PublishMessage(channelNameTemplate, message);
            inputField.text = "";
        }
    }

    #endregion

    #region PrivateMethod
    private void PhotonChatInit()
    {
        Debug.Log(PhotonNetwork.InLobby);
        if (PhotonNetwork.InLobby || PhotonNetwork.InRoom)
        {

            if (PhotonNetwork.IsConnectedAndReady)
            {
                if (isDebugging)
                {

                    channelNameTemplate = "DebugChat";
                }
                else
                {
                    channelNameTemplate =
                        PhotonNetwork.CurrentRoom.Name +
                        "$" +
                        PhotonNetwork.CurrentRoom.CustomProperties["CreatorId"];

                }
                chatClient = new ChatClient(this);
                userId = PhotonNetwork.LocalPlayer.UserId;
                nickname = PhotonNetwork.LocalPlayer.NickName;
                chatClient.Connect(
                    PhotonNetwork.PhotonServerSettings.AppSettings.AppIdChat,
                    PhotonNetwork.AppVersion,
                    new AuthenticationValues(userId + "$" + nickname)
                );
                Debug.Log(userId);
            }
        }
    }
    #endregion



    #region PhotonChatMethod
    public void OnConnected()
    {
        chatClient.Subscribe(new string[]
        {
            channelNameTemplate
        });
        //throw new System.NotImplementedException();
    }

    public void OnDisconnected()
    {
        chatClient.Unsubscribe(new string[]
        {
            channelNameTemplate
        });
        throw new System.NotImplementedException();
    }

    public void OnGetMessages(string channelName, string[] senders, object[] messages)
    {
        if (channelName == channelNameTemplate)
        {
            for (int i = 0; i < senders.Length; i++)
            {
                Debug.Log(senders[i].Split('$')[1]);
                string senderName = senders[i].Split('$')[1];
                Debug.Log($"{senderName}: {messages[i]}");
                chatRecord.text += $"{senderName}: {messages[i]}\n";

            }

        }
        //throw new System.NotImplementedException();
    }

    #region UnusedPhotonChatMethod
    public void DebugReturn(DebugLevel level, string message)
    {
        //throw new System.NotImplementedException();
    }

    public void OnChatStateChange(ChatState state)
    {
        //throw new System.NotImplementedException();
    }
    public void OnPrivateMessage(string sender, object message, string channelName)
    {
        //throw new System.NotImplementedException();
    }

    public void OnStatusUpdate(string user, int status, bool gotMessage, object message)
    {
        //throw new System.NotImplementedException();
    }
    public void OnSubscribed(string[] channels, bool[] results)
    {
        //throw new System.NotImplementedException();
    }

    public void OnUnsubscribed(string[] channels)
    {
        //throw new System.NotImplementedException();
    }

    public void OnUserSubscribed(string channel, string user)
    {
        //throw new System.NotImplementedException();
    }

    public void OnUserUnsubscribed(string channel, string user)
    {
        //throw new System.NotImplementedException();
    }
    #endregion
    #endregion





}
