using System;
using UnityEngine;

namespace Extensions
{
    public class EnumExtensions
    {
        /// <summary>
        /// Parses the specified my string.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="myString">My string.</param>
        /// <returns></returns>
        public static T Parse<T>(string myString) where T : struct, IConvertible
        {
            try
            {
                T enumerable = (T)System.Enum.Parse(typeof(T), myString);
                return enumerable;
            }
            catch (System.Exception)
            {
                Debug.LogErrorFormat("Parse: Can't convert {0} to enum, please check the spell.", myString);
            }
            return default(T);
        }
    }
}