using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace BlockchainAssignment
{
    class Block
    {
        public int index;
        public DateTime timestamp;
        public string data;
        public string previousHash;
        public string hash;
        public int nonce;

        public List<Transaction> transactions;

        public Block(int index, string data, string previousHash = "")
        {
            this.index = index;
            this.timestamp = DateTime.Now;
            this.data = data;
            this.previousHash = previousHash;
            this.nonce = 0;

            transactions = new List<Transaction>();

            hash = CalculateHash();
        }

        public Block(int index, List<Transaction> transactions, string previousHash = "")
        {
            this.index = index;
            this.timestamp = DateTime.Now;
            this.transactions = transactions;
            this.previousHash = previousHash;
            this.nonce = 0;

            data = "Transaction Block";

            hash = CalculateHash();
        }

        public string CalculateHash()
        {
            SHA256 sha256 = SHA256.Create();

            string transactionData = "";

            foreach (Transaction transaction in transactions)
            {
                transactionData += transaction.CalculateTransactionHash();
            }

            string rawData =
                index.ToString() +
                timestamp.ToString() +
                data +
                previousHash +
                nonce.ToString() +
                transactionData;

            byte[] bytes = sha256.ComputeHash(
                Encoding.UTF8.GetBytes(rawData)
            );

            StringBuilder builder = new StringBuilder();

            for (int i = 0; i < bytes.Length; i++)
            {
                builder.Append(bytes[i].ToString("x2"));
            }

            return builder.ToString();
        }

        public void MineBlock(int difficulty)
        {
            string target = new string('0', difficulty);

            while (hash.Substring(0, difficulty) != target)
            {
                nonce++;
                hash = CalculateHash();
            }
        }

        public string ReadBlock()
        {
            string output =
                "Block Index: " + index +
                "\nTimestamp: " + timestamp +
                "\nData: " + data +
                "\nPrevious Hash: " + previousHash +
                "\nHash: " + hash +
                "\nNonce: " + nonce;

            if (transactions.Count > 0)
            {
                output += "\n\nTransactions:";

                foreach (Transaction transaction in transactions)
                {
                    output +=
                        "\n\nFrom: " + transaction.fromAddress +
                        "\nTo: " + transaction.toAddress +
                        "\nAmount: " + transaction.amount +
                        "\nFee: " + transaction.fee +
                        "\nValid: " + transaction.IsTransactionValid();
                }
            }

            output += "\n\n-----------------------------\n";

            return output;
        }
    }
}