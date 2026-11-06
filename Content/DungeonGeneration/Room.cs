using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DungeonDelvers.Content.DungeonGeneration
{
    public enum RoomType
    {
        normal = 0,
        treasure = 1,
        boss = 2,
    }
    public class Room
    {
        public int roomID;
        public HashSet<Point> roomTiles = new HashSet<Point>();
        public int width;
        public int height;
        public Vector2 roomTilePosition;
        public Vector2 roomScreenPosition { get; private set; }
        public RoomType roomType;
        public Rectangle roomRect { get; }

        public Room(int roomID, int width, int height, Vector2 roomTilePosition, HashSet<Point> roomTiles, RoomType roomType)
        {
            this.roomID = roomID;
            this.roomType = roomType;
            this.width = width;
            this.height = height;
            this.roomTilePosition = roomTilePosition;
            this.roomTiles = roomTiles;

            roomRect = new Rectangle((int)roomTilePosition.X, (int)roomTilePosition.Y, width, height);
        }
        public void Update()
        {
            roomScreenPosition = roomTilePosition - Main.screenPosition;
        }
    }
}
