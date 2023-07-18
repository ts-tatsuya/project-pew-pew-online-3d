using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
public class PlayerConnectionStatusManager : MonoBehaviourPunCallbacks
{
    private enum ConnectionStatus
    {
        GREEN,
        YELLOW,
        RED
    }
    [SerializeField]
    private Text userNameDisplay;
    [SerializeField]
    private Text regionDisplay;
    [SerializeField]
    private Text pingDisplay;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        userNameDisplay.text = "Code Name: " + PhotonNetwork.NickName;
        regionDisplay.text = "Region: " + PhotonNetwork.CloudRegion.ToUpper();
        switch (ConnectionHealth())
        {
            case ConnectionStatus.GREEN:
            default:
                pingDisplay.text = "ALL " + ConnectionStatus.GREEN;
                pingDisplay.color = Color.green;
                break;
            case ConnectionStatus.YELLOW:
                pingDisplay.text = "CODE " + ConnectionStatus.YELLOW;
                pingDisplay.color = Color.yellow;
                break;
            case ConnectionStatus.RED:
                pingDisplay.text = "CODE " + ConnectionStatus.RED;
                pingDisplay.color = Color.red;
                break;
        }
        //Debug.Log(PhotonNetwork.GetPing());
    }

    private ConnectionStatus ConnectionHealth()
    {
        if (PhotonNetwork.GetPing() <= 50)
        {
            return ConnectionStatus.GREEN;
        }
        else if (PhotonNetwork.GetPing() <= 100)
        {
            return ConnectionStatus.YELLOW;
        }
        else
        {
            return ConnectionStatus.RED;
        }
    }
}
