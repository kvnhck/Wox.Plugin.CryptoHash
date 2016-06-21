using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Wox.Plugin.CryptoHash
{
    public class Plugin : IPlugin
    {
        private PluginInitContext _pluginContext;
        private Dictionary<string, HashAlgorithm> _algorithms;

        public List<Result> Query(Query query)
        {
            if (string.IsNullOrWhiteSpace(query.Search))
                return new List<Result>(0);

            return _algorithms
                .Select(a => new Result
                {
                    Title = GetHash(Encoding.UTF8, a.Value, query.Search),
                    SubTitle = $"{a.Key} hash of \"{query.Search}\"",
                    Action = (context =>
                    {
                        //TODO: copy hash to clipboard
                        return false;
                    })
                })
                .ToList();
        }

        public void Init(PluginInitContext context)
        {
            _pluginContext = context;

            _algorithms = new Dictionary<string, HashAlgorithm>
            {
                { "MD5", MD5.Create() },
                { "SHA1", SHA1.Create() },
                { "SHA256", SHA256.Create() },
                { "SHA384", SHA384.Create() },
                { "SHA512", SHA512.Create() },
                { "RIPEMD160", RIPEMD160.Create() }
            };
        }

        private static string GetHash(Encoding encoding, HashAlgorithm hasher, string input)
        {
            var data = encoding.GetBytes(input);
            var hashBytes = hasher.ComputeHash(data);
            return BitConverter.ToString(hashBytes).Replace("-", "").ToLower();
        }
    }
}
