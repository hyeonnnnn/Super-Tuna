using UnityEngine;

public class InGameManager : SingletonBehaviour<InGameManager>
{
    public InGameUIController _InGameUIController { get; private set; }

    protected override void Init()
    {
        IsDestroyOnLoad = false;

        base.Init();
    }

    private void Start()
    {
        _InGameUIController = FindAnyObjectByType<InGameUIController>();

        if (_InGameUIController != null)
        {
            AudioManager.Instance.Play(AudioType.BGM, "InGame");
            return;
        }

        _InGameUIController.Init();
    }
}
