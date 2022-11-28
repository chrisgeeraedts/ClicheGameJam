using System;
using UnityEngine;
using TMPro;
using Assets.Scripts.Shared;
using Assets.Scripts.Map;
using System.Collections.Generic;

namespace Assets.Scripts.Underwater2
{
    public class NPC : MonoBehaviour
    {
        [SerializeField] TextMeshProUGUI npcChatText;
        [SerializeField] GameObject fishingpole;
        [SerializeField] ChatUnderwaterSO npcChatStart;
        [SerializeField] ChatUnderwaterSO playerHasThreeFishChat;
        [SerializeField] TextMeshProUGUI responseAText, responseBText, responseCText;
        [SerializeField] GameObject playerResponseParent;
        [SerializeField] GameObject playerResponseParentButton1;
        [SerializeField] GameObject playerResponseParentButton2;
        [SerializeField] GameObject playerResponseParentButton3;        
        [SerializeField] string npcMoveOnChatText;
        [SerializeField] List<GameObject> gameObjectsToActivateOnChat;
        [SerializeField] GameObject Tree;
        [SerializeField] GameObject Exit;



        private bool chatMode = false;
        private bool chatFinished = false;
        private ChatUnderwaterSO currentChat;
        private string chatKeyHintPrefix = "[<color=#E97419>^key^</color>]";

        public void HandleAnswerA()
        {
            ProgressChat(currentChat.FollowUpA);
        }

        public void HandleAnswerB()
        {
            ProgressChat(currentChat.FollowUpB);
        }

        public void HandleAnswerC()
        {
            ProgressChat(currentChat.FollowUpC);
        }

        private void Update()
        {
            HandlePlayerChatInput();
        }

        private void HandlePlayerChatInput()
        {
            if (!chatMode) return;

            if (Input.GetKeyDown(KeyCode.A))
            {
                if (!responseAText.enabled) return;
                HandleAnswerA();
            }
            else if (Input.GetKeyDown(KeyCode.S))
            {
                if (!responseBText.enabled) return;
                HandleAnswerB();
            }
            else if (Input.GetKeyDown(KeyCode.D))
            {
                if (!responseCText.enabled) return;
                HandleAnswerC();
            }
        }

        private void ProgressChat(ChatUnderwaterSO chat)
        {
            if (chat == null)
            {
                ExitChat();
                return;
            }

            if (chat.NpcChat.Equals(npcMoveOnChatText, StringComparison.InvariantCultureIgnoreCase))
            {
                var player = FindObjectOfType<PlayerScript>();
                MapManager.GetInstance().NumberOfFishInInventory = MapManager.GetInstance().NumberOfFishInInventory - 3;
                player.Options_CanJump = true; //Allow player to jump, workaround for not flat tilemap collider, but player has dealt with NPC so jumping is fine
                //TODO NICE: Move player to house and make disappear
                Debug.Log("NPC Happy, move to house plz");
                ExitChat();                
                FindObjectOfType<PlayerScript>().SetArrow(Exit);
                FindObjectOfType<PlayerScript>().Options_ShowTargetingArrow = false;
                return;
            }

            currentChat = chat;
            SetChatText(currentChat);
        }

        private void ExitChat()
        {
            FindObjectOfType<PlayerScript>().Options_ShowTargetingArrow = true;
            chatMode = false;
            chatFinished = true;
            var player = FindObjectOfType<PlayerScript>();
            player.SetPlayerActive(true);            
            FindObjectOfType<PlayerScript>().UnlockMovement();
            player.Options_CanChopTrees = true;
            playerResponseParent.SetActive(false);
            SetNPCChatObjectsActive(false);
            FindObjectOfType<PlayerScript>().SetArrow(Tree);
            //fishingpole.SetActive(true);
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (chatFinished) return;
            if (chatMode) return;
            if (collision.gameObject.tag != Constants.TagNames.Player) return;

            FindObjectOfType<PlayerScript>().SetPlayerActive(false);
            FindObjectOfType<PlayerScript>().StopMovement();
            FindObjectOfType<PlayerScript>().LockMovement();
            playerResponseParent.SetActive(true);
            chatMode = true;
            currentChat = npcChatStart;
            SetNPCChatObjectsActive(true);

            if (MapManager.GetInstance().NumberOfFishInInventory >= 3)
            {
                currentChat = playerHasThreeFishChat;
                GetComponent<CircleCollider2D>().enabled = false;
            }

            SetChatText(currentChat);
        }

        private void SetNPCChatObjectsActive(bool active)
        {
            foreach (var gameObject in gameObjectsToActivateOnChat)
            {
                gameObject.SetActive(active);
            }
        }

        private void SetChatText(ChatUnderwaterSO chat)
        {
            npcChatText.text = chat.NpcChat.Replace("^", Environment.NewLine);
            responseAText.text = $"{chatKeyHintPrefix.Replace("^key^", "A")}{chat.ResponseA}";
            responseBText.text = $"{chatKeyHintPrefix.Replace("^key^", "S")}{chat.ResponseB}";
            responseCText.text = $"{chatKeyHintPrefix.Replace("^key^", "D")}{chat.ResponseC}";

            responseAText.enabled = !string.IsNullOrEmpty(chat.ResponseA);
            playerResponseParentButton1.SetActive(!string.IsNullOrEmpty(chat.ResponseA));
            responseBText.enabled = !string.IsNullOrEmpty(chat.ResponseB);
            playerResponseParentButton2.SetActive(!string.IsNullOrEmpty(chat.ResponseB));
            responseCText.enabled = !string.IsNullOrEmpty(chat.ResponseC);
            playerResponseParentButton3.SetActive(!string.IsNullOrEmpty(chat.ResponseC));
        }

        void Start()
        {
            FindObjectOfType<PlayerScript>().Options_ShowTargetingArrow = false;
        }
    }
}
