using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Voice;
using Photon.Voice.PUN;
using Photon.Voice.Unity;
public class VoiceController : MonoBehaviour
{
    [Header("Necessary Component")]
    [SerializeField]
    private PhotonVoiceView voiceView;
    [SerializeField]
    private Recorder recorder;
    // Start is called before the first frame update
    void Start()
    {
        voiceView = GameManager.VoiceView;
        //PunVoiceClient.Instance.ConnectAndJoinRoom();
        recorder = GameObject.FindObjectOfType<Recorder>();
        recorder.TransmitEnabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (voiceView == null && GameManager.VoiceView != null)
        {
            voiceView = GameManager.VoiceView;
        }
        VoiceHold();
    }

    private void VoiceHold()
    {
        if (
            Input.GetKey(KeyCode.Y) &&
            recorder.TransmitEnabled == false
        )
        {
            recorder.TransmitEnabled = true;
        }
        else if (
            Input.GetKeyUp(KeyCode.Y) &&
            recorder.TransmitEnabled
        )
        {
            recorder.TransmitEnabled = false;
        }
    }
}
