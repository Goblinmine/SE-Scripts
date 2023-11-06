using EmptyKeys.UserInterface.Generated.ContractsBlockView_Gamepad_Bindings;
using Sandbox.Game;
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
        public class SimpleAirLock
        {
            public string Name { get; set; }
            public List<IMyDoor> Doors { get; set; }

            MyGridProgram gridProgram;
            TimeSpan closeTime;
            TimeSpan openOffset;

            TimeSpan? closeIn;
            TimeSpan? openIn;

            int stage = 0;
            IMyDoor doorA = null;
            IMyDoor doorB = null;

            public SimpleAirLock(IMyDoor door, MyGridProgram gridProgram, TimeSpan closeTime, TimeSpan openOffset)
            {
                Doors = new List<IMyDoor>() { door };
                this.gridProgram = gridProgram;
                this.closeTime = closeTime;
                this.openOffset = openOffset;
            }

            public void AddDoorB(IMyDoor door)
            {
                if (Doors.Count > 2)
                {
                    gridProgram.Echo("Error door count");
                    return;
                }

                Doors.Add(door);
            }


            public void Update()
            {
                AutoAirlock();
            }

            void AutoAirlock()
            {
                //lock / unlock doors
                AutoLockDoors();

                switch (stage)
                {
                    case 0:
                        //Check if door has opend
                        foreach (IMyDoor door in Doors)
                        {
                            if (door.Status == DoorStatus.Opening)
                            {
                                doorA = door;
                            }
                        }
                        if (doorA == null)
                            return;
                        doorB = GetOpposideDoor(doorA);
                        stage++;
                        break;

                    case 1:
                        //Auto Close DoorA
                        if (!AutoCloseDoor(doorA))
                            return;
                        stage++;
                        break;
                    case 2:
                        if (!AutoOpenDoor(doorB))
                            return;
                        stage++;
                        break;

                    case 3:
                        //Close doorB
                        if (!AutoCloseDoor(doorB))
                            return;
                        stage = 0;
                        doorA = null;
                        doorB = null;
                        break;
                }
            }

            bool AutoCloseDoor(IMyDoor door)
            {
                if (door.Status == DoorStatus.Open)
                {

                    //init closein time
                    if (closeIn == null)
                        closeIn = closeTime;
                    //count down

                    closeIn -= gridProgram.Runtime.TimeSinceLastRun;
                    if (closeIn <= TimeSpan.Zero)
                    {
                        door.CloseDoor();
                        closeIn = null;
                    }
                }
                else if (door.Status == DoorStatus.Closed)
                    return true;
                return false;
            }

            bool AutoOpenDoor(IMyDoor door)
            {
                if (door.Status == DoorStatus.Closed)
                {
                    //init closein time
                    if (openIn == null)
                        openIn = openOffset;
                    //count down

                    openIn -= gridProgram.Runtime.TimeSinceLastRun;
                    if (openIn <= TimeSpan.Zero)
                    {
                        door.OpenDoor();
                        openIn = null;
                        return true;
                    }
                }
                else if (door.Status == DoorStatus.Open)
                    return true;
                return false;
            }

            void AutoLockDoors()
            {
                bool bothDoorsClosed = true;
                foreach (var door in Doors)
                {
                    //Lock Doors
                    if (door.Status != DoorStatus.Closed)
                    {
                        var doorB = GetOpposideDoor(door);
                        if (doorB != null && doorB.Enabled)
                            doorB.Enabled = false;
                    }

                    //Check door status
                    if (door.Status != DoorStatus.Closed)
                        bothDoorsClosed = false;
                }

                //Unlock Doors
                if (bothDoorsClosed)
                {
                    foreach (var door in Doors)
                        door.Enabled = true;
                }
            }

            private IMyDoor GetOpposideDoor(IMyDoor doorA)
            {
                foreach (var door in Doors)
                {
                    if (door != doorA)
                        return door;
                }
                return null;
            }

            public override string ToString()
            {
                string output = string.Empty;
                foreach (var door in Doors)
                {
                    output += door.CustomName;
                }
                return output;
            }
        }
    }
}
