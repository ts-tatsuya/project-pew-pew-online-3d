using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class ConnectToServerForDebug : MonoBehaviourPunCallbacks
{
    private enum DebugNickname
    {
        DebuggerAlpha,
        DebuggerBeta,
        DebuggerDelta,
        DebuggerEpsilon,
        DebuggerGamma,
        DebuggerLambda,
        DebuggerZeta
    }
    [Header("Setting For Debug")]
    [SerializeField]
    private bool isDebugging;
    [SerializeField]
    private DebugNickname nickname;
    // Start is called before the first frame update
    void Awake()
    {
        if (isDebugging)
        {
            PhotonNetwork.NickName = nickname.ToString();
            PhotonNetwork.QuickResends = 3;
            PhotonNetwork.MaxResendsBeforeDisconnect = 7;
            PhotonNetwork.ConnectUsingSettings();
        }
    }
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public override void OnConnectedToMaster()
    {
        if (PhotonNetwork.IsConnectedAndReady)
        {
            PhotonNetwork.JoinLobby();
        }
    }
}
