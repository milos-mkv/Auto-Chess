using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MeinMenuBehaviour : MonoBehaviour
{
    [SerializeField] private Button playButton;
    [SerializeField] private Button exitButton;

    private void Awake()
    {
        if (NetworkManager.Singleton != null) {
            Destroy(NetworkManager.Singleton.gameObject);
        }

        playButton.onClick.AddListener(() => {
            SceneManager.LoadScene(GameScenes.CreateGameScene.ToString());
        });

        exitButton.onClick.AddListener(() => { 
            Application.Quit(); 
        });
    }
}
