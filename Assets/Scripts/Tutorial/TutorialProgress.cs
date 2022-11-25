using UnityEngine;
using TMPro;
using System;
using Assets.Scripts.Shared;
using System.Collections;
using UnityEngine.SceneManagement;

namespace Assets.Scripts.Tutorial
{
    public class TutorialProgress : MonoBehaviour
    {
        public AudioSource bridgeAudio;
        [SerializeField] TextMeshProUGUI tutorialTextField;
        private PlayerScript player;

        public enum TutorialTriggerType
        {
            None,
            WASD,
            Jump,
            CoinPickup,
            VerticalJump,
            Lever,
            Vines,
            Gun,
            PlayerWantsGun,
            GainGun,
            ShootCrates,
            Teleport
        }

        private void Start()
        {
            player = FindObjectOfType<PlayerScript>();
        }

        public void TriggerTutorial(TutorialTriggerType trigger)
        {
            switch (trigger)
            {
                case TutorialTriggerType.WASD: TriggerWASD(); break;
                case TutorialTriggerType.Jump: TriggerJump(); break;
                case TutorialTriggerType.CoinPickup: TriggerCoinPickup(); break;
                case TutorialTriggerType.VerticalJump: TriggerVerticalJump(); break;
                case TutorialTriggerType.Lever: TriggerLever(); break;
                case TutorialTriggerType.Vines: TriggerVines(); break;
                case TutorialTriggerType.Gun: TriggerGun(); break;
                case TutorialTriggerType.PlayerWantsGun: TriggerPlayerWantsGun(); break;
                case TutorialTriggerType.GainGun: TriggerGainGun(); break;
                case TutorialTriggerType.ShootCrates: TriggerShootCrates(); break;
                case TutorialTriggerType.Teleport: TriggerTeleport(); break;
                default: Debug.LogWarning($"Tutorial for trigger {trigger} not defined."); break;
            }
        }

        private IEnumerator TeleportToMap()
        {
            yield return new WaitForSeconds(2);

            SceneManager.LoadScene(Constants.SceneNames.MapScene);
        }

        private void TriggerTeleport()
        {
            tutorialTextField.text = $"GOOD LUCK ! !";
            GlobalAchievementManager.GetInstance().TutorialCompleted();
            StartCoroutine(TeleportToMap());
        }

        private void TriggerShootCrates()
        {
            tutorialTextField.text = $"Here, have fun shooting these crates.{Environment.NewLine}After that it is time to enter the REAL world and hunt some cliches !.";
        }

        private void TriggerWASD()
        {
            tutorialTextField.text = $"Look at that, your first achievement!{Environment.NewLine}I bet you are amazing at finding all the cliches in this world.";
            GlobalAchievementManager.GetInstance().SetAchievementCompleted(23);
        }

        private void TriggerPlayerWantsGun()
        {
            player.Say("But I WANT a gun !");
        }

        private void TriggerGainGun()
        {
            tutorialTextField.text = $"Fine!, have a gun. Just be careful.{Environment.NewLine}Just like the sword you use [<color=#E97419>LMB</color>] to fire a bullet..";
            player.Options_CanAttackMelee = false;
            player.Options_CanFireGun = true;
            player.PlayerEquipment = PlayerEquipment.Gun;
        }

        private void TriggerGun()
        {
            tutorialTextField.text = $"You are also capable of wielding a gun.{Environment.NewLine}But I won't give you a gun just yet.";
        }

        private void TriggerVines()
        {
            tutorialTextField.text = $"These vines are blocking our way{Environment.NewLine}Use [<color=#E97419>LMB</color>] to hack at them with your sword.";
            player.Options_CanAttackMelee = true;
        }

        private void TriggerLever()
        {
            tutorialTextField.text = $"Oops, I forgot to tell you to use the lever..{Environment.NewLine}Use [<color=#E97419>E</color>] when standing next to it.";
        }

        private void TriggerVerticalJump()
        {
            tutorialTextField.text = $"Looks like we need some more of your finest jumping skills.";
        }

        private void TriggerCoinPickup()
        {
            tutorialTextField.text = $"Nice jump!{Environment.NewLine}And look ahead: we are rewarded with a coin we can pick up !";
        }

        private void TriggerJump()
        {
            tutorialTextField.text = $"Oh boy, how do we get up there?{Environment.NewLine}No worries, use [<color=#E97419>Space bar</color>] to jump!";
            player.Options_CanJump = true;
        }
    }
}
