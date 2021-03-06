using System.Net.Security;
using StackExchange.Redis;

namespace AspNetCore.StackExchange.Redis;

public class RedisConfiguration
{
    /// <summary>
        /// The key separation prefix used for all cache entries
        /// </summary>
        public string KeyPrefix { get; set; }

        /// <summary>
        /// The password or access key
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// Specify if the connection can use Admin commands like flush database
        /// </summary>
        /// <value>
        ///   <c>true</c> if can use admin commands; otherwise, <c>false</c>.
        /// </value>
        public bool AllowAdmin { get; set; } = false;

        /// <summary>
        /// Specify if the connection is a secure connection or not.
        /// </summary>
        /// <value>
        ///   <c>true</c> if is secure; otherwise, <c>false</c>.
        /// </value>
        public bool Ssl { get; set; } = false;

        /// <summary>
        /// The connection timeout
        /// </summary>
        public int ConnectTimeout { get; set; } = 5000;

        /// <summary>
        /// Time (ms) to allow for synchronous operations
        /// </summary>
        public int SyncTimeout { get; set; } = 1000;

        /// <summary>
        /// If true, Connect will not create a connection while no servers are available
        /// </summary>
        public bool AbortOnConnectFail { get; set; }

        /// <summary>
        /// Database Id
        /// </summary>
        /// <value>
        /// The database id, the default value is 0
        /// </value>
        public int Database { get; set; } = 0;

        /// <summary>
        /// The host of Redis Servers
        /// </summary>
        /// <value>
        /// The ips or names
        /// </value>
        public RedisHost[] Hosts { get; set; }

        /// <summary>
        /// A RemoteCertificateValidationCallback delegate responsible for validating the certificate supplied by the remote party; note
        /// that this cannot be specified in the configuration-string.
        /// </summary>
        public event RemoteCertificateValidationCallback CertificateValidation;
        public ConfigurationOptions ConfigurationOptions
        {
            get
            {
                var configurationOptions = new ConfigurationOptions
                {
                    Ssl = Ssl,
                    AllowAdmin = AllowAdmin,
                    Password = Password,
                    ConnectTimeout = ConnectTimeout,
                    SyncTimeout = SyncTimeout,
                    AbortOnConnectFail = AbortOnConnectFail
                };

                foreach (var redisHost in Hosts)
                {
                    configurationOptions.EndPoints.Add(redisHost.Host, redisHost.Port);
                }

                configurationOptions.CertificateValidation += CertificateValidation;

                return configurationOptions;
            }
        }
}