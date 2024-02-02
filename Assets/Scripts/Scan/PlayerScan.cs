using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerScan : MonoBehaviour
{
    // ----- VARIABLES ----- //
    public static Collider2D[] overlappedColliders;
    private int layerMask;

    [SerializeField]
    private float scanRange = 10f;

    [SerializeField]
    private float scanCooldown = 3f;

    [SerializeField]
    private bool inCooldown = false;
    // ----- VARIABLES ----- //

    void Update()
    {
        if (InputManager.GetInstance().GetScanPressed()) // Si touche de scan
        {
            ScanWorld();
        }
    }

    private void Awake()
    {
        layerMask = LayerMask.GetMask("Player");
    }

    private void ScanWorld()
    {
        if (!inCooldown)
        {
            overlappedColliders = Physics2D.OverlapCircleAll(transform.position, scanRange, layerMask); // raycast circle autour du joueur
            if (overlappedColliders.Length > 0)
            {
                foreach (var collider in overlappedColliders)
                {
                    //Debug.Log(collider.tag + collider.gameObject.name);
                    if (collider.CompareTag("NPCs"))
                    {
                        Color initialColor = collider.gameObject.GetComponent<SpriteRenderer>().color;
                        GameObject light = collider.transform.Find("LightScan").gameObject;
                        collider.gameObject.GetComponent<SpriteRenderer>().color = new Color(0.4f, 0.8f, 0.3f, 1f); // Vert
                        light.SetActive(true);
                        StartCoroutine(ColorAndLightBackToNormal(collider.gameObject, initialColor, light));
                    }

                    if (collider.CompareTag("Items"))
                    {
                        GameObject light = collider.transform.Find("LightScan").gameObject;
                        light.SetActive(true);
                        StartCoroutine(LightBackToNormal(light));
                    }

                    if (collider.CompareTag("Enemies"))
                    {
                        // Immobilisation des ennemis pendant 2sec : 
                        collider.gameObject.GetComponent<Enemy>().canMove = false;
                        StartCoroutine(EnemyMovementBack(collider.gameObject));
                    }
                }
            }

            inCooldown = true;
        }
        else // En cooldown
        {
            StartCoroutine(WaitCooldown()); // On attend pour le désactiver
        }
;
    }

    IEnumerator ColorAndLightBackToNormal(GameObject gameObject, Color color, GameObject light)
    {
        yield return new WaitForSeconds(2f);
        gameObject.GetComponent<SpriteRenderer>().color = color;
        light.SetActive(false);
    }

    IEnumerator LightBackToNormal(GameObject light)
    {
        yield return new WaitForSeconds(2f);
        light.SetActive(false);
    }

    IEnumerator EnemyMovementBack(GameObject gameObject)
    {
        yield return new WaitForSeconds(2f);
        gameObject.GetComponent<Enemy>().canMove = true;
    }

    IEnumerator WaitCooldown()
    {
        Debug.Log("début scan cooldown");
        yield return new WaitForSeconds(scanCooldown);
        Debug.Log("fin scan cooldown");
        inCooldown = false;
    }



    private void OnDrawGizmos()
    {
       Gizmos.DrawWireSphere(transform.position, scanRange); // Affichage temp du cercle de scan autour du joueur   
    }
}


