using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Voice.PUN;
using Photon.Voice.Unity;
public class VoiceUIDisplay : MonoBehaviour
{
    [Header("Necessary Gameobject")]
    [SerializeField]
    private GameObject speakingIndicator;
    [Header("Necessary Component")]
    [SerializeField]
    private Image voiceIndicator;
    [SerializeField]
    private PhotonVoiceView voiceView;
    [SerializeField]
    private Recorder recorder;
    [Header("Sprites")]
    [SerializeField]
    private Sprite voiceOn;
    [SerializeField]
    private Sprite voiceMute;
    private bool isVoiceOn;
    // Start is called before the first frame update
    void Start()
    {
        isVoiceOn = false;

    }

    // Update is called once per frame
    void Update()
    {
        VoiceHold();
        VoiceUIUpdate();
        if (GameManager.VoiceView != null)
        {
            voiceView = GameManager.VoiceView;
        }
    }

    private void VoiceHold()
    {
        if (
            Input.GetKey(KeyCode.Y) &&
            isVoiceOn == false
        )
        {
            isVoiceOn = true;
        }
        else if (
            Input.GetKeyUp(KeyCode.Y) &&
            isVoiceOn == true
        )
        {
            isVoiceOn = false;
        }
    }

    private void VoiceUIUpdate()
    {
        if (recorder)
        {
            Debug.Log("IS SPEAKING: " + recorder.IsCurrentlyTransmitting);
            if (
                recorder.IsCurrentlyTransmitting &&
                speakingIndicator.activeSelf == false
            )
            {
                speakingIndicator.SetActive(true);
            }
            else if (
                recorder.IsCurrentlyTransmitting == false &&
                speakingIndicator.activeSelf
            )
            {
                speakingIndicator.SetActive(false);
            }

        }
        // if (voiceView)
        // {
        //     Debug.Log("IS SPEAKING: " + voiceView.SpeakerInUse.IsPlaying);
        //     if (
        //         voiceView.IsSpeaking &&
        //         speakingIndicator.activeSelf == false
        //     )
        //     {
        //         speakingIndicator.SetActive(true);
        //     }
        //     else if (
        //         voiceView.IsSpeaking == false &&
        //         speakingIndicator.activeSelf
        //     )
        //     {
        //         speakingIndicator.SetActive(false);
        //     }

        // }

        if (
            isVoiceOn &&
            voiceIndicator.sprite != voiceOn
        )
        {
            voiceIndicator.sprite = voiceOn;
        }
        else if (
            isVoiceOn == false &&
            voiceIndicator.sprite != voiceMute
        )
        {
            voiceIndicator.sprite = voiceMute;
        }
    }
}
