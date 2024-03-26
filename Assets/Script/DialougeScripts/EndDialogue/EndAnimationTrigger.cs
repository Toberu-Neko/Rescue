using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(EnemyInRange))]
public class EndAnimationTrigger : MonoBehaviour
{
    [Header("¹ï¸Ü")]
    [SerializeField] private DialogueScriptableObj[] endDialogue;
    [SerializeField] private CanvasGroup[] diaCanvasGroup;
    [SerializeField] private CanvasGroup lastBlackImage;
    [SerializeField] private GameObject dialogueTextGameobj;
    [SerializeField] private GameObject dialogueObj;
    [SerializeField] private GameObject endCanvas;
    private MainMenuDialogueManager dialogueManager;
    int dialogueCount;
    bool blackImagAlphaChanging;

    [Header("UI")]
    [SerializeField] private GameObject saveAndLoadUI;
    private PauseMenu pauseMenu;
    //player
    [SerializeField] private LocalizedString pressEString;
    [SerializeField] private LocalizedString clearEnemyString;
    [SerializeField] private LocalizedString endString;
    private PlayerStates playerStates;
    PlayerData playerData;
    private GameObject notePressE;
    private TextMeshProUGUI notePressEText;

    private EnemyInRange enemyInRange;

    public KeyCode interactionKey = KeyCode.E;


    bool playerInRange;
    void Start()
    {
        playerData = PlayerManager.instance.playerData;
        playerStates = PlayerManager.instance.player.GetComponent<PlayerStates>();
        if (playerData.gameEnded)
        {
            enabled = false;
        }
        pauseMenu = UIManager.instance.UI.transform.Find("PauseMenuCanvas").gameObject.GetComponent<PauseMenu>();
        notePressE = PlayerManager.instance.player.transform.Find("WorldSpaceCanvas/PressEToTalkNote").gameObject;
        notePressEText = notePressE.transform.Find("PressEToTalk").gameObject.GetComponent<TextMeshProUGUI>();
        enemyInRange = GetComponent<EnemyInRange>();
        dialogueManager = GetComponent<MainMenuDialogueManager>();
        playerInRange = false;
    }

    void Update()
    {
        if (dialogueManager.dialogueEnded && dialogueCount < endDialogue.Length && diaCanvasGroup[0].alpha != 0)
        {
            dialogueTextGameobj.SetActive(false);
            dialogueManager.dialogueEnded = false;


            StopAllCoroutines();
            StartCoroutine(CanvasGroupAlpha(dialogueCount));
        }

        if (dialogueCount == endDialogue.Length  && dialogueManager.dialogueEnded)
        {
            if (lastBlackImage.alpha == 0)
            {
                dialogueTextGameobj.SetActive(false);
            }
            if (lastBlackImage.alpha < 1 && !blackImagAlphaChanging)
            {
                blackImagAlphaChanging = true;
                StopAllCoroutines();
                StartCoroutine(BlackImageAlpha());
            }
            if (lastBlackImage.alpha == 1)
            {
                //dialogueObj.SetActive(false);
                //Debug.Log("Ended");
                playerStates.nowGoal = endString.GetLocalizedString();
                saveAndLoadUI.SetActive(true);
                Time.timeScale = 0f;
            }
        }
        if (Input.GetKeyDown(interactionKey) && playerInRange && Cursor.lockState == CursorLockMode.Locked && !enemyInRange.enemyInRange && !playerStates.gameEnded)
        {
            pauseMenu.canPause = false;
            playerStates.gameEnded = true;
            endCanvas.SetActive(true);
            AudioManager.instance.Play("BGM_FinalAni");
            AudioManager.instance.Stop("BGM_L1");
            AudioManager.instance.Stop("BGM_Rain");
            StopAllCoroutines();
            StartCoroutine(CanvasGroupAlpha(0));
        }
        if (enemyInRange.enemyInRange && playerInRange && notePressEText.text != clearEnemyString.GetLocalizedString())
        {
            notePressEText.text = clearEnemyString.GetLocalizedString();
        }
        if (!enemyInRange.enemyInRange && playerInRange && notePressEText.text != pressEString.GetLocalizedString())
        {
            notePressEText.text = pressEString.GetLocalizedString();
        }
    }
    IEnumerator CanvasGroupAlpha(int i)
    {
        while (diaCanvasGroup[i].alpha < 1)
        {
            diaCanvasGroup[i].alpha += 0.1f;
            yield return new WaitForSeconds(0.05f);
        }
        //Debug.Log(i);
        yield return null;
        dialogueTextGameobj.SetActive(true);
        dialogueCount++;
        dialogueManager.StartDialogue(endDialogue[i]);
    }
    IEnumerator BlackImageAlpha()
    {
        while (lastBlackImage.alpha < 1)
        {
            lastBlackImage.alpha += .1f;
            yield return new WaitForSeconds(0.05f);
        }
        blackImagAlphaChanging = false;
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.CompareTag("Player"))
        {
            playerInRange = true;
            notePressE.SetActive(true);
        }
    }
    private void OnTriggerExit(Collider collider)
    {
        if (collider.gameObject.CompareTag("Player"))
        {
            playerInRange = false;
            notePressE.SetActive(false);
        }
    }
}
