using PCLCrypto;
using System.Text;
using AsymmetricAlgorithm = PCLCrypto.AsymmetricAlgorithm;

namespace MarketData.IG
{
    public class Rsa
    {
        private ICryptographicKey _key { get; set; }

        public Rsa(byte[] key, bool intermediateConvertToBase64BeforeEncryption = false, bool isPrivateKey = false)
        {
            try
            {
                var rsa = WinRTCrypto.AsymmetricKeyAlgorithmProvider.OpenAlgorithm(AsymmetricAlgorithm.RsaPkcs1);
                IntermediateConvertToBase64BeforeEncryption = intermediateConvertToBase64BeforeEncryption;
                CanDecrypt = isPrivateKey;


                if (isPrivateKey)
                    _key = rsa.ImportKeyPair(key);
                else
                    _key = rsa.ImportPublicKey(key);
            }
            catch (Exception ex)
            {
                string exception = ex.Message;
            }
        }

        protected bool IntermediateConvertToBase64BeforeEncryption { get; set; }

        protected bool CanDecrypt { get; set; }

        public byte[] RsaEncrypt(string data)
        {
            var databuf = Encoding.UTF8.GetBytes(data);

            if (IntermediateConvertToBase64BeforeEncryption)			   
                databuf = Encoding.UTF8.GetBytes(Convert.ToBase64String(databuf));
            return WinRTCrypto.CryptographicEngine.Encrypt(_key, databuf).ToArray();
        }

        public string RsaDecrypt(byte[] encrypted)
        {
            if (!CanDecrypt)
                throw new Exception("Unable to Decrypt, class was only initalised with a public key");

            var data = WinRTCrypto.CryptographicEngine.Decrypt(_key, encrypted);

            if (IntermediateConvertToBase64BeforeEncryption)
                data = Convert.FromBase64String(Encoding.UTF8.GetString(data, 0, data.Length));

            return Encoding.UTF8.GetString(data, 0, data.Length);
        }
    }
}
