using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManagerScript : MonoBehaviour
{
    public byte titleGameScene = 0;
	public byte startGameScene = 1;
	public byte endGameScene = 2;
	
	public void ToTitleScene() {
        SceneManager.LoadScene(titleGameScene);
    }
    public void ToGameScene() {
        SceneManager.LoadScene(startGameScene);
    }
	public void ToEndScene() {
        SceneManager.LoadScene(endGameScene);
    }
	public void ToScene(int number) {
        SceneManager.LoadScene(number);
    } 
	public void ExitGame() {
		Application.Quit();
	}
}
