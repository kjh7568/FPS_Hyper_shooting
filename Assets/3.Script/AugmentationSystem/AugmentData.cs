public class AugmentData
{
    public int id;
    public AugmentType type;
    public float value;
    public string description;

    public AugmentData(int id, AugmentType type, float value, string description)
    {
        this.id = id;
        this.type = type;
        this.value = value;
        this.description = description;
    }
}
public enum AugmentType
{
    MoveSpeedUp,
    DashCooltimeDown,
    MaxHealthUp
}