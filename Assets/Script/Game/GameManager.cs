using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Voice.PUN;

public static class GameManager
{
    public static int SpawnPoint { get; set; }
    public static int SlotNumber { get; set; }
    public static bool IsUIFocused { get; set; }
    public static PhotonVoiceView VoiceView { get; set; }
    public static int PlayerAlive { get; set; }
}
