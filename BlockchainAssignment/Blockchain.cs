using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace BlockchainAssignment
{
    class Blockchain
    {
        public List<Block> chain;
        public int difficulty = 4;
        public float miningReward = 10;
        public double targetBlockTime = 3.0;

        public Blockchain()
        {
            chain = new List<Block>();
            chain.Add(CreateGenesisBlock());
        }

        public Block CreateGenesisBlock()
        {
            return new Block(0, "Genesis Block", "0");
        }

        public Block GetLatestBlock()
        {
            return chain[chain.Count - 1];
        }

        public double AddBlock(Block newBlock, string minerAddress)
        {
            newBlock.previousHash = GetLatestBlock().hash;

            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            newBlock.MineBlock(difficulty);

            stopwatch.Stop();

            chain.Add(newBlock);

            AdjustDifficulty(stopwatch.Elapsed.TotalSeconds);

            Transaction rewardTransaction = new Transaction(
                null,
                minerAddress,
                miningReward
            );

            List<Transaction> rewardTransactions = new List<Transaction>();
            rewardTransactions.Add(rewardTransaction);

            Block rewardBlock = new Block(chain.Count, rewardTransactions);
            rewardBlock.previousHash = newBlock.hash;
            rewardBlock.MineBlock(difficulty);

            chain.Add(rewardBlock);

            return stopwatch.Elapsed.TotalSeconds;
        }

        public void AdjustDifficulty(double miningTime)
        {
            if (miningTime < targetBlockTime / 2 && difficulty < 6)
            {
                difficulty++;
            }
            else if (miningTime > targetBlockTime * 2 && difficulty > 1)
            {
                difficulty--;
            }
        }

        public bool IsChainValid()
        {
            for (int i = 1; i < chain.Count; i++)
            {
                Block currentBlock = chain[i];
                Block previousBlock = chain[i - 1];

                if (currentBlock.hash != currentBlock.CalculateHash())
                {
                    return false;
                }

                if (currentBlock.previousHash != previousBlock.hash)
                {
                    return false;
                }

                foreach (Transaction transaction in currentBlock.transactions)
                {
                    if (!transaction.IsTransactionValid())
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        public string ReadAllBlocks()
        {
            string output = "";

            foreach (Block block in chain)
            {
                output += block.ReadBlock() + "\n";
            }

            return output;
        }

        public float GetBalance(string address)
        {
            float balance = 0;

            foreach (Block block in chain)
            {
                foreach (Transaction transaction in block.transactions)
                {
                    if (transaction.fromAddress == address)
                    {
                        balance -= transaction.amount;
                    }

                    if (transaction.toAddress == address)
                    {
                        balance += transaction.amount;
                    }
                }
            }

            return balance;
        }
    }
}