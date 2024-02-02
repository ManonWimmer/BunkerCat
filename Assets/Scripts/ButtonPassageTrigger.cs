using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonPassageTrigger : MonoBehaviour
{
    // ----- VARIABLES ----- // 
    private bool isPlayerInRange; // Joueur dans le trigger ?

    private string currentMap = "default";

    public GameObject mapDefault;
    public GameObject grassDefault;

    public GameObject mapPassage;
    public GameObject grassPassage;

    private QuestsSO allQuests;
    private QuestSO questJulia;

    [SerializeField]
    public TextAsset textPassageCantOpen;

    private ShowVisualCues showVisualCues;

    // ----- VARIABLES ----- //


    private void Awake()
    {
        isPlayerInRange = false; // Le joueur ne se trouve pas dans le trigger au début du jeu
        showVisualCues = GetComponent<ShowVisualCues>();
        showVisualCues.DesactivateAllCues();


        // On remet la map normale au début de la partie au cas ou si elle été changée
        mapPassage.SetActive(false);
        grassPassage.SetActive(false);

        mapDefault.SetActive(true);
        grassDefault.SetActive(true);
    }

    private void Start()
    {
        allQuests = QuestsController.GetInstance().GetPlayerQuests();
        questJulia = allQuests.GetQuestAt(2);
    }

    private void Update()
    {
        if (isPlayerInRange && currentMap == "default") // Le joueur est dans le trigger
        {
            Debug.Log(questJulia.isCompleted);
            showVisualCues.device = InputManager.GetInstance().GetDevice();
            showVisualCues.ActivateCueForDevice();

            if (InputManager.GetInstance().GetInteractPressed())
            {
                // On check dans l'inventory si il y a la clé en fer 
                // Ou surement + simple -> que la questJulia a isCompleted à true
                if (questJulia.isCompleted)
                {
                    // On ouvre le passage
                    OpenPassage();
                    // TODO : enlever la clé de l'inventaire
                }
                else
                {
                    DialogueManager.GetInstance().EnterDialogueMode(textPassageCantOpen);
                }
            }
        }
        else // Le joueur n'est pas / plus dans le trigger
        {
            showVisualCues.DesactivateAllCues();
        }
    }

    public void OpenPassage()
    {
        if (currentMap  == "default")
        {
            // On change la map :
            mapPassage.SetActive(true);
            grassPassage.SetActive(true);

            mapDefault.SetActive(false);
            grassDefault.SetActive(false);
        }

        currentMap = "passage"; // On a plus les visual cues et on peut plus interagir avec l'ouverture du passage
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag("Player"))
        {
            isPlayerInRange = true; // Le joueur est dans le trigger
        }
    }

    private void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.CompareTag("Player"))
        {
            isPlayerInRange = false; // Le joueur n'est plus dans le trigger
        }
    }
}
