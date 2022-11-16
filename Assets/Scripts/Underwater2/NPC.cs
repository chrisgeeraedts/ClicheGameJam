using System;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Assets.Scripts.Shared;

namespace Assets.Scripts.Underwater2
{
    public class NPC : MonoBehaviour
    {
        [SerializeField] TextMeshProUGUI npcChatText;
        [SerializeField] GameObject fishingpole;
        [SerializeField] ChatUnderwaterSO npcChatStart;
        [SerializeField] TextMeshProUGUI responseAText, responseBText, responseCText;
        [SerializeField] GameObject playerResponseParent;

        private bool chatMode = false;
        private bool chatFinished = false;
        private ChatUnderwaterSO currentChat;

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
                HandleAnswerA();
            }
            else if (Input.GetKeyDown(KeyCode.S))
            {
                HandleAnswerB();
            }
            else if (Input.GetKeyDown(KeyCode.D))
            {
                HandleAnswerC();
            }
        }

        private void ProgressChat(ChatUnderwaterSO chat)
        {
            if (chat == null)
            {
                chatMode = false;
                chatFinished = true;
                FindObjectOfType<Player>().SetActive(true);
                playerResponseParent.SetActive(false);
                return;
            }

            //TODO NICE: Show player response in chatbubble?
            currentChat = chat;
            SetChatText(currentChat);
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (chatFinished) return;
            if (chatMode) return;
            if (collision.gameObject.tag != Constants.TagNames.Player) return;

            FindObjectOfType<Player>().SetActive(false);
            playerResponseParent.SetActive(true);
            chatMode = true;
            currentChat = npcChatStart;
            SetChatText(currentChat);
        }

        private void SetChatText(ChatUnderwaterSO chat)
        {
            npcChatText.text = chat.NpcChat.Replace("^", Environment.NewLine);
            responseAText.text = chat.ResponseA;
            responseBText.text = chat.ResponseB;
            responseCText.text = chat.ResponseC;
        }
    }
}
