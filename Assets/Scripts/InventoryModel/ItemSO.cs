using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Inventory.Model
{
    [CreateAssetMenu] // !!!
    public class ItemSO : ScriptableObject // !!!!!
    {
        // ----- VARIABLES ----- //
        [field: SerializeField]
        public bool IsStackable { get; set; } // Maj quand public au lieu de private | Bool est-ce qu'il peut se stacker ou item dans 1 slot de l'inventory

        public int ID => GetInstanceID(); // Récupération de l'ID de l'item

        [field: SerializeField]
        public int MaxStackSize { get; set; } = 1; // Nombre d'items par stack max

        [field: SerializeField]
        public string Name { get; set; } // Nom de l'item

        [field: SerializeField]
        [field: TextArea] // Texte de plusieurs lignes !!
        public string Description { get; set; } // Description de l'item

        [field: SerializeField]
        public Sprite ItemImage { get; set; } // Sprite de l'item
        // ----- VARIABLES ----- //


    }

}
