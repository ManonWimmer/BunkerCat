using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Inventory.Model
{
    [CreateAssetMenu]
    public class EdibleItemSO : ItemSO, IDestroyableItem, IItemAction
    {
        // ----- VARIABLES ----- //
        [SerializeField]
        private List<ModifierData> modifiersData = new List<ModifierData>(); 

        public string ActionName => "Boire"; // Consume = manger l'objet (health, bonus de force, vitesse temp ?)

        // SFX :
        [field: SerializeField]
        public AudioClip actionSFX { get; private set; }

        //dossier item Modifiers
        public bool PerformAction(GameObject character)
        {
            foreach (ModifierData data in modifiersData)
            {
                data.statModifier.AffectCharacter(character, data.value);
            }

            return true;
        }
        // ----- VARIABLES ----- //

    }

    public interface IDestroyableItem
    {

    }

    public interface IItemAction
    {
        // ----- VARIABLES ----- //
        public string ActionName { get; }
        public AudioClip actionSFX { get; }
        bool PerformAction(GameObject character);
        // ----- VARIABLES ----- //
    }

    [Serializable]
    public class ModifierData
    {
        public CharacterStatModifierSO statModifier;
        public float value;
    }
}

