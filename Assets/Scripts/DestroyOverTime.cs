using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyOverTime : MonoBehaviour
{
    // ----- VARIABLES ----- //
    public float lifeTime; // Temps � attendre avant de d�truire un certain objet
    // ----- VARIABLES ----- //

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Destroy(gameObject, lifeTime); // Detruire un certain objet apr�s un certain temps
    }
}
