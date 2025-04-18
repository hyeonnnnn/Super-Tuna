using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class TitleManager : MonoBehaviour
{
    [SerializeField] private Animation _logoAnim;
    [SerializeField] private Image _logoImg;
    [SerializeField] private GameObject _title;
    [SerializeField] private TextMeshProUGUI _progressText;

    private void Awake()
    {
        _logoAnim.gameObject.SetActive(true);
        _title.SetActive(false);
    }
    private void Start()
    {
        if(UserDataManager.Instance.ExistsSavedData)
        {
            UserDataManager.Instance.LoadUserData();
        }
        else
        {
            UserDataManager.Instance.SetDefaultData();
            UserDataManager.Instance.SaveUserData();
        }

        AudioManager.Instance.SyncUserSettings();

        StartCoroutine(LoadingSequence());
    }

    private IEnumerator LoadingSequence()
    {
        _logoAnim.Play();
        yield return new WaitForSeconds(_logoAnim.clip.length);

        _logoAnim.gameObject.SetActive(false);
        _title.SetActive(true);

        var loadingOperation = SceneLoader.Instance.LoadSceneAsync(ESceneType.Lobby);
        if (loadingOperation == null)
        {
            yield break;
        }

        loadingOperation.allowSceneActivation = false;
        _progressText.text = "50%";
        yield return new WaitForSeconds(0.5f);

        while (true)
        {
            if (loadingOperation.isDone)
            {
                break;
            }

            _progressText.text = $"{(int)(loadingOperation.progress * 100.0f)}%";

            if (loadingOperation.progress >= 0.9f)
            {
                loadingOperation.allowSceneActivation = true;
            }

            yield return null;
        }
    }
}
