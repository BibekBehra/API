using System.Runtime.Serialization;

namespace AN360API.APIEnum
{
    public enum ResponseTypes
    {
        [EnumMember]
        NotUsed = 0,
        [EnumMember]
        EntryExit1 = 1,
        [EnumMember]
        EntryExit2 = 2,
        [EnumMember]
        Perimeter = 3,
        [EnumMember]
        InteriorFollower = 4,
        [EnumMember]
        DayNight = 5,
        [EnumMember]
        TwentyFourHourSilent = 6,
        [EnumMember]
        TwentyFourHourAudible = 7,
        [EnumMember]
        TwentyFourHourAuxiliary = 8,
        [EnumMember]
        FireNoVerification = 9,
        [EnumMember]
        InteriorWithDelay = 10,
        [EnumMember]
        Monitor = 12,
        [EnumMember]
        CarbonMonoxide = 14,
        [EnumMember]
        FireWithVerification = 16,
        [EnumMember]
        Trouble = 19,
        [EnumMember]
        ArmStay = 20,
        [EnumMember]
        ArmAway = 21,
        [EnumMember]
        Disarm = 22,
        [EnumMember]
        NoResponse = 23,
        [EnumMember]
        SilentBurglary = 24,
        [EnumMember]
        Garage = 50,
        [EnumMember]
        GarageMonitor = 53,
        [EnumMember]
        ResidentMonitor = 85,
        [EnumMember]
        ResidentResponse = 86,
        [EnumMember]
        GeneralMonitor = 87,
        [EnumMember]
        GeneralResponse = 88
    }
}