using System;

namespace FunkySheep
{
    public static class Time
    {

        /// <summary>
        /// Get the unix epoch time (same has the javascript function) 
        /// </summary>
        /// <param name="date">The date to be converted to javascript epoxh time</param>
        /// <returns></returns>
        public static double getTime(DateTime date)
        {
            DateTime unixDate = new DateTime(1970, 1, 1);
            double unixTimestamp = date.ToUniversalTime().Subtract(unixDate).TotalMilliseconds;
            return Math.Round(unixTimestamp);
        }

        /// <summary>
        /// Get the time unix time stamp of now 
        /// </summary>
        /// <returns></returns>
        public static double Now()
        {
            return getTime(DateTime.Now);
        }
    }
}
