static public class Const
{
    static public string Event_Key_Num_Changed = "Event_Key_Num_Changed";
    static public string Event_Player_Data_Reload = "Event_Player_Data_Reload";
    static public readonly int TileRow = 11;
    static public readonly int GemAttrIncrease = 3;
    static public readonly int MedicineHpIncrease1 = 200;
    static public readonly int MedicineHpIncrease2 = 500;
}

public enum BirthPointType
{
    Upstairs, Downstairs,
}

public enum FaceType
{
    Up, Down, Left, Right,
}

public enum ItemType
{
    KeyYellow, KeyBlue, KeyRed, GemRed, GemBlue, MedicineHp1, MedicineHp2
}

public enum KeyType
{
    Yellow, Blue, Red,
}

static public class Utils
{
    static public int Pos2Index(UnityEngine.Vector2 pos)
    {
        return (int)pos.y * Const.TileRow + (int)pos.x;
    }
}