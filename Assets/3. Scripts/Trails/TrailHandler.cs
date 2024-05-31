using System.Linq;
using _3._Scripts.Config;
using _3._Scripts.Player;
using UnityEngine;

namespace _3._Scripts.Trails
{
    public class TrailHandler
    {
        private readonly PlayerMovement _movement;
        private readonly TrailRenderer _trailRenderer;
        public TrailHandler(PlayerMovement movement, TrailRenderer trailRenderer)
        {
            _movement = movement;
            _trailRenderer = trailRenderer;
        }

        public void SetTrail(string id)
        {
            var data = Configuration.Instance.AllTrails.FirstOrDefault(t => t.ID == id);
            if(data == null) return;
            
            _trailRenderer.startColor = data.Color;
            _trailRenderer.endColor = data.Color;
            _movement.SpeedMultiplier = data.Boost;
        }
    }
}