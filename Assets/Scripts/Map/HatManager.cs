using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Map
{
    class HatManager : MonoBehaviour
    {
        [SerializeField] Sprite avatarWithSillyHatImage;
        [SerializeField] Sprite avatarWithCrown;
        [SerializeField] Image avatarRenderer;

        private void Start()
        {
            ApplyHat();
        }

        private void ApplyHat()
        {
            if (MapManager.GetInstance().IsWearingSillyHat)
            {
                avatarRenderer.sprite = avatarWithSillyHatImage;
            }
            else
            {
                avatarRenderer.sprite = avatarWithCrown;
            }
        }

        public void SwapHats()
        {
            if (MapManager.GetInstance() != null || !MapManager.GetInstance().HasSillyHat) return;
            MapManager.GetInstance().IsWearingSillyHat = !MapManager.GetInstance().IsWearingSillyHat;
            ApplyHat();
        }
    }
}
