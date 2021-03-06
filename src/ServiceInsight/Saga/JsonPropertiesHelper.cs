﻿namespace ServiceInsight.Saga
{
    using System.Collections.Generic;
    using System.Linq;
    using Newtonsoft.Json;

    class JsonPropertiesHelper
    {
        static readonly IList<string> StandardKeys = new List<string> { "$type", "Id", "Originator", "OriginalMessageId" };

        public static IList<KeyValuePair<string, string>> ProcessValues(string stateAfterChange) => JsonConvert.DeserializeObject<Dictionary<string, object>>(stateAfterChange)
                  .Where(m => StandardKeys.All(s => s != m.Key))
                  .Select(f => new KeyValuePair<string, string>(f.Key, f.Value?.ToString()))
                  .ToList();

        public static IList<KeyValuePair<string, string>> ProcessArray(string stateAfterChange) => ProcessValues(stateAfterChange.TrimStart('[').TrimEnd(']'));
    }
}