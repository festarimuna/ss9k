// Public enums for Manager classes

namespace Utils
{
    public enum ButtonType
    {
        Tile = 0,
        Menu = 1,
    } // set which clickable GOs are tiles and menus

    public enum CanvasType
    {
        Tile = 0,
        Menu = 1,
    } // set which canvases are tiles and menus

    public enum TerrainType
    {
        Grass = 0,
        Sand = 1,
        Dirt = 2,
        Water = 3,
    } // set tile terrain types

    public enum ListNames
    {
        tileListAdjacentTiles = 0,
        tileListFrontier = 1,
        tileListNeighbours = 2,
    } // names for lists of tiles
}