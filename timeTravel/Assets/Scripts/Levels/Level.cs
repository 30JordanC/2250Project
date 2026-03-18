using UnityEngine;
using System.Collections.Generic;

public abstract class Level : MonoBehaviour
{
    public Timer levelTimer;

    public List<GameObject> environmentObjects;
    public List<GameObject> npcList;
    public List<GameObject> enemyList;
    public List<GameObject> puzzleList;

    public abstract void Initialize();
    public abstract void SpawnNPCs();
    public abstract void SpawnEnemies();
    public abstract void StartLevel();
    public abstract void EndLevel();
}