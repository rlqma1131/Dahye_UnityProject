using UnityEngine;
using UnityEngine.SceneManagement;

namespace Main
{
    public class InteractionHandler : MonoBehaviour
    {
        public enum Type { NPC_Angel, NPC_Wizzard, DungeonEntrance }
        public Type type;

        public GameObject questionMark;
        private bool playerInRange = false;

        void Start()
        {
            if (questionMark != null)
                questionMark.SetActive(false);
        }

        void Update()
        {
            if (playerInRange && Input.GetKeyDown(KeyCode.Space))
            {
                TriggerInteraction();
            }
        }

        void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                playerInRange = true;
                if (questionMark != null)
                    questionMark.SetActive(true);
            }
        }

        void OnTriggerExit2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                playerInRange = false;
                if (questionMark != null)
                    questionMark.SetActive(false);
            }
        }

        void TriggerInteraction()
        {
            switch (type)
            {
                case Type.NPC_Angel:
                    LoadMiniGameScene("FlappyPlane");
                    break;
                case Type.NPC_Wizzard:
                    LoadMiniGameScene("TheStack");
                    break;
                case Type.DungeonEntrance:
                    LoadMiniGameScene("Dungeon");
                    break;
            }
        }

        void LoadMiniGameScene(string sceneName)
        {
            SceneManager.LoadScene(sceneName, LoadSceneMode.Single);
        }

        public void ReturnToMainScene(string miniGameSceneName)
        {
            SceneManager.UnloadSceneAsync(miniGameSceneName);
        }
    }
}
