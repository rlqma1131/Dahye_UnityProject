using UnityEngine;
using UnityEngine.SceneManagement;

namespace Main
{
    public class MainSceneBGMChanger : MonoBehaviour
    {
        private void Start()
        {
            SoundManager.instance.ChangeBackGroundMusic(SoundManager.instance.mainSceneBGM);
        }
    }
}