using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIMenuScript : MonoBehaviour
{
	public bool isMainTitle = true;
	public GameObject thisMenu, optionsMenu, creditsMenu;
	public GameObject menuFirst, optionsFirst, optionsExitTo, creditsFirst, creditsExitTo;
	
    void Start() {
		if(isMainTitle) {
			EventSystem.current.SetSelectedGameObject(null);
			EventSystem.current.SetSelectedGameObject(menuFirst);
		}
    }

    void Update() {
        if(!isMainTitle && (Input.GetKeyDown(KeyCode.Pause) || Input.GetKeyDown(KeyCode.P) || Input.GetKeyDown(KeyCode.JoystickButton7))) {
			PauseUnpause();
		}
    }
	
	public void PauseUnpause() {
		if(!thisMenu.activeInHierarchy) {
			thisMenu.SetActive(true);
			Time.timeScale = 0f;
			
			EventSystem.current.SetSelectedGameObject(null);
			EventSystem.current.SetSelectedGameObject(menuFirst);
		}
		else {
			thisMenu.SetActive(false);
			if(optionsMenu != null) optionsMenu.SetActive(false);
			if(creditsMenu != null) creditsMenu.SetActive(false);
			Time.timeScale = 1f;
		}
	}
	
	public void OpenOptions() {
		optionsMenu.SetActive(true);
		EventSystem.current.SetSelectedGameObject(null);
		EventSystem.current.SetSelectedGameObject(optionsFirst);
	}
	public void CloseOptions() {
		optionsMenu.SetActive(false);
		EventSystem.current.SetSelectedGameObject(null);
		EventSystem.current.SetSelectedGameObject(optionsExitTo);
	}
	
	public void OpenCredits() {
		creditsMenu.SetActive(true);
		EventSystem.current.SetSelectedGameObject(null);
		EventSystem.current.SetSelectedGameObject(creditsFirst);
	}
	public void CloseCredits() {
		creditsMenu.SetActive(false);
		EventSystem.current.SetSelectedGameObject(null);
		EventSystem.current.SetSelectedGameObject(creditsExitTo);
	}
}
