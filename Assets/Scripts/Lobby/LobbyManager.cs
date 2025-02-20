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
            return;
        }

        LobbyUIController.Init();
        //브금 소리 재생
    }


}
