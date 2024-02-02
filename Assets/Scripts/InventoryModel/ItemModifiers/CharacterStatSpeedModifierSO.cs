using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class CharacterStatSpeedModifierSO : CharacterStatModifierSO
{
    private float timer = 0f;
    private bool isTimerRunning = false;
    private float initialSpeed;
    private PlayerController playerController;

    private void Update()
    {
        if (isTimerRunning)
        {
            timer += Time.deltaTime;

            if (timer >= 3f)
            {
                playerController.movementSpeed = initialSpeed;
                ResetTimer();
            }
        }
    }
    public override void AffectCharacter(GameObject character, float val)
    {
        playerController = character.GetComponent<PlayerController>();
        StartTimer();

    }

    private void StartTimer()
    {
        timer = 0f;
        isTimerRunning = true;
        initialSpeed = playerController.movementSpeed;
        playerController.movementSpeed = initialSpeed * 1.5f;
    }

    private void ResetTimer()
    {
        timer = 0f;
        isTimerRunning = false;
    }
}
