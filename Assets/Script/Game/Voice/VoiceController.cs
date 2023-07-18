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
    }

    // Update is called once per frame
    void Update()
    {
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
