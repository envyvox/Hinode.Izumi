using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Security.Cryptography;
using System.Text;
using Hangfire.Dashboard;
using Microsoft.AspNetCore.Http;

namespace Hinode.Izumi.Framework.Hangfire
{
        public class BasicAuthAuthorizationUser
        {
            /// <summary>
            /// Represents user's name
            /// </summary>
            public string Login { get; set; }

            /// <summary>
            /// SHA1 hashed password
            /// </summary>
            public byte[] Password { get; set; }

            /// <summary>
            /// Setter to update password as plain text
            /// </summary>
            public string PasswordClear
            {
                set
                {
                    using (var cryptoProvider = SHA1.Create())
                    {
                        Password = cryptoProvider.ComputeHash(Encoding.UTF8.GetBytes(value));
                    }
                }
            }

            /// <summary>
            /// Validate user
            /// </summary>
            /// <param name="login">User name</param>
            /// <param name="password">User password</param>
            /// <param name="loginCaseSensitive">Whether or not login checking is case sensitive</param>
            /// <returns></returns>
            public bool Validate(string login, string password, bool loginCaseSensitive)
            {
                if (String.IsNullOrWhiteSpace(login))
                    throw new ArgumentNullException("login");

                if (String.IsNullOrWhiteSpace(password))
                    throw new ArgumentNullException("password");

                if (login.Equals(Login,
                    loginCaseSensitive ? StringComparison.CurrentCulture : StringComparison.OrdinalIgnoreCase))
                {
                    using (var cryptoProvider = SHA1.Create())
                    {
                        byte[] passwordHash = cryptoProvider.ComputeHash(Encoding.UTF8.GetBytes(password));
                        return StructuralComparisons.StructuralEqualityComparer.Equals(passwordHash, Password);
                    }
                }

                return false;
            }
        }

        public class BasicAuthAuthorizationFilter : IDashboardAuthorizationFilter
        {
            private readonly BasicAuthAuthorizationFilterOptions _options;

            public BasicAuthAuthorizationFilter()
                : this(new BasicAuthAuthorizationFilterOptions())
            {
            }

            public BasicAuthAuthorizationFilter(BasicAuthAuthorizationFilterOptions options)
            {
                _options = options;
            }

            private bool Challenge(HttpContext context)
            {
                context.Response.StatusCode = 401;
                context.Response.Headers.Append("WWW-Authenticate", "Basic realm=\"Hangfire Dashboard\"");
                return false;
            }

            public bool Authorize(DashboardContext _context)
            {
                var context = _context.GetHttpContext();
                if ((_options.SslRedirect) && (context.Request.Scheme != "https"))
                {
                    string redirectUri =
                        new UriBuilder("https", context.Request.Host.ToString(), 443, context.Request.Path).ToString();

                    context.Response.StatusCode = 301;
                    context.Response.Redirect(redirectUri);
                    return false;
                }

                if ((_options.RequireSsl) && (context.Request.IsHttps == false))
                {
                    return false;
                }

                string header = context.Request.Headers["Authorization"];

                if (String.IsNullOrWhiteSpace(header) == false)
                {
                    AuthenticationHeaderValue authValues = AuthenticationHeaderValue.Parse(header);

                    if ("Basic".Equals(authValues.Scheme, StringComparison.OrdinalIgnoreCase))
                    {
                        string parameter = Encoding.UTF8.GetString(Convert.FromBase64String(authValues.Parameter));
                        var parts = parameter.Split(':');

                        if (parts.Length > 1)
                        {
                            string login = parts[0];
                            string password = parts[1];

                            if ((String.IsNullOrWhiteSpace(login) == false) &&
                                (String.IsNullOrWhiteSpace(password) == false))
                            {
                                return _options
                                           .Users
                                           .Any(user => user.Validate(login, password, _options.LoginCaseSensitive))
                                       || Challenge(context);
                            }
                        }
                    }
                }

                return Challenge(context);
            }
        }

        public class BasicAuthAuthorizationFilterOptions
        {

            public BasicAuthAuthorizationFilterOptions()
            {
                SslRedirect = true;
                RequireSsl = true;
                LoginCaseSensitive = true;
                Users = new BasicAuthAuthorizationUser[] { };
            }

            /// <summary>
            /// Redirects all non-SSL requests to SSL URL
            /// </summary>
            public bool SslRedirect { get; set; }

            /// <summary>
            /// Requires SSL connection to access Hangfire dahsboard. It's strongly recommended to use SSL when you're using basic authentication.
            /// </summary>
            public bool RequireSsl { get; set; }

            /// <summary>
            /// Whether or not login checking is case sensitive.
            /// </summary>
            public bool LoginCaseSensitive { get; set; }

            /// <summary>
            /// Represents users list to access Hangfire dashboard.
            /// </summary>
            public IEnumerable<BasicAuthAuthorizationUser> Users { get; set; }
        }
}
