using DungeonDelvers.Content.DungeonGeneration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DungeonDelvers.Content
{
    public enum FloorType
    {
        dungeon = 0,
    }
    public class Floor
    {
        public int floorID;
        public FloorType floorType;
        public List<Room> rooms;

        public Floor(int floorID, FloorType floorType)
        {
            this.floorID = floorID;
            this.floorType = floorType;
            GenerateRooms();
        }
        private void GenerateRooms()
        {
            int maxTotalRooms = Main.rand.Next(9, 15);
            int treasureRoomCount = Main.rand.Next(1, 3);
            int normalRoomCount = maxTotalRooms - treasureRoomCount - 2; // 2 for boss and entrance rooms

        }
    }
}
