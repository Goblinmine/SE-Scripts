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
        string tag = "SP-Zone";
        MyIni globalSettings = new MyIni();

        public Program()
        {
            Runtime.UpdateFrequency = UpdateFrequency.Update10;

            //Read Settings of PB
            globalSettings.TryParse(Me.CustomData);
            //Set some default settings if not yet set
            if(!globalSettings.ContainsSection(tag))
            {
                globalSettings.Set(tag, "tag", "SP-Zone");
                Me.CustomData = globalSettings.ToString();
            }
        }

        public void Main(string argument, UpdateType updateSource)
        {
            //was soll das scipt machen
            //lock outer doors if inner presure is higher than outer

            //have button to depreserice room (and unlock doors)

            if (argument.Trim() != string.Empty && argument != null)
                HandleUserInput(argument);

        }

        private void HandleUserInput(string userCommand)
        {
            //command: <press|unpress|open|close|forceopen> <name of zone> 
            var command = userCommand.Split(' ');

            if (command.Length != 2)
            {
                Echo($"Unknown command: {command}.");
                return;
            }

            switch (command[0])
            {
                case "open":
                    Echo("Command \"open\" getting executed");
                    break;
                case "forceopen":
                    Echo("Command \"forceopen\" getting executed");
                    break;
                case "close":
                    Echo("Command \"close\" getting executed");
                    break;
                default:
                    Echo($"Command \"{command[0]}\" not valid!");
                    return;
            }
        }

        private void Init()
        {
            List<IMyDoor> doorList = new List<IMyDoor>();
            GridTerminalSystem.GetBlocksOfType(doorList, (x) => x.CustomName.Contains(tag));
        }

        //public SettingsCollection ReadSettings(IMyTerminalBlock block, string tag)
        //{

        //    //read settings of a block
        //    SettingsCollection settings = new SettingsCollection(tag);

        //    bool settingsStarted = false;
        //    foreach (string row in block.CustomData.Split('\n'))
        //    {
        //        string rowTrimed = row.Trim();

        //        if (row == "/n")
        //            continue;

        //        if (settingsStarted)
        //        {
        //            Echo(rowTrimed);
        //            var setting = rowTrimed.Split('=');
        //            settings.Add(setting[0], setting[1]);
        //        }
        //        else if (rowTrimed == tag)
        //            settingsStarted = true;
        //        else if (rowTrimed.Contains('['))
        //            settingsStarted = false;
        //    }

        //    if (settings.Count == 0)
        //        //settings.InitDefault();
        //        InitDefaultSettings();

        //    return settings;
        //}

        //public void SaveSettings(IMyTerminalBlock block, SettingsCollection settings)
        //{
        //    //2 cases
        //    //1. settings allready here (need to override)
        //    //2. setting not yet there



        //    //find start and end 
        //    int start = 0, end = 0;

        //    var lines = block.CustomData.Split('\n').ToList();

        //    for (int i = 0; i < lines.Count; i++)
        //    {
        //        string rowTrimed = lines[i].Trim();
        //        if (rowTrimed == settings.Tag)
        //            start = i;
        //        else if (rowTrimed.Contains('['))
        //            end = i - 1;
        //    }

        //    var mySettings = settings.ToLines();
        //    StringBuilder s = new StringBuilder();

        //    if (start == 0 && end == 0)
        //    {
        //        //add to the bottom

        //        foreach (var setting in mySettings)
        //        {
        //            s.AppendLine(setting);
        //        }
        //        block.CustomData = s.ToString();
        //    }
        //    else
        //    {
        //        //override

        //        for (int i = start; i < end; i++)
        //        {
        //            lines[i] = mySettings[i - start];
        //        }

        //        foreach (var line in lines)
        //        {
        //            s.AppendLine(line);
        //        }

        //        block.CustomData = s.ToString();
        //    }
        //}
    }
}
