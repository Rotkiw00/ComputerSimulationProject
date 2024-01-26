using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectionSimulatorLibrary;

public class DemographySettings
{
    public Dictionary<RegionType, int> PlaceWeight { get; } = new Dictionary<RegionType, int>();
    public List<int> PopulationPerDistrict { get; } = new List<int>();

    public DemographySettings()
    {
        ResetPlaceWeight();

        for (int i = 0; i < 100; i++)
        {
            PopulationPerDistrict.Add(10000);
        }

        PopulationPerDistrict[0] = 208227;
        PopulationPerDistrict[1] = 209160;
        PopulationPerDistrict[2] = 299013;
        PopulationPerDistrict[3] = 233820;
        PopulationPerDistrict[4] = 243037;
        PopulationPerDistrict[5] = 497725;
        PopulationPerDistrict[6] = 232906;
        PopulationPerDistrict[7] = 275652;
        PopulationPerDistrict[8] = 447269;
        PopulationPerDistrict[9] = 298288;
        PopulationPerDistrict[10] = 265993;
        PopulationPerDistrict[11] = 243614;
        PopulationPerDistrict[12] = 269829;
        PopulationPerDistrict[13] = 320897;
        PopulationPerDistrict[14] = 325101;
        PopulationPerDistrict[15] = 257526;
        PopulationPerDistrict[16] = 196250;
        PopulationPerDistrict[17] = 182417;
        PopulationPerDistrict[18] = 314444;
        PopulationPerDistrict[19] = 242707;
        PopulationPerDistrict[20] = 281461;
        PopulationPerDistrict[21] = 217749;
        PopulationPerDistrict[22] = 282036;
        PopulationPerDistrict[23] = 299531;
        PopulationPerDistrict[24] = 204319;
        PopulationPerDistrict[25] = 258302;
        PopulationPerDistrict[26] = 269254;
        PopulationPerDistrict[27] = 293861;
        PopulationPerDistrict[28] = 246208;
        PopulationPerDistrict[29] = 492566;
        PopulationPerDistrict[30] = 343878;
        PopulationPerDistrict[31] = 289658;
        PopulationPerDistrict[32] = 324285;
        PopulationPerDistrict[33] = 289248;
        PopulationPerDistrict[34] = 279329;
        PopulationPerDistrict[35] = 303068;
        PopulationPerDistrict[36] = 306672;
        PopulationPerDistrict[37] = 370536;
        PopulationPerDistrict[38] = 256808;
        PopulationPerDistrict[39] = 445920;
        PopulationPerDistrict[40] = 451037;
        PopulationPerDistrict[41] = 317867;
        PopulationPerDistrict[42] = 369053;
        PopulationPerDistrict[43] = 959492;
        PopulationPerDistrict[44] = 347311;
        PopulationPerDistrict[45] = 293317;
        PopulationPerDistrict[46] = 258876;
        PopulationPerDistrict[47] = 184145;
        PopulationPerDistrict[48] = 185155;
        PopulationPerDistrict[49] = 349664;
        PopulationPerDistrict[50] = 289990;
        PopulationPerDistrict[51] = 192118;
        PopulationPerDistrict[52] = 255081;
        PopulationPerDistrict[53] = 254682;
        PopulationPerDistrict[54] = 355776;
        PopulationPerDistrict[55] = 345163;
        PopulationPerDistrict[56] = 258135;
        PopulationPerDistrict[57] = 404599;
        PopulationPerDistrict[58] = 345676;
        PopulationPerDistrict[59] = 383410;
        PopulationPerDistrict[60] = 152008;
        PopulationPerDistrict[61] = 345170;
        PopulationPerDistrict[62] = 329249;
        PopulationPerDistrict[63] = 251519;
        PopulationPerDistrict[64] = 397552;
        PopulationPerDistrict[65] = 266252;
        PopulationPerDistrict[66] = 163347;
        PopulationPerDistrict[67] = 284914;
        PopulationPerDistrict[68] = 160015;
        PopulationPerDistrict[69] = 322459;
        PopulationPerDistrict[70] = 227650;
        PopulationPerDistrict[71] = 302823;
        PopulationPerDistrict[72] = 231021;
        PopulationPerDistrict[73] = 295278;
        PopulationPerDistrict[74] = 189531;
        PopulationPerDistrict[75] = 291875;
        PopulationPerDistrict[76] = 214244;
        PopulationPerDistrict[77] = 334341;
        PopulationPerDistrict[78] = 252862;
        PopulationPerDistrict[79] = 218075;
        PopulationPerDistrict[80] = 333881;
        PopulationPerDistrict[81] = 302955;
        PopulationPerDistrict[82] = 309217;
        PopulationPerDistrict[83] = 229922;
        PopulationPerDistrict[84] = 225319;
        PopulationPerDistrict[85] = 301819;
        PopulationPerDistrict[86] = 287505;
        PopulationPerDistrict[87] = 300427;
        PopulationPerDistrict[88] = 280864;
        PopulationPerDistrict[89] = 314289;
        PopulationPerDistrict[90] = 408994;
        PopulationPerDistrict[91] = 305030;
        PopulationPerDistrict[92] = 281345;
        PopulationPerDistrict[93] = 253398;
        PopulationPerDistrict[94] = 263808;
        PopulationPerDistrict[95] = 237443;
        PopulationPerDistrict[96] = 347119;
        PopulationPerDistrict[97] = 424690;
        PopulationPerDistrict[98] = 248444;
        PopulationPerDistrict[99] = 226830;
    }

    public void ResetPlaceWeight()
    {
        PlaceWeight.Clear();

        PlaceWeight.Add(RegionType.City, 20);
        PlaceWeight.Add(RegionType.CityWithCountyRights, 55);
        PlaceWeight.Add(RegionType.Municipality, 10);
        PlaceWeight.Add(RegionType.CityDistrict, 15);
    }

    public List<RegionType> GetInhabitedRegions()
    {
        return PlaceWeight.Keys.ToList();
    }
}
