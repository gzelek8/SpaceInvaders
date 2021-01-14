
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public bool inGame;
    public GameObject resetButton;
    public GameObject lvlScreen;
    public GameObject winScreen;
    public Text scoreText;
    private float score;
    public GameObject enemiesParent;
    public List<GameObject> enemies;
    public GameObject player;
    public bool isImmortal;
    public bool isTrippleAttack;
    public float trippleAttackTime;
    private void Start()
    {
        InitizialeGame();
    }

    private void InitizialeGame()
    {
        instance = this;
        inGame = true;
        if (LevelManager.level < 3)
            CreateEnemiesWave();
        else if (LevelManager.level == 3)
            CreateBoss();
    }

    private void CreateBoss()
    {
        //enemies[2] is boss
        Instantiate(enemies[2], new Vector2(Vector2.zero.x, Vector2.zero.y + 2), Quaternion.identity);
    }

    private void CreateEnemiesWave()
    {
        int row = LevelManager.level + 3;
        int column = LevelManager.level + 6;
        for (int i = 0; i < row; i++)
        {
            for (int j = 0; j < column; j++)
            {
                GameObject enemy = enemies[UnityEngine.Random.Range(0, 2)];
                Instantiate(enemy, new Vector2(enemiesParent.transform.position.x + j, enemiesParent.transform.position.y - i), Quaternion.identity, enemiesParent.transform);
            }
        }
        
    }

    public void ScoreCollected(int value = 10)
    {
        score += value;
        scoreText.text = score.ToString();
    }

    public void ActiveShield()
    {
        player.GetComponentInChildren<SpriteRenderer>().color = Color.yellow;
        isImmortal = true;
    }

    public void StopShield()
    {
        player.GetComponentInChildren<SpriteRenderer>().color = Color.white;
        isImmortal = false;
    }


    public void ActiveTripleShot()
    {
        if (isTrippleAttack)
        {
            CancelInvoke("StopTrippleShot");
        }

        isTrippleAttack = true;
        Invoke("StopTrippleShot", trippleAttackTime);
    }

    public void StopTrippleShot()
    {
        isTrippleAttack = false;
    }

    public void GameOver()
    {
        inGame = false;
        resetButton.SetActive(true);
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(1);
    }

    public void GoToNextLevel()
    {
        SceneManager.LoadScene(1);
    }

    public void WinLevel()
    {
        if (GameObject.FindGameObjectWithTag("Enemy1") == null && GameObject.FindGameObjectWithTag("Enemy2") == null &&  GameObject.FindGameObjectWithTag("Boss") == null)
        {
            LevelManager.level++;
            inGame = false;
            if(LevelManager.level < 3)
            {
                lvlScreen.GetComponentInChildren<Button>().GetComponentInChildren<Text>().text = $"Go to level {LevelManager.level + 1}";
                lvlScreen.SetActive(true);
            }
            else if (LevelManager.level == 3)
            {
                lvlScreen.GetComponentInChildren<Button>().GetComponentInChildren<Text>().text = $"Go defeat BOSS";
                lvlScreen.SetActive(true);
            }
                
            else if(LevelManager.level > 3)
                winScreen.SetActive(true);
        }

    }

    public void QuitButton()
    {
        Application.Quit();
    }

}
