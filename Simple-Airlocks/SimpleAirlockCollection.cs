using Sandbox.Game.EntityComponents;
using Sandbox.ModAPI.Ingame;
using Sandbox.ModAPI.Interfaces;
using SpaceEngineers.Game.ModAPI.Ingame;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using VRage;
using VRage.Collections;
using VRage.Game;
using VRage.Game.Components;
using VRage.Game.GUI.TextPanel;
using VRage.Game.ModAPI.Ingame;
using VRage.Game.ModAPI.Ingame.Utilities;
using VRage.Game.ObjectBuilders.Definitions;
using VRageMath;

namespace IngameScript
{
    partial class Program
    {
        public class SimpleAirlockCollection : Dictionary<string, SimpleAirLock>
        {
            TimeSpan closeTime;
            TimeSpan openOffset;

            public SimpleAirlockCollection(TimeSpan closeTime, TimeSpan openOffset)
            {
                this.closeTime = closeTime;
                this.openOffset = openOffset;
            }

            public void InitNewAirlock(IMyDoor door, string key, MyGridProgram gridProgram)
            {
                if (this.ContainsKey(key))
                {
                    this[key].AddDoorB(door);
                }
                else
                {
                    this.Add(key, new SimpleAirLock(door, gridProgram, closeTime, openOffset));
                }
            }

            public override string ToString()
            {
                string output = string.Empty;
                foreach (var airlock in this)
                {
                    output += $"KEY: {airlock.Key}, AIRLOCK: {airlock.Value}\n";
                }

                return output;
            }

            public void Update()
            {
                foreach (var airlock in this.Values)
                {
                    airlock.Update();
                }
            }
        }
    }
}
