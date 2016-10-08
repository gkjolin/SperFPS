float linearSmoothStep(float a, float b, float x)
{
    float t = saturate((x - a)/(b - a));
    //return t*t*(3.0 - (2.0*t));
    return t;
}