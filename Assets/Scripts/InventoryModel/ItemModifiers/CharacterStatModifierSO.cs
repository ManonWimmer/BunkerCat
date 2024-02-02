using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CharacterStatModifierSO : ScriptableObject // Abstract pour indiquer que la classe doit uniquement servir de classe de base pour d'autre classes
{
    // ----- VARIABLES ----- //
    public abstract void AffectCharacter(GameObject character, float val);
}
