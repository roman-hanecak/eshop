using System;
using System.Collections.Generic;

namespace Visma.Bootcamp.ApiTests.Contexts
{
    public class BaseContext
    {
        protected Dictionary<string, Guid> ContextDictionary { get; }

        public BaseContext()
        {
            ContextDictionary = new Dictionary<string, Guid>();
        }

        public Guid GetIdOrException(string name)
        {
            if (!ContextDictionary.ContainsKey(name))
            {
                throw new Exception($"Key [{name}] was not found in dictionary: [{string.Join(",", new List<string>(ContextDictionary.Keys))}]");
            }

            return ContextDictionary[name];
        }

        public Guid GetIdOrDefault(string key)
        {
            key = key?.Trim();
            if (!string.IsNullOrEmpty(key))
            {
                if (!ContextDictionary.ContainsKey(key))
                {
                    // default for non empty value is non existent id
                    return Guid.Empty;
                }

                return ContextDictionary[key];
            }

            // default for empty value is 0
            return default;
        }

        public Guid? GetIdOrDefaultNullable(string key)
        {
            Guid idOrDefault = GetIdOrDefault(key);
            return idOrDefault == Guid.Empty ? (Guid?)null : idOrDefault;
        }
    }
}
