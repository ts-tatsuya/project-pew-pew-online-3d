using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Photon.Pun;

public class AvatarSelectionMenuDisplay : MonoBehaviour
{

    [SerializeField]
    private Text _selectedAvatarName;
    [SerializeField]
    private GameObject _roomLobbyMenu;
    [SerializeField]
    private SpotLightHighlightController _spotLightHighlightController;
    [SerializeField]
    private RawImage _avatarViewDisplay;
    [SerializeField]
    private RenderTexture _avatarViewTexture;
    [SerializeField]
    private List<Button> _avatarSelectionButtonList = new List<Button>();
    [SerializeField]
    private Button _confirmAvatar;

    public bool isCleaningUp;
    // Start is called before the first frame update
    private void Awake()
    {
        isCleaningUp = false;
        foreach (Button avatarButton in _avatarSelectionButtonList)
        {
            avatarButton.interactable = false;
        }
        _confirmAvatar.onClick.AddListener(ConfirmAvatar);

    }

    private void OnEnable()
    {
        _avatarViewDisplay.gameObject.SetActive(true);
        _selectedAvatarName.text = GameMetaDataManager.avatarNames[GameManager.PlayerAvatarId];
    }

    public IEnumerator LoadAvatarView()
    {
        isCleaningUp = true;
        AsyncOperation loadAvatarView = SceneManager.LoadSceneAsync("CharacterView", LoadSceneMode.Additive);
        yield return new WaitUntil(() => loadAvatarView.isDone);
        _spotLightHighlightController = FindObjectOfType<SpotLightHighlightController>();
        _avatarViewDisplay.texture = _avatarViewTexture;
        yield return new WaitUntil(() => _spotLightHighlightController != null);
        UpdateAvatarSelectionButton(true);
        isCleaningUp = false;
    }

    public void UnloadAvatarView()
    {
        StartCoroutine(UnloadAvatarViewCoroutine());
    }
    public IEnumerator UnloadAvatarViewCoroutine()
    {
        isCleaningUp = true;
        UpdateAvatarSelectionButton(false);
        _spotLightHighlightController = null;
        _avatarViewDisplay.texture = null;
        AsyncOperation unloadAvatarView = SceneManager.UnloadSceneAsync(SceneManager.GetSceneByName("CharacterView").buildIndex);
        yield return new WaitUntil(() => unloadAvatarView.isDone);
        isCleaningUp = false;
        _roomLobbyMenu.SetActive(true);
        gameObject.SetActive(false);

    }

    public void AvatarSelectionButton(int buttonId)
    {
        GameManager.PlayerAvatarId = buttonId;
        _selectedAvatarName.text = GameMetaDataManager.avatarNames[GameManager.PlayerAvatarId];
        _spotLightHighlightController.ChangeHighlightPosition(GameManager.PlayerAvatarId);

    }

    private void UpdateAvatarSelectionButton(bool isEnabled)
    {
        for (int i = 0; i < _avatarSelectionButtonList.Count; i++)
        {
            Debug.Log(i);
            int currentI = i;
            _avatarSelectionButtonList[i].onClick.AddListener(() => AvatarSelectionButton(currentI));
            _avatarSelectionButtonList[i].interactable = true;
        }
    }

    private void ConfirmAvatar()
    {
        _avatarViewDisplay.gameObject.SetActive(false);
        _roomLobbyMenu.GetComponent<RoomManager>().AvatarChangeButton();
    }
}
