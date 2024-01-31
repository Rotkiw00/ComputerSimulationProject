using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectionSimulatorLibrary;

public class DemographySettings
{
    public Dictionary<RegionType, int> PlaceWeight { get; } = new Dictionary<RegionType, int>();
    public List<(int, Attributes)> PopulationPerDistrict { get; } = new List<(int, Attributes)>();
    private double _turnout = 0.7438;
    public double Turnout
    { 
        get => _turnout;
        set
        {
            if (value < 0) _turnout = 0.01;
            else if (value > 1) _turnout = 1;
            else _turnout = value;

        } 
    }

    public DemographySettings()
    {
        ResetPlaceWeight();

        var PopulationList = new List<int>();
        
        #region DefaultPopulation
        for (int i = 0; i < 100; i++)
        {
            PopulationList.Add(10000);
        }
        
        PopulationList[0] = 208227;
        PopulationList[1] = 209160;
        PopulationList[2] = 299013;
        PopulationList[3] = 233820;
        PopulationList[4] = 243037;
        PopulationList[5] = 497725;
        PopulationList[6] = 232906;
        PopulationList[7] = 275652;
        PopulationList[8] = 447269;
        PopulationList[9] = 298288;
        PopulationList[10] = 265993;
        PopulationList[11] = 243614;
        PopulationList[12] = 269829;
        PopulationList[13] = 320897;
        PopulationList[14] = 325101;
        PopulationList[15] = 257526;
        PopulationList[16] = 196250;
        PopulationList[17] = 182417;
        PopulationList[18] = 314444;
        PopulationList[19] = 242707;
        PopulationList[20] = 281461;
        PopulationList[21] = 217749;
        PopulationList[22] = 282036;
        PopulationList[23] = 299531;
        PopulationList[24] = 204319;
        PopulationList[25] = 258302;
        PopulationList[26] = 269254;
        PopulationList[27] = 293861;
        PopulationList[28] = 246208;
        PopulationList[29] = 492566;
        PopulationList[30] = 343878;
        PopulationList[31] = 289658;
        PopulationList[32] = 324285;
        PopulationList[33] = 289248;
        PopulationList[34] = 279329;
        PopulationList[35] = 303068;
        PopulationList[36] = 306672;
        PopulationList[37] = 370536;
        PopulationList[38] = 256808;
        PopulationList[39] = 445920;
        PopulationList[40] = 451037;
        PopulationList[41] = 317867;
        PopulationList[42] = 369053;
        PopulationList[43] = 959492;
        PopulationList[44] = 347311;
        PopulationList[45] = 293317;
        PopulationList[46] = 258876;
        PopulationList[47] = 184145;
        PopulationList[48] = 185155;
        PopulationList[49] = 349664;
        PopulationList[50] = 289990;
        PopulationList[51] = 192118;
        PopulationList[52] = 255081;
        PopulationList[53] = 254682;
        PopulationList[54] = 355776;
        PopulationList[55] = 345163;
        PopulationList[56] = 258135;
        PopulationList[57] = 404599;
        PopulationList[58] = 345676;
        PopulationList[59] = 383410;
        PopulationList[60] = 152008;
        PopulationList[61] = 345170;
        PopulationList[62] = 329249;
        PopulationList[63] = 251519;
        PopulationList[64] = 397552;
        PopulationList[65] = 266252;
        PopulationList[66] = 163347;
        PopulationList[67] = 284914;
        PopulationList[68] = 160015;
        PopulationList[69] = 322459;
        PopulationList[70] = 227650;
        PopulationList[71] = 302823;
        PopulationList[72] = 231021;
        PopulationList[73] = 295278;
        PopulationList[74] = 189531;
        PopulationList[75] = 291875;
        PopulationList[76] = 214244;
        PopulationList[77] = 334341;
        PopulationList[78] = 252862;
        PopulationList[79] = 218075;
        PopulationList[80] = 333881;
        PopulationList[81] = 302955;
        PopulationList[82] = 309217;
        PopulationList[83] = 229922;
        PopulationList[84] = 225319;
        PopulationList[85] = 301819;
        PopulationList[86] = 287505;
        PopulationList[87] = 300427;
        PopulationList[88] = 280864;
        PopulationList[89] = 314289;
        PopulationList[90] = 408994;
        PopulationList[91] = 305030;
        PopulationList[92] = 281345;
        PopulationList[93] = 253398;
        PopulationList[94] = 263808;
        PopulationList[95] = 237443;
        PopulationList[96] = 347119;
        PopulationList[97] = 424690;
        PopulationList[98] = 248444;
        PopulationList[99] = 226830;
        #endregion

        var PopulationAttributes = new List<Attributes>();

        #region Default Attributes
        PopulationAttributes.Add(new Attributes(-1.13165913628244, 14.9768834716823, 1.82305348324052, 11.8834716822528));
        PopulationAttributes.Add(new Attributes(0.834023178807946, 18.3293667218543, 6.44298427152318, 15.6581125827815));
        PopulationAttributes.Add(new Attributes(7.32413504755931, 26.2224312741716, 11.2313159820215, 24.6289327897983));
        PopulationAttributes.Add(new Attributes(1.88313029114185, 21.8402849473467, 8.57784431137724, 19.6799504439397));
        PopulationAttributes.Add(new Attributes(-3.60239471511148, 16.5782411230388, 2.42877786952931, 13.6596820809249));
        PopulationAttributes.Add(new Attributes(-20.8614388639449, -0.622846402840135, -9.84755142528976, -5.24381330270439));
        PopulationAttributes.Add(new Attributes(-30.877378435518, -13.2605708245243, -13.9408033826638, -17.392177589852));
        PopulationAttributes.Add(new Attributes(5.12974880631098, 25.3472078056882, 12.62767282541, 23.742474569234));
        PopulationAttributes.Add(new Attributes(12.701650097366, 29.8621502511018, 10.0630316695706, 27.3239725325407));
        PopulationAttributes.Add(new Attributes(-23.5999166927002, -5.33218785796105, -8.38852441945225, -8.92689784442361));
        PopulationAttributes.Add(new Attributes(-14.9688796680498, 4.15663900414938, -3.24948132780083, 0.79201244813278));
        PopulationAttributes.Add(new Attributes(-19.9822898218565, -0.408896760079179, -2.8367538285238, -3.16595478695698));
        PopulationAttributes.Add(new Attributes(-2.08822923108637, 19.1692434549577, 7.77417027417027, 17.5613275613276));
        PopulationAttributes.Add(new Attributes(-37.3017573939134, -20.909237033862, -15.753321903129, -24.7588941277325));
        PopulationAttributes.Add(new Attributes(-32.5352990033223, -9.85724667774086, -7.13143687707641, -12.9443521594684));
        PopulationAttributes.Add(new Attributes(-21.0287725108673, 0.35603394742289, -4.81473814945146, -3.24673980542331));
        PopulationAttributes.Add(new Attributes(-26.3540043627298, -7.85291368027423, -9.75070115300717, -11.7471694193414));
        PopulationAttributes.Add(new Attributes(-29.837330552659, -9.98696558915537, -8.76694473409801, -13.2612095933264));
        PopulationAttributes.Add(new Attributes(18.2684205117665, 36.6812249511869, 17.8804850477854, 35.9043263796115));
        PopulationAttributes.Add(new Attributes(-2.15808632345294, 18.0364014560582, 10.2116484659386, 16.4378575143006));
        PopulationAttributes.Add(new Attributes(-1.66009636443276, 16.9831362242663, 7.67356548401227, 14.91020586947));
        PopulationAttributes.Add(new Attributes(-35.9919942522837, -18.1119778302371, -15.2350405419275, -22.0902186184953));
        PopulationAttributes.Add(new Attributes(-32.8978869574269, -16.0882689705423, -12.5752055792651, -19.324971375039));
        PopulationAttributes.Add(new Attributes(-26.0525227460711, -3.18186517783292, 0.928453267162947, -4.47063688999172));
        PopulationAttributes.Add(new Attributes(10.2858905800082, 30.231386260798, 15.9872480460716, 28.8518099547511));
        PopulationAttributes.Add(new Attributes(3.11591192355629, 21.9812006647279, 11.8264437058579, 20.5780016618197));
        PopulationAttributes.Add(new Attributes(-10.5254343730375, 8.86435001046682, 2.77423068871677, 6.74534226501989));
        PopulationAttributes.Add(new Attributes(-7.19604784191368, 12.2657306292252, 1.17264690587623, 9.3036921476859));
        PopulationAttributes.Add(new Attributes(2.258671124112, 20.8409945674885, 9.18094442122858, 18.998641872127));
        PopulationAttributes.Add(new Attributes(-10.9461869296129, 6.60180425889017, 1.73922165110668, 4.38581768593308));
        PopulationAttributes.Add(new Attributes(1.81061394380853, 20.3636836628512, 9.22112382934443, 18.3995837669095));
        PopulationAttributes.Add(new Attributes(12.0971393290801, 26.3742539617205, -5.24490635933319, 21.8841325375592));
        PopulationAttributes.Add(new Attributes(-23.3095188064719, -4.86394200462282, -10.4859214120614, -9.19258247530994));
        PopulationAttributes.Add(new Attributes(-5.70222405271829, 14.455313014827, 4.82753294892916, 11.9259678747941));
        PopulationAttributes.Add(new Attributes(-3.38858385993992, 17.5665596187714, 8.51289754480473, 15.7096239511033));
        PopulationAttributes.Add(new Attributes(-8.89894890207098, 11.9174732022063, 3.23238630450619, 9.46404412529919));
        PopulationAttributes.Add(new Attributes(-13.5964912280702, 7.42690058479532, -1.73611111111111, 4.3890977443609));
        PopulationAttributes.Add(new Attributes(-0.407887908666322, 22.1344058121432, 13.1214322781526, 20.8635184224183));
        PopulationAttributes.Add(new Attributes(18.2613555532974, 39.7027741083223, 20.3764861294584, 38.6815364292247));
        PopulationAttributes.Add(new Attributes(3.96952851554457, 21.6445336627548, 8.39870290302656, 19.189829112621));
        PopulationAttributes.Add(new Attributes(7.49148343140291, 25.4965417569939, 10.8026220708166, 23.3018478373077));

        #endregion

        for (int i = 0; i < 100; i++)
        {
            #region senatToSejm
            int senatToSejm = i + 1 switch
            {
                1 or 2 or 3 => 1,
                4 or 5 => 2,
                6 or 7 or 8 => 3,
                9 or 10 => 4,
                11 or 12 or 13 => 5,
                14 or 15 or 16 => 6,
                17 or 18 or 19 => 7,
                20 or 21 or 22 => 8,
                23 or 24 => 9,
                28 or 29 => 10,
                25 or 26 or 27 => 11,
                30 => 12,
                31 or 32 or 33 => 13,
                36 or 37 => 14,
                34 or 35 => 15,
                38 or 39 => 16,
                49 or 50 => 17,
                46 or 47 or 48 => 18,
                42 or 43 or 44 or 45 => 19,
                40 or 41 => 20,
                51 or 52 or 53 => 21,
                57 or 58 => 22,
                54 or 55 or 56 => 23,
                59 or 60 or 61 => 24,
                65 or 66 or 67 => 25,
                62 or 63 or 64 => 26,
                78 or 79 => 27,
                68 or 69 => 28,
                70 or 71 => 29,
                72 or 73 => 30,
                74 or 75 or 80 => 31,
                76 or 77 => 32,
                81 or 82 or 83 => 33,
                84 or 85 => 34,
                86 or 87 => 35,
                94 or 95 or 96 => 36,
                92 or 93 => 37,
                88 or 89 => 38,
                90 or 91 => 39,
                99 or 100 => 40,
                97 or 98 => 41,
                _ => 0
            };
            #endregion

            PopulationPerDistrict.Add((PopulationList[i], PopulationAttributes[senatToSejm - 1]));
        }
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
