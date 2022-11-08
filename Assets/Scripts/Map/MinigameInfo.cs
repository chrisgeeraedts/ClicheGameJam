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
        private bool isLost;

        public string MinigameName => minigameName;
        public string SceneName => sceneName;
        public Sprite MapSprite => mapSprite;
        public bool IsWon => isWon;
        public bool IsLost => isLost;
    }
}
