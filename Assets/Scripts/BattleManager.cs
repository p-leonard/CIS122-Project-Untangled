using System;
using UnityEngine;
using HealthComponents;
using System.Linq;

public class BattleManager : MonoBehaviour
{
    [Header("Player")]
    public GameObject player;
    private MonoBehaviour playerControllerScript;
    private HealthSystem playerHealth;

    [Header("Boss")]
    public GameObject boss;
    private BossAnimationController bossAnimator;
    private HealthSystem bossHealth;

    // Events
    public event Action OnBattleStart;
    public event Action OnPlayerWin;
    public event Action OnPlayerLose;

    private bool battleEnded = false;

    void Start()
    {
        playerHealth = player.GetComponent<HealthSystem>();
        playerControllerScript = player.GetComponent<PlayerController>();

        bossHealth = boss.GetComponent<HealthSystem>();
        bossAnimator = boss.GetComponent<BossAnimationController>();

        object[] components = { playerHealth, playerControllerScript, bossHealth, bossAnimator, boss, player};

        if (components.Any(x => x == null)) Debug.LogError("NULL REFERENCE!");

        playerHealth.OnDeath += HandlePlayerDeath;
        bossHealth.OnDeath += HandleBossDeath;

        StartBattle();
    }

    private void StartBattle()
    {
        Debug.Log("Battle Started!");
        OnBattleStart?.Invoke();
    }

    private void HandlePlayerDeath()
    {
        if (battleEnded) return;

        Debug.Log("Player Lost!");
        battleEnded = true;

        OnPlayerLose?.Invoke();

#warning The Battle Manager shouldn't tell the animators what to do, let them subscribe to the event and handle it themselves!
        //bossAnimator?.SetAnimationState(BossAnimationController.AnimationState.Idle);
#warning Same with the player controls!
        //DisablePlayerControls();
    }

    private void HandleBossDeath()
    {
        if (battleEnded) return;

        Debug.Log("Player Won!");
        battleEnded = true;

        OnPlayerWin?.Invoke();

#warning The Battle Manager shouldn't tell the animators what to do, let them subscribe to the event and handle it themselves!
        bossAnimator?.SetAnimationState(BossAnimationController.AnimationState.Idle);
    }


#warning I think that we should reload the scene if we want to reset the battle. As it stands, we really don't need a reset function.
// If you think otherwise, though, I'd love to hear it.

    //    public void ResetBattle()
    //    {
    //        Debug.Log("Resetting battle...");
    //        battleEnded = false;


#warning Because Unity wants to handle the disposal of gameObjects, we need to avoid using null coalessing operators or we'll cause some WEIRD errors.
    //        //They're fine for regular C# events though, as Unity doesn't really control them. That's why OnPlayerWin?.Invoke() is OK!

    //        //bossAnimator?.SetAnimationState(BossAnimationController.AnimationState.ResetAnimation);
    //        //playerHealth?.SetHealth(playerHealth.MaxHealth);
    //        //bossHealth?.SetHealth(bossHealth.MaxHealth);
    //    }
}
