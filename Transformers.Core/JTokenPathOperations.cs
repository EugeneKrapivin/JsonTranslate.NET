using System;
using Newtonsoft.Json.Linq;

namespace Transformers.Core
{
    public class JTokenPathOperations
    {
        public void CreateByPath(JToken root, string path)
        {
            if (root == null) throw new ArgumentNullException(nameof(root));
            if (string.IsNullOrEmpty(path)) throw new ArgumentNullException(nameof(path));

            var segmentNames = path.Split('.');

            var cur = root;
            foreach (var segmentName in segmentNames)
            {
                JToken segment = segmentName.EndsWith("[]") ? new JArray() : new JObject();

                var name = segmentName.EndsWith("[]") ? segmentName.Replace("[]", "") : segmentName;

                if (cur.Type == JTokenType.Array && cur is JArray j)
                {
                    j.Add(segment);
                }
                else if (cur.Type == JTokenType.Object && cur is JObject o)
                {
                    if (!o.ContainsKey(name))
                    {
                        o.Add(name, segment);
                    }
                }
                else
                {
                    throw new NotSupportedException("whats up doc?");
                }
            }
        }

        public JToken SelectByPath(JToken root, string path)
        {
            if (root == null) throw new ArgumentNullException(nameof(root));
            if (string.IsNullOrEmpty(path)) throw new ArgumentNullException(nameof(path));

            return root.SelectToken(path);
        }

        public bool SetByPath(JToken root, JToken element, string path)
        {
            if (root == null) throw new ArgumentNullException(nameof(root));
            if (string.IsNullOrEmpty(path)) throw new ArgumentNullException(nameof(path));

            var segmentNames = path.Split('.');

            var cur = root;

            foreach (var segmentName in segmentNames)
            {
                if (root is JObject o)
                {
                    cur = o[segmentName];
                }
                else if (root is JArray j)
                {

                }
            }

            return false;
        }
    }
}