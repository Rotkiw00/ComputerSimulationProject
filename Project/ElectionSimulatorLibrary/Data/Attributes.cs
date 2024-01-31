using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectionSimulatorLibrary;

public class Attributes
{
    public Attributes(int cp, int ee, int sc, int il)
    {
        Set(cp, ee, sc, il);
    }

    public Attributes(double cp, double ee, double sc, double il)
    {
        int cpi = (int)Math.Round(cp);
        int eei = (int)Math.Round(ee);
        int sci = (int)Math.Round(sc);
        int ili = (int)Math.Round(il);

        Set(cpi, eei, sci, ili);
    }

    private void Set(int cp, int ee, int sc, int il)
    {
        Conservatism_Progressivism = cp;
        Euroscepticism_Euroenthusiasm = ee;
        Socialism_Capitalism = sc;
        IlliberalDemocracy_LiberalDemocracy = il;
    }

    // L: -100 -> 100 :P

    private int _c_p = 0;
    private int _e_e = 0;
    private int _s_c = 0;
    private int _i_l = 0;

    public int Conservatism_Progressivism
    {
        get => _c_p;
        set
        {
            if (value < -100) _c_p = -100;
            else if (value > 100) _c_p = 100;
            else _c_p = value;
        }
    }
    public int Euroscepticism_Euroenthusiasm
    {
        get => _e_e;
        set
        {
            if (value < -100) _e_e = -100;
            else if (value > 100) _e_e = 100;
            else _e_e = value;
        }
    }
    public int Socialism_Capitalism
    {
        get => _s_c;
        set
        {
            if (value < -100) _s_c = -100;
            else if (value > 100) _s_c = 100;
            else _s_c = value;
        }
    }
    public int IlliberalDemocracy_LiberalDemocracy
    {
        get => _i_l;
        set
        {
            if (value < -100) _i_l = -100;
            else if (value > 100) _i_l = 100;
            else _i_l = value;
        }
    }

    public double CalculateDifference(Attributes other)
    {
        int n_c_p = _c_p + 100;
        int n_e_e = _e_e + 100;
        int n_s_c = _s_c + 100;
        int n_i_l = _i_l + 100;

        int o_c_p = other.Conservatism_Progressivism + 100;
        int o_e_e = other.Euroscepticism_Euroenthusiasm + 100;
        int o_s_c = other.Socialism_Capitalism + 100;
        int o_i_l = other.IlliberalDemocracy_LiberalDemocracy + 100;

        int d_c_p = n_c_p - o_c_p;
        int d_e_e = n_e_e - o_e_e;
        int d_s_c = n_s_c - o_s_c;
        int d_i_l = n_i_l - o_i_l;

        d_c_p = d_c_p < 0 ? -d_c_p : d_c_p;
        d_e_e = d_e_e < 0 ? -d_e_e : d_e_e;
        d_s_c = d_s_c < 0 ? -d_s_c : d_s_c;
        d_i_l = d_i_l < 0 ? -d_i_l : d_i_l;

        double result = (d_c_p + d_e_e + d_s_c + d_i_l) / 4.0;
        result /= 2.0;

        // 0  - 24 extremely similar
        // 25 - 49 similar
        // 50 - 74 different
        // 75 - 100 extremely different

        return result;
    }
}
