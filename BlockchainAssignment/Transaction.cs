using System;
using BlockchainAssignment.Wallet;

namespace BlockchainAssignment
{
    class Transaction
    {
        public string fromAddress;
        public string toAddress;
        public float amount;
        public float fee;
        public string signature;
        public DateTime timestamp;

        public Transaction(string fromAddress, string toAddress, float amount, float fee = 0)
        {
            this.fromAddress = fromAddress;
            this.toAddress = toAddress;
            this.amount = amount;
            this.fee = fee;
            this.timestamp = DateTime.Now;
        }

        public string CalculateTransactionHash()
        {
            string rawData =
                fromAddress +
                toAddress +
                amount.ToString() +
                fee.ToString() +
                timestamp.ToString();

            return Convert.ToBase64String(
                System.Text.Encoding.UTF8.GetBytes(rawData)
            );
        }

        public void SignTransaction(string privateKey)
        {
            if (fromAddress == null)
            {
                return;
            }

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
            {
                return true;
            }

            if (string.IsNullOrEmpty(signature))
            {
                return false;
            }

            return Wallet.Wallet.ValidateSignature(
                fromAddress,
                CalculateTransactionHash(),
                signature
            );
        }
    }
}