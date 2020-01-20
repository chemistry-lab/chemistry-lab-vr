using System.Security.Cryptography.X509Certificates;
using System.Security.Cryptography;
using System.Text;
using System;

namespace DialogFlow.Utilities
{
    public static class Jwt
    {
        public static string GetJwt(string email, string keyfile, string scope)
        {
            X509Certificate2 certificate = new X509Certificate2(keyfile, "notasecret");
            object header = new { typ = "JWT", alg = "RS256" };

            int[] time = GetExpiryAndIssueDate();
            object claims = new
            {
                iss = email,
                scope = scope,
                aud = "https://oauth2.googleapis.com/token",
                iat = time[0],
                exp = time[1],
            };

            byte[] headerRaw = Encoding.UTF8.GetBytes(Json.ToJson(header));
            string headerEncoded = Convert.ToBase64String(headerRaw);
            byte[] claimsRaw = Encoding.UTF8.GetBytes(Json.ToJson(claims));
            string claimsEncoded = Convert.ToBase64String(claimsRaw);

            byte[] inputRaw = Encoding.UTF8.GetBytes(headerEncoded + "." + claimsEncoded);
            RSACryptoServiceProvider rsa = (RSACryptoServiceProvider)certificate.PrivateKey;
            byte[] signatureRaw = rsa.SignData(inputRaw, "SHA256");
            string signatureEncoded = Convert.ToBase64String(signatureRaw);

            return headerEncoded + "." + claimsEncoded + "." + signatureEncoded;
        }

        private static int[] GetExpiryAndIssueDate()
        {
            DateTime utc = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            var issueTime = DateTime.UtcNow;

            var iat = (int)issueTime.Subtract(utc).TotalSeconds;
            var exp = (int)issueTime.AddMinutes(55).Subtract(utc).TotalSeconds;

            return new[] { iat, exp };
        }
    }
}
