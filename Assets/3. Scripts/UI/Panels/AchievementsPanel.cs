using System.Collections.Generic;
using _3._Scripts.Config;
using _3._Scripts.UI.Elements;
using _3._Scripts.UI.Panels.Base;
using UnityEngine;

namespace _3._Scripts.UI.Panels
{
    public class AchievementsPanel : SimplePanel
    {
        [SerializeField] private AchievementTable table;
        [SerializeField] private Transform container;

        private readonly List<AchievementTable> _tables = new();
        public override void Initialize()
        {
            base.Initialize();
            foreach (var data in Configuration.Instance.AchievementData)
            {
                var obj = Instantiate(table, container);
                obj.Initialize(data);
                _tables.Add(obj);
            }
        }

        protected override void OnOpen()
        {
            base.OnOpen();
            foreach (var t in _tables)
            {
                t.UpdateView();
            }
        }
    }
}