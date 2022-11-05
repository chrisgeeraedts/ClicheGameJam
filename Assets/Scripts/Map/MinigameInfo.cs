using UnityEngine;

namespace Assets.Scripts.Map
{
    [CreateAssetMenu(fileName = "New MinigameInfo", menuName = "Minigame Info")]
    public class MinigameInfo : ScriptableObject
    {
        [SerializeField] string minigameName;
        [SerializeField] string sceneName;
        [SerializeField] Sprite mapSprite;

        public string MinigameName { get { return minigameName; } }
        public string SceneName { get { return sceneName; } }
        public Sprite MapSprite { get { return mapSprite; } }
    }
}
