using System;
using System.Linq;
using Microsoft.AspNetCore.Routing;

namespace nrdkrmp.AzureFunctionsRoutePriority
{
    public class DefaultRouteComparison
    {
        public static int LiteralsFirst(Route x, Route y)
        {
            var xTemplate = x.ParsedTemplate;
            var yTemplate = y.ParsedTemplate;

            for (var i = 0; i < xTemplate.Segments.Count; i++)
            {
                if (yTemplate.Segments.Count <= i)
                    return -1;

                var xSegment = xTemplate.Segments[i].Parts[0];
                var ySegment = yTemplate.Segments[i].Parts[0];

                if (!xSegment.IsParameter && ySegment.IsParameter)
                    return -1;

                if (xSegment.IsParameter && !ySegment.IsParameter)
                    return 1;

                if (xSegment.IsParameter)
                {
                    if (xSegment.InlineConstraints.Count() > ySegment.InlineConstraints.Count())
                    {
                        return -1;
                    }
                    else if (xSegment.InlineConstraints.Count() < ySegment.InlineConstraints.Count())
                    {
                        return 1;
                    }
                }
                else
                {
                    var comparison = string.Compare(xSegment.Text, ySegment.Text, StringComparison.OrdinalIgnoreCase);
                    if (comparison != 0)
                        return comparison;
                }
            }

            if (yTemplate.Segments.Count > xTemplate.Segments.Count)
                return 1;

            return 0;
        }
    }
}