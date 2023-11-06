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
    partial class Program : MyGridProgram
    {
        string tag = "S-AL";
        string doortag = "S-AL";
        MyIni globalSettings = new MyIni();
        TimeSpan closeTime = TimeSpan.FromMilliseconds(500);
        TimeSpan openOffset = TimeSpan.FromMilliseconds(100);
        SimpleAirlockCollection airLocks;
        List<IMyDoor> doors = new List<IMyDoor>();

        public Program()
        {
            Runtime.UpdateFrequency = UpdateFrequency.Update10;
            
            InitSettings();
            InitAirlocks();
        }



        public void Main(string argument, UpdateType updateSource)
        {
            airLocks.Update();

            switch (argument.ToLower().Trim())
            {
                case "refresh":
                    InitSettings();
                    InitAirlocks();
                    break;
            }
        }
        public void InitAirlocks()
        {
            airLocks = new SimpleAirlockCollection(closeTime, openOffset);
            doors.Clear();
            GridTerminalSystem.GetBlocksOfType(doors, x => x.CustomName.Contains($"[{doortag}]"));

            foreach (var door in doors)
            {
                var key = door.CustomName.Substring(door.CustomName.IndexOf(tag) + tag.Length + 1);
                airLocks.InitNewAirlock(door, key, this);
            }

            //Echo
            Echo($"Airlocks managed: {airLocks.Count}");
        }
        public void InitSettings()
        {
            //Read Settings of PB
            globalSettings.TryParse(Me.CustomData);
            //Set some default settings if not yet set
            if (!globalSettings.ContainsSection(tag))
            {
                globalSettings.Set(tag, "tag", tag);
                globalSettings.Set(tag, "auto close time", closeTime.TotalSeconds);
                globalSettings.Set(tag, "auto open offsest time", openOffset.TotalSeconds);
                Me.CustomData = globalSettings.ToString();
            }
            else
            {
                //read settings
                doortag = globalSettings.Get(tag, "tag").ToString();
                closeTime = TimeSpan.FromSeconds(globalSettings.Get(tag, "auto close time").ToDouble());
                openOffset = TimeSpan.FromSeconds(globalSettings.Get(tag, "auto open offsest time").ToDouble());
            }
        }
    }
}
