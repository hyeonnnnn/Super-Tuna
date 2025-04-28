using UnityEngine;
using System;
using System.Collections;
using System.Security.Cryptography.X509Certificates;

public class Growth : MonoBehaviour
{
    private readonly int[] expTable;
    private readonly int MaxExp;
    private readonly int MaxLevel;

    public int CurrentExp { get; private set; } = 0;
    public int CurrentLevel { get; private set; } = 1;
    public bool doingEvolution = false;

    [SerializeField] private HungerSystem hungerSystem;
    [SerializeField] private PlayerMove playerMove;
    [SerializeField] private GameObject[] characterPrefabs;
    private int characterPrefabsInx = 0;
    private Vector3 baseScale = Vector3.one;

    public static event Action<int, int> OnExpGaugeChanged;
    public static event Action<int> OnLevelChanged;

    [SerializeField] private GameObject LevelUpEffect;

    public Growth()
    {
        expTable = new int[] { 0, 300, 1000 };
        MaxExp = expTable[expTable.Length - 1];
        MaxLevel = expTable.Length;
    }

    private void Start()
    {
        OnExpGaugeChanged?.Invoke(CurrentExp, MaxExp);
        OnLevelChanged?.Invoke(CurrentLevel);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.J))
        {
            AddExp(40);
        }
        else if (Input.GetKeyDown(KeyCode.K))
        {
            AddExp(80);
        }
    }

    public void AddExp(int expAmount)
    {
        if (CurrentExp >= expTable[MaxLevel - 1])
        {
            return;
        }
        else
        {
            CurrentExp = Math.Min(MaxExp, CurrentExp + expAmount);
            OnExpGaugeChanged?.Invoke(CurrentExp, MaxExp);
            CheckLevelUp();
        }
    }

    private void CheckLevelUp()
    {
        if(CurrentExp >= expTable[CurrentLevel])
        {
            StartCoroutine(CheckHuntAnimation());
        }
    }

    private IEnumerator CheckHuntAnimation()
    {
        Animator animator = characterPrefabs[characterPrefabsInx].GetComponent<Animator>();

        if (animator != null)
        {
            while (!animator.GetCurrentAnimatorStateInfo(0).IsName("Hunting"))
            {
                yield return null;
            }

            while (animator.GetCurrentAnimatorStateInfo(0).IsName("Hunting") && animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1f)
            {
                yield return null;
            }
        }

        if(!doingEvolution)
        {
            doingEvolution = true;
            StartCoroutine(ApplyLevelUp());
        }
    }

    private IEnumerator ApplyLevelUp()
    {
        LevelUpEffect.SetActive(true);

        yield return new WaitForSeconds(2f);

        LevelUpEffect.SetActive(false);

        if(!playerMove.GetPlayerIsDead())
        {
            CurrentLevel += 1;
            OnLevelChanged?.Invoke(CurrentLevel);

            characterPrefabs[characterPrefabsInx].SetActive(false);
            characterPrefabsInx++;
            characterPrefabs[characterPrefabsInx].SetActive(true);

            ChangePrefabAnimator(characterPrefabs[characterPrefabsInx]);

            IncreaseScale();
            doingEvolution = false;
        }
    }

    private void IncreaseScale()
    {
        float scaleMultiplier = 1 + (CurrentLevel * 0.8f);
        transform.localScale = baseScale * scaleMultiplier;
    }

    private void ChangePrefabAnimator(GameObject Prefab)
    {
        hungerSystem.ChangeAnimator(Prefab.GetComponent<Animator>());
    }
}
