using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public PlayerController player;
    public DoorController doorExit;

    public bool gameOver;

    public List<EnemyController> enemyList = new List<EnemyController>();

    

    public void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        player = FindObjectOfType<PlayerController>();
        doorExit = FindObjectOfType<DoorController>();
    }

    public void Update()
    {
        gameOver = player.isDead;
        UIManager.instance.GameOverUI(gameOver);
    }

    public void IsEnemy(EnemyController enemy)
    {
        enemyList.Add(enemy);
    }

    public void EnemyDead(EnemyController enemy)
    {
        enemyList.Remove(enemy);
        if (enemyList.Count == 0)
            doorExit.OpenDoor();
    }

    public void RestartScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

}
