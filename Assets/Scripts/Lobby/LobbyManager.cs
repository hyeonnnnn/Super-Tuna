using UnityEngine;

public class LobbyManager : SingletonBehaviour<LobbyManager>
{
    public LobbyUiController LobbyUIController { get; private set; }

    protected override void Init()
    {
        IsDestroyOnLoad = false;

        base.Init();
    }

    private void Start()
    {
        LobbyUIController = FindAnyObjectByType<LobbyUiController>();
        
        if(LobbyUIController != null )
        {
            AudioManager.Instance.Play(AudioType.BGM, "Lobby");
            return;
        }

        LobbyUIController.Init();
    }
}
