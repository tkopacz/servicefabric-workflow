using Microsoft.ApplicationInsights;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace TKWebUI
{
    public static class Util
    {
        public static void MeasureTime(this TelemetryClient tc, string metricName,Action operation)
        {
            Stopwatch sw = Stopwatch.StartNew();
            try
            {
                operation();
                sw.Stop();
                tc.TrackMetric(metricName + "Ms", sw.ElapsedMilliseconds);
                tc.TrackMetric(metricName + "Ticks", sw.ElapsedTicks);
            }
            catch (Exception ex)
            {
                tc.TrackException(ex);
            }

        }
    }
}
