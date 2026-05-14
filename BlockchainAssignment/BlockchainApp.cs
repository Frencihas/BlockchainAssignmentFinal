using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace BlockchainAssignment
{
    public partial class BlockchainApp : Form
    {
        public BlockchainApp()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Blockchain demoChain = new Blockchain();

            string user1Private;
            Wallet.Wallet user1 =
                new Wallet.Wallet(out user1Private);

            string user2Private;
            Wallet.Wallet user2 =
                new Wallet.Wallet(out user2Private);

            string minerPrivate;
            Wallet.Wallet miner =
                new Wallet.Wallet(out minerPrivate);

            Transaction tx1 =
                new Transaction(
                    user1.publicID,
                    user2.publicID,
                    20,
                    5
                );

            Transaction tx2 =
                new Transaction(
                    user1.publicID,
                    user2.publicID,
                    15,
                    1
                );

            Transaction tx3 =
                new Transaction(
                    user2.publicID,
                    user1.publicID,
                    30,
                    10
                );

            tx1.SignTransaction(user1Private);
            tx2.SignTransaction(user1Private);
            tx3.SignTransaction(user2Private);

            List<Transaction> pool =
                new List<Transaction>();

            pool.Add(tx1);
            pool.Add(tx2);
            pool.Add(tx3);

            List<Transaction> selected =
                demoChain.SelectTransactions(
                    pool,
                    "Greedy"
                );

            Block block =
                new Block(1, selected);

            double miningTime =
                demoChain.AddBlock(
                    block,
                    miner.publicID
                );

            richTextBox1.Text =
                "TRANSACTION PREFERENCE TEST\n\n" +
                "Selection Mode: Greedy (Highest Fee First)\n\n" +
                "Mining Time: " +
                miningTime.ToString("0.000") +
                " seconds\n\n" +
                demoChain.ReadAllBlocks() +
                "\nBlockchain Valid: " +
                demoChain.IsChainValid();
        }
    }
}