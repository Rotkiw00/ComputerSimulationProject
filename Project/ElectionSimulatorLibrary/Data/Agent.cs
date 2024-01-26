﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectionSimulatorLibrary;

public abstract class Agent
{
    public int LockTime { get; set; } = -1;
    public bool Available { get; set; } = true;

}
