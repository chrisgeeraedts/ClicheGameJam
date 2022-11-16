using UnityEngine;

namespace Assets.Scripts.Underwater2
{
    [CreateAssetMenu(fileName = "New NPC Chat", menuName = "NPC Chat")]
    public class ChatUnderwaterSO : ScriptableObject
    {
        [SerializeField] string npcChat;
        [SerializeField] string responseA, responseB, responseC;
        [SerializeField] ChatUnderwaterSO followUpA, followUpB, followUpC;

        public string NpcChat => npcChat;
        public string ResponseA => responseA;
        public string ResponseB => responseB;
        public string ResponseC => responseC;
        public ChatUnderwaterSO FollowUpA => followUpA;
        public ChatUnderwaterSO FollowUpB => followUpB;
        public ChatUnderwaterSO FollowUpC => followUpC;
    }
}
