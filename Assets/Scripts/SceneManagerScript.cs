using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManagerScript : MonoBehaviour
{
    public byte titleGameScene = 0;
	public byte startGameScene = 1;
	
	public void ToTitleScene() {
        SceneManager.LoadScene(titleGameScene);
    }
    public void ToGameScene() {
        SceneManager.LoadScene(startGameScene);
    } 
	public void ExitGame() {
		Application.Quit();
	}
}
