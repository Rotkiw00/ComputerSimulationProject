using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectionSimulatorLibrary;

public class TimeManager
{
    private int _time = 0;
    // CONTROL VALUE - liczba godzin, ustawiona wartość 2 z powodów testowych, można zwiększyć (zalecane 1588)
    private int _end = 2;

    public bool Done { get; private set; } = false;
    public int Interval { get; set; } = 1;
    public int CurrentTime 
    { 
        get { return _time; } 
        set 
        {
            if (value < 0) return; 
            else if (value >= _end) _time = _end;
            else _time = value;

            if(_time == _end) Done = true;
        } 
    }

    public void Forward()
    {
        CurrentTime += Interval;
    }

    public void Reset() { _time = 0; Done = false; }
}
