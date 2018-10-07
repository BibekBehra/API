using System.Runtime.Serialization;

namespace AN360API.APIEnum
{
    public enum DeviceType
    {
        [EnumMember]
        Police = 854,
        [EnumMember]
        CarbonMonoxideDetector = 856,
        [EnumMember]
        Door = 858,
        [EnumMember]
        Environmental = 859,
        [EnumMember]
        Fire = 860,
        [EnumMember]
        Flood = 861,
        [EnumMember]
        GlassBreak = 862,
        [EnumMember]
        HeatSensor = 864,
        [EnumMember]
        Medical = 866,
        [EnumMember]
        MotionSensor = 867,
        [EnumMember]
        NotUsed = 868,
        [EnumMember]
        SmokeDetector = 876,
        [EnumMember]
        Temperature = 877,
        [EnumMember]
        Window = 878,
        [EnumMember]
        GarageDoor = 879,
        [EnumMember]
        WirelessSiren = 880,
        [EnumMember]
        Other = 869
    }
}