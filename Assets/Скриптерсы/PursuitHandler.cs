using System.Collections.Generic;
using UnityEngine;
using Скриптерсы.Enemy;

public class PursuitHandler : MonoBehaviour
{
    private readonly HashSet<EnemyBase> enemyBases = new HashSet<EnemyBase>();

    public void StartPursuit(EnemyBase enemyBase)
    {
        if (enemyBase == null) return;

        enemyBases.Add(enemyBase);
        CheckPursuit();
    }

    public void StopPursuit(EnemyBase enemyBase)
    {
        if (enemyBase == null) return;

        enemyBases.Remove(enemyBase);
        CheckPursuit();
    }

    private void CheckPursuit()
    {
        float fightValue = enemyBases.Count > 0 ? 1f : 0f;
        
        Debug.Log(fightValue);
        FMODUnity.RuntimeManager.StudioSystem.setParameterByName("Fight", fightValue);
    }

    public void PlayMusic()
    {
        FMODUnity.RuntimeManager.StudioSystem.setParameterByName("Fight", 1);
    }
}