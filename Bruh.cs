using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Juegazo
{
    public class Bruh
    {
    public float lerp(float a, float b, float t) {
        return a+(b-a)*t; // linear interpolation function, not really needed
    }
    public float inverseLerp(float a, float b, float value) {
        return (value-a)/(b-a);
    }
    public float mixedFunctions(float fr){ //smoothing thing (its really good i promise)
        float v1 = fr*fr;
        float v2 = 1-(1-fr)*(1-fr);
        return lerp(v1,v2,fr);
    }
    }
}