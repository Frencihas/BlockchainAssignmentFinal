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

            string senderPrivateKey;
            Wallet.Wallet senderWallet = new Wallet.Wallet(out senderPrivateKey);

            string receiverPrivateKey;
            Wallet.Wallet receiverWallet = new Wallet.Wallet(out receiverPrivateKey);

            string minerPrivateKey;
            Wallet.Wallet minerWallet = new Wallet.Wallet(out minerPrivateKey);

            Transaction tx = new Transaction(
                senderWallet.publicID,
                receiverWallet.publicID,
                25
            );

            tx.SignTransaction(senderPrivateKey);

            List<Transaction> transactions = new List<Transaction>();
            transactions.Add(tx);

            Block transactionBlock = new Block(1, transactions);

            double miningTime = demoChain.AddBlock(
                transactionBlock,
                minerWallet.publicID
            );

            richTextBox1.Text =
                "FINAL BLOCKCHAIN DEMO\n\n" +
                "Mining Time: " + miningTime.ToString("0.000") + " seconds" +
                "\nCurrent Difficulty: " + demoChain.difficulty +
                "\nTarget Block Time: " + demoChain.targetBlockTime + " seconds\n\n" +
                demoChain.ReadAllBlocks() +
                "\nBlockchain Valid: " + demoChain.IsChainValid() +
                "\n\nSender Balance: " + demoChain.GetBalance(senderWallet.publicID) +
                "\nReceiver Balance: " + demoChain.GetBalance(receiverWallet.publicID) +
                "\nMiner Balance: " + demoChain.GetBalance(minerWallet.publicID);
        }
    }
}