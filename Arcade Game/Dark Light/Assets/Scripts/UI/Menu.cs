﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;
using UnityEngine.UI;

/*---------------------------------/
 * Script by Aiden Nathan.
 *---------------------------------*/

namespace MainMenu
{
    public class Menu : MonoBehaviour
    {
        #region Variables
        //General: 
        public GameObject main, mainBackground, fade, options, general, video, audio, controls, overlay; //Allows for reference to GameObjects Meny and Options
        //public bool toggle = false; //Toggle for switching between settings and main
        //public int option = 0; //Changes between the 4 main screens in options.
        public bool quitTimer = false; //Check whether or not the exit button has been pressed
        public int qTimer = 0; //Timer for transition - exit
        public bool startTimer = false; //Checks whether or not the play button has been pressed
        public int sTimer = 0; //Timer for transition - load game

        //Settings:
        public AudioMixer masterMixer; //Creates reference for the menu music
        Resolution[] resolutions; //Creates reference   for all resolutions within Unity
        public Dropdown resolutionDropdown; //Creates reference for the resolution dropdown 

        //Controls:
        private Dictionary<string, KeyCode> keybind = new Dictionary<string, KeyCode>();
        public Text up, down, left, right, jump, attack, dash;
        private GameObject currentKey;
        #endregion

        #region General
        public void Start() //Used to load resolutions and create list for the dropdown, collects both Width and Height seperately
        {
            resolutions = Screen.resolutions;
            resolutionDropdown.ClearOptions();

            int currentResolutionIndex = 0;
            List<string> options = new List<string>();
            for (int i = 0; i < resolutions.Length; i++) //Load possible resolutions into list
            {
                string option = resolutions[i].width + " x " + resolutions[i].height;
                options.Add(option);

                if (resolutions[i].width == Screen.currentResolution.width && resolutions[i].height == Screen.currentResolution.height) //Makes sure the resolution is correctly applied
                {
                    currentResolutionIndex = i;
                }
            }

            resolutionDropdown.AddOptions(options);
            resolutionDropdown.value = currentResolutionIndex;
            resolutionDropdown.RefreshShownValue();

            keybind.Add("Up", KeyCode.W);
            keybind.Add("Down", KeyCode.S);
            keybind.Add("Left", KeyCode.A);
            keybind.Add("Right", KeyCode.D);
            keybind.Add("Jump", KeyCode.Space);
            keybind.Add("Attack", KeyCode.E);
            keybind.Add("Dash", KeyCode.LeftShift);

            //up.text = keybind["Up"].ToString();
            //down.text = keybind["Down"].ToString();
            //left.text = keybind["Left"].ToString();
            //right.text = keybind["Right"].ToString();
            //jump.text = keybind["Jump"].ToString();
            //attack.text = keybind["Attack"].ToString();
            //dash.text = keybind["Dash"].ToString();
        }

        public void Update()
        {
            //Debug.Log("Load: " + loadTimer + " - Exit: " + exitTimer);
            //if (Input.GetKey(KeyCode.M) == true) //Failsafe
            //{
            //    toggle = false;
            //}
            if (quitTimer == true) //Exit Transition
            {
                qTimer++;
                if (qTimer >= 120)
                {
                    qTimer = 0;
                    Application.Quit();
                    //UnityEditor.EditorApplication.isPlaying = false;
                    quitTimer = false;
                }
            }
            if (startTimer == true) //Play Transition
            {
                sTimer++;
                if (sTimer >= 120)
                {
                    sTimer = 0;
                    main.SetActive(false);
                    mainBackground.SetActive(false);
                    overlay.SetActive(true);
                    startTimer = false;
                    fade.GetComponent<FadeController>().FadeIn();
                }
            }
        }
        #endregion

        #region Main
        public void StartGame() //Trigger for Play Button
        {
            startTimer = true;
            fade.GetComponent<FadeController>().FadeOut();
        }

        public void Quit() //Trigger for Exit Button
        {
            quitTimer = true;

        }

        public void Options(bool toggle) //Trigger for Settings - sets active layer/pannel
        {
            if (toggle == true)
            {
                main.SetActive(false);
                options.SetActive(true);
            }
            else if (toggle == false)
            {
                main.SetActive(true);
                options.SetActive(false);
            }
        }
        #endregion

        #region Settings
        public void ChangeBetween(int option) //Trigger for Settings - sets active layer/pannel
        {
            if (option == 0)
            {
                general.SetActive(true);
                video.SetActive(false);
                audio.SetActive(false);
                controls.SetActive(false);
            }
            else if (option == 1)
            {
                general.SetActive(false);
                video.SetActive(true);
                audio.SetActive(false);
                controls.SetActive(false);
            }
            else if (option == 2)
            {
                general.SetActive(false);
                video.SetActive(false);
                audio.SetActive(true);
                controls.SetActive(false);
            }
            else if (option == 3)
            {
                general.SetActive(false);
                video.SetActive(false);
                audio.SetActive(false);
                controls.SetActive(true);
            }
        }

        public void MasterVolume(float volume) //Trigger for changing volume of game's master channel
        {
            masterMixer.SetFloat("Master", volume);
        }

        public void EffectsVolume(float volume) //Trigger for changing volume of game's sfx channel
        {
            masterMixer.SetFloat("Effects", volume);
        }

        public void MusicVolume(float volume) //Trigger for changing volume of game's music channel
        {
            masterMixer.SetFloat("Music", volume);
        }

        public void AmbienceVolume(float volume) //Trigger for changing volume of game's music channel
        {
            masterMixer.SetFloat("Ambience", volume);
        }

        public void ChangeQuality(int qualityIndex) //Trigger for applying level of quality - detailing of objects
        {
            QualitySettings.SetQualityLevel(qualityIndex);
        }

        public void ToggleFullscreen(bool isFullscreen) //Trigger for applying fullscreen
        {
            Screen.fullScreen = isFullscreen;
        }

        public void ChangeResolution(int resIndex) //Trigger for changing and applying resolution based on list
        {
            Resolution resolution = resolutions[resIndex];
            Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
        }

        public void OnGUI()
        {
            if (currentKey != null) //Checks whether or not there is a Keycode saved to 'currentKey'
            {
                Event keypress = Event.current; //Creates an event called keypress
                if (keypress.isKey) //Checks whether or not the event "keypress" contains a keycode
                {
                    keybind[currentKey.name] = keypress.keyCode; //Saves the keycode from the event as the keycode attached to the keybind dictionary
                    currentKey.transform.GetChild(0).GetComponent<Text>().text = keypress.keyCode.ToString(); //Changes the text to match that of the keycode replacing the previous one
                    currentKey = null; //resets the currentKey putting it back to null
                }
            }
        }

        public void changeControls(GameObject clicked) //Trigger for changing any one of the keybinds
        {
            currentKey = clicked;

        }
        #endregion
    }
}

