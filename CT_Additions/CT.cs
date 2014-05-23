using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CT_Additions
{
    public static class CT
    {

        public static double Clamp(double value, double min, double max)
        {
            return Convert.ToDouble(Clamp((float)value, (float)min, (float)max));
        }

        public static float Clamp(float value, float min, float max)
        {
            if (value < min) return min;
            else if (value > max) return max;
            else return value;
        }
    }

    public static class Extensions
    {
        /// <summary>
        /// Get substring of specified number of characters on the right.
        /// </summary>
        public static string Right(this string value, int length)
        {
            if (String.IsNullOrEmpty(value)) return string.Empty;

            //return value.Length <= length ? value : value.Substring(value.Length - length);
            if (value.Length <= length)
            { return value; }
            else
            { return value.Substring(value.Length - length); }

        }

        /// <summary>
        /// Get substring of specified number of characters on the left.
        /// </summary>
        public static string Left(this string value, int length)
        {
            if (String.IsNullOrEmpty(value)) return string.Empty;

            if (value.Length <= length)
            { return value; }
            else
            { return value.Substring(0,length); }
            
        }
    }
} 
