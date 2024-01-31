namespace ElectionSimulatorLibrary;

public enum RegionType
{
    ElectoralDistrict,
    CityWithCountyRights,
    County,
    Municipality,
    City,
    CityDistrict
}

public enum ElectionType
{
    Sejm,
    Senat
}

public enum MapMode
{
    Normal,
    Result
}

public enum ResponseCode
{
    Undefined,
    GeneratePopulation,
    Calculate,
    Done,   // z folderem do wyczyszczenia
    Result,
    Data
}

public enum ActionType
{
    A_DoNothing, //nic
    PL_Meeting, //tworzy event i nic
    PL_Live_Tv, //tworzy event i nic
    PL_Internet,//tworzy event i nic
    P_Meeting, //tworzy event i nic
    P_Live_Tv, //tworzy event i nic
    P_Internet, //tworzy event i nic
    V_Live_Tv,
    V_Internet
}

public enum EventType
{
    Single,
    Multi
}

public enum AgentType
{
    Agent,
    Voter,
    Politician,
    PoliticianLeader
}