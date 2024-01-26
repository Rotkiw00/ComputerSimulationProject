using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectionSimulatorLibrary;

public class TimeManager
{
    private int _time = 0;

    public bool Done { get; private set; } = false;
    public int Interval { get; set; } = 1;
    public int CurrentTime 
    { 
        get { return _time; } 
        set 
        {
            if (value < 0) return; 
            else if (value >= 1589) _time = 1589;
            else _time = value;

            if(_time == 1589) Done = true;
        } 
    }

    public void Forward()
    {
        CurrentTime += Interval;
    }
}
