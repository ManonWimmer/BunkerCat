using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyOverTime : MonoBehaviour
{
    // ----- VARIABLES ----- //
    public float lifeTime; // Temps à attendre avant de détruire un certain objet
    // ----- VARIABLES ----- //

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Destroy(gameObject, lifeTime); // Detruire un certain objet après un certain temps
    }
}
