using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    #region Singleton
    public static PlayerManager instance;

    private void Awake()
    {
        instance = this;
    }

    #endregion

    public GameObject player;
    //public GameObject pressENote;
    public GameObject mainCamera;
    //public GameObject playerHealthBar;
    
    public PlayerData playerData;
    //public GameObject pauseMenu;
}
