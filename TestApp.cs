using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;

using ki113d.CsBloocoin;

namespace CsBloocoinTestApp {

    class TestApp {

        static void Main( String[] args ) {
            BloocoinClient blc = new BloocoinClient();
            String[] bloostamp;
            if (!blc.bloostampExists()) { // Bloostamp not found, create a new one.
                do {
                    bloostamp = blc.generateBloostamp().Split(':');
                } while (!blc.register(bloostamp[0], bloostamp[1]));
                blc.writeBloostamp(bloostamp[0], bloostamp[1]);
                Console.WriteLine("New bloostamp created!");
            } else
                bloostamp = blc.readBloostamp().Split(':'); // Bloostamp found, read it.
            blc.setAddrAndPwd(bloostamp[0], bloostamp[1]);

            try {
                // Get current coin count.
                CmdReply<int> coins = blc.getReply<int>("my_coins");
                // Get all transactions for this account.
                CmdReply<List<Transaction>> trans = blc.getReply<List<Transaction>>("transactions");
                Transaction last = (trans.payload["transactions"].Count != 0) ?
                    trans.payload["transactions"].Last() : new Transaction();

                String sLast = (last.to == blc.getAddress()) ? "From: " : "To: ";
                sLast += (last.to == blc.getAddress()) ? last.from : last.to;
                sLast += " Amount: " + last.amount;

                // Print output :D
                Console.WriteLine("Address:           {0}", blc.getAddress());
                Console.WriteLine("Pwd:               {0}", blc.getPwd());
                Console.WriteLine("Coins:             {0}", coins.payload["amount"]);
                Console.WriteLine("Transaction count: {0}", trans.payload["transactions"].Count);
                Console.WriteLine("Last Transaction:  {0}", sLast);
            } catch (ki113d.CsBloocoin.Exceptions.ConnectionTimeoutException ohShit) {
                Console.WriteLine("ConnectionTimeoutException caught: {0}", ohShit.ErrorMessage);
            }
        }

    }

}
