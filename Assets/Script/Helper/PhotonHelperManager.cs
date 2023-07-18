using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public static class PhotonHelperManager
{
    public static bool JoinLobby()
    {
        if (PhotonNetwork.IsConnectedAndReady)
        {
            if (PhotonNetwork.InLobby == false)
            {

                PhotonNetwork.JoinLobby();
                Debug.Log("JOINED LOBBY");
                return true;
            }
            else
            {
                return false;
            }
        }
        else
        {
            return false;
        }
    }
    public static bool LeaveLobby()
    {
        if (PhotonNetwork.InLobby == true)
        {
            PhotonNetwork.LeaveLobby();
            Debug.Log("LEFT FROM LOBBY");
            return true;
        }
        else
        {
            return false;
        }
    }
}
