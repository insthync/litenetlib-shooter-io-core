﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathMatchNetworkGameRule : IONetworkGameRule
{
    public int endMatchCountDown = 10;
    public int EndMatchCountingDown { get; protected set; }

    public override bool HasOptionMatchTime { get { return true; } }

    public override bool HasOptionMatchKill { get { return true; } }

    protected override void EndMatch()
    {
        networkManager.StartCoroutine(EndMatchRoutine());
    }

    IEnumerator EndMatchRoutine()
    {
        EndMatchCountingDown = endMatchCountDown;
        while (EndMatchCountingDown > 0)
        {
            yield return new WaitForSeconds(1);
            --EndMatchCountingDown;
        }
        networkManager.StopHost();
    }

    public override bool RespawnCharacter(BaseNetworkGameCharacter character, params object[] extraParams)
    {
        var targetCharacter = character as CharacterEntity;
        var gameplayManager = GameplayManager.Singleton;
        // In death match mode will not reset score, kill, assist, death
        targetCharacter.Exp = 0;
        targetCharacter.level = 1;
        targetCharacter.statPoint = 0;
        targetCharacter.watchAdsCount = 0;
        targetCharacter.addStats = new CharacterStats();
        targetCharacter.Armor = 0;

        return true;
    }
}
