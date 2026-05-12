using System;
using BlockchainAssignment.Wallet;

namespace BlockchainAssignment
{
    class Transaction
    {
        public string fromAddress;
        public string toAddress;
        public float amount;
        public string signature;

        public Transaction(string fromAddress, string toAddress, float amount)
        {
            this.fromAddress = fromAddress;
            this.toAddress = toAddress;
            this.amount = amount;
        }

        public string CalculateTransactionHash()
        {
            string rawData = fromAddress + toAddress + amount.ToString();

            return Convert.ToBase64String(
                System.Text.Encoding.UTF8.GetBytes(rawData)
            );
        }

        public void SignTransaction(string privateKey)
        {
            string transactionHash = CalculateTransactionHash();

            signature = Wallet.Wallet.CreateSignature(
                fromAddress,
                privateKey,
                transactionHash
            );
        }

        public bool IsTransactionValid()
        {
            if (fromAddress == null)
                return true;

            if (string.IsNullOrEmpty(signature))
                return false;

            return Wallet.Wallet.ValidateSignature(
                fromAddress,
                CalculateTransactionHash(),
                signature
            );
        }
    }
}