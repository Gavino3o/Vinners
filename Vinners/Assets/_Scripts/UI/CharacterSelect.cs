using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using FishNet;

/*
 * Contains all functionalities related to the character select lobby. 
 */
public class CharacterSelect : View
{
    
    [SerializeField] private TextMeshProUGUI lockInButtonText;
    [SerializeField] private Button nextCharacterButton;
    [SerializeField] private Button prevCharacterButton;
    [SerializeField] private Button lockInButton;

    [SerializeField] private Button startGameButton;

    [SerializeField] private Image characterDisplayPanel;
    [SerializeField] private Button leaveButton;
    /* figure out how to get the character sprites from this later
     * [SerializeField] List<Character> characterList;
     * OR
     * we can have character splash arts
     * [SerializeField] List<Image> characterSprites;
     */

    //temp
    [SerializeField] private List<GameObject> characterList;

    // just to monitor in inspector
    [SerializeField] private int currCharacterIndex;

    public override void Initialise()
    {
        nextCharacterButton.onClick.AddListener(() =>
        {
            currCharacterIndex = (currCharacterIndex + 1) % characterList.Count;
            Player.LocalInstance.ChooseCharacter(characterList[currCharacterIndex]);
            // characterDisplayPanel = characterList[currCharacterIndex].characterSprite
        });

        prevCharacterButton.onClick.AddListener(() =>
        {
            // math here might be wrong
            currCharacterIndex = (currCharacterIndex + characterList.Count - 1) % characterList.Count;
            Player.LocalInstance.ChooseCharacter(characterList[currCharacterIndex]);
            // characterDisplayPanel = characterList[currCharacterIndex].characterSprite
        });

        /*
         * Assigns the currently hovered character to the player and locks in for the player.
         */
        lockInButton.onClick.AddListener(() => 
        {
            Player.LocalInstance.ServerSetLockIn(!Player.LocalInstance.isLockedIn);
                     
        });

        /*
         * Only the Host should have access to the start button
         */
        if (InstanceFinder.IsHost)
        {
            startGameButton.onClick.AddListener(() => {
                GameManager.Instance.StartGame();
            });

            leaveButton.onClick.AddListener(() =>
            {
                InstanceFinder.ServerManager.StopConnection(true);
                InstanceFinder.ClientManager.StopConnection();
                // Destroy(InstanceFinder.NetworkManager);
            });

            startGameButton.gameObject.SetActive(true);
        } else
        {
            startGameButton.gameObject.SetActive(false);

            leaveButton.onClick.AddListener(() => {
                InstanceFinder.ClientManager.StopConnection();
                // Destroy(InstanceFinder.NetworkManager);
            });
            
        }

        base.Initialise();
        currCharacterIndex = 0;

    }

    private void Update()
    {
        if (!Initialised) return;

        lockInButtonText.color = Player.LocalInstance.isLockedIn ? Color.green : Color.red;

        // Should only be able to start game if all players in the lobby are ready.
        startGameButton.interactable = GameManager.Instance.canStart;
    }





}
