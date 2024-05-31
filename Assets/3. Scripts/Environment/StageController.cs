﻿using System;
using System.Collections.Generic;
using _3._Scripts.Singleton;
using GBGamesPlugin;
using UnityEngine;

namespace _3._Scripts.Environment
{
    public class StageController : Singleton<StageController>
    {
        [SerializeField] private List<Stage> stages = new();
        
        public Stage CurrentStage { get; private set; }
        private void Awake()
        {
            CurrentStage = Instantiate(stages[GBGames.saves.stageID], transform);
            CurrentStage.transform.localPosition = Vector3.zero;
        }
    }
}