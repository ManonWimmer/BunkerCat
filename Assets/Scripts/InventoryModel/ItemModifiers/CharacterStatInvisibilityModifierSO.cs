using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class CharacterStatInvisibilityModifierSO : CharacterStatModifierSO
{
    private float timer = 0f;
    private bool isTimerRunning = false;
    private GameObject player;

    private void Update()
    {
        Debug.Log(isTimerRunning);
        if (isTimerRunning)
        {
            timer += Time.deltaTime;

            if (timer >= 3f)
            {
                int layerVisible = LayerMask.NameToLayer("Player");
                player.layer = layerVisible;

                player.GetComponent<SpriteRenderer>().color = new Color(1f, 1, 1f, 1f); // On remet l'opacité
                ResetTimer();
            }
        }
    }
    public override void AffectCharacter(GameObject character, float val)
    {
        player = character;
        StartTimer();
    }

    private void StartTimer()
    {
        Debug.Log("start timer");
        timer = 0f;

        int layerHidden = LayerMask.NameToLayer("PlayerHidden");
        player.layer = layerHidden;

        player.GetComponent<SpriteRenderer>().color = new Color(1f, 1, 1f, 0.5f); // On baisse l'opacité

        isTimerRunning = true;
    }

    private void ResetTimer()
    {
        Debug.Log("reset timer");
        timer = 0f;
        isTimerRunning = false;
    }
}
