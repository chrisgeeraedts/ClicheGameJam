using UnityEngine;

namespace Assets.Scripts.Map
{
    [CreateAssetMenu(fileName = "New MinigameInfo", menuName = "Minigame Info")]
    public class MinigameInfo : ScriptableObject
    {
        [SerializeField] string minigameName;
        [SerializeField] string sceneName;
        [SerializeField] Sprite mapSprite;
        private bool isWon;
        private bool isFinished;
        private bool wasLastFinished;

        public string MinigameName => minigameName;
        public string SceneName => sceneName;
        public Sprite MapSprite => mapSprite;
        public bool IsWon => isWon;
        public bool IsFinished => isFinished;

        public void FinishGame(bool isWon)
        {
            isFinished = true;
            this.isWon = isWon;
        }
    }
}
