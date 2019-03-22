using System;
using System.Net;
using System.Diagnostics;
using System.Collections.Generic;

namespace ServerClientCalculator
{
    public class GameServer
    {

        private delegate void GameCommand(byte[] data, EndPoint sender);

        private Dictionary<byte, GameCommand> commandsTable;

        private Dictionary<EndPoint, GameClient> clientsTable;

        private IGameTransport transport;

        public uint NumClients
        {
            get
            {
                return (uint)clientsTable.Count;
            }
        }

       
        private void Add(byte[] data, EndPoint sender)
        {
            if(!clientsTable.ContainsKey(sender))
            {
                GameClient newClient = new GameClient(this, sender);
                clientsTable[sender] = newClient;
            }

            float primoNum = BitConverter.ToSingle(data, 1);
            float secondoNum = BitConverter.ToSingle(data, 5);

            float sol = primoNum + secondoNum;
            Packet solution = new Packet(4, sol);
            clientsTable[sender].Enqueue(solution);
            

        }
        private void NewFunction()
        {
            Console.WriteLine("_____________");
        }
        private void Sub(byte[] data, EndPoint sender)
        {
            if (!clientsTable.ContainsKey(sender))
            {
                GameClient newClient = new GameClient(this, sender);
                clientsTable[sender] = newClient;
            }

            float primoNum = BitConverter.ToSingle(data, 1);
            float secondoNum = BitConverter.ToSingle(data, 5);

            float sol = primoNum - secondoNum;
            Packet solution = new Packet(4, sol);
            clientsTable[sender].Enqueue(solution);


        }

        private void Moltiplier(byte[] data, EndPoint sender)
        {
            if (!clientsTable.ContainsKey(sender))
            {
                GameClient newClient = new GameClient(this, sender);
                clientsTable[sender] = newClient;
            }

            float primoNum = BitConverter.ToSingle(data, 1);
            float secondoNum = BitConverter.ToSingle(data, 5);

            float sol = primoNum * secondoNum;
            Packet solution = new Packet(4, sol);
            clientsTable[sender].Enqueue(solution);


        }

        private void Divider(byte[] data, EndPoint sender)
        {
            if (!clientsTable.ContainsKey(sender))
            {
                GameClient newClient = new GameClient(this, sender);
                clientsTable[sender] = newClient;
            }

            float primoNum = BitConverter.ToSingle(data, 1);
            float secondoNum = BitConverter.ToSingle(data, 5);

            float sol = primoNum / secondoNum;
            Packet solution = new Packet(4, sol);
            clientsTable[sender].Enqueue(solution);


        }

        public GameServer(IGameTransport gameTransport)
        {
            transport = gameTransport;
            clientsTable = new Dictionary<EndPoint, GameClient>();
            commandsTable = new Dictionary<byte, GameCommand>();
            commandsTable[0] = Add;
            commandsTable[1] = Sub;
            commandsTable[2] = Moltiplier;
            commandsTable[3] = Divider;
        }


        public void Run()
        {

            Console.WriteLine("server started");
            while (true)
            {
                SingleStep();
            }
        }

        private float currentNow;
        public float Now
        {
            get
            {
                return currentNow;
            }
        }

        public void SingleStep()
        {
            EndPoint sender = transport.CreateEndPoint();
            byte[] data = transport.Recv(256, ref sender);
            if (data != null)
            {
                byte gameCommand = data[0];
                if (commandsTable.ContainsKey(gameCommand))
                {
                    commandsTable[gameCommand](data, sender);
                }
                else
                {
                    clientsTable[sender].Malus++;
                }
            }

            foreach (GameClient client in clientsTable.Values)
            {
                client.Process();
            }

          

        }


        public bool Send(Packet packet, EndPoint endPoint)
        {
            return transport.Send(packet.GetData(), endPoint);
        }

        public void SendToAllClients(Packet packet)
        {
            foreach (GameClient client in clientsTable.Values)
            {
                client.Enqueue(packet);
            }
        }

        public void SendToAllClientsExceptOne(Packet packet, GameClient except)
        {
            foreach (GameClient client in clientsTable.Values)
            {
                if (client != except)
                    client.Enqueue(packet);
            }
        }


        public GameClient GetClientFromEndPoint(EndPoint endpoint)
        {
            if (clientsTable.ContainsKey(endpoint))
            {
                return clientsTable[endpoint];
            }
            return null;
        }

    }
}
