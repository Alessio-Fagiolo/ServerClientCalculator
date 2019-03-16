using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
namespace ServerClientCalculator.Test
{
    public class TestGameServer
    {
        private FakeTransport transport;
        private GameServer server;

        [SetUp]
        public void SetupTests()
        {
            transport = new FakeTransport();
            server = new GameServer(transport);
        }

        [Test]
        public void checkAdd()
        {
            Packet add = new Packet(0, 1.0f,2.0f);
            transport.ClientEnqueue(add, "test", 0);
            server.SingleStep();

            FakeData soluzione = transport.ClientDequeue();

            Assert.That(soluzione.data[0], Is.EqualTo(4));

        }

        [Test]
        public void checkAddResult()
        {
            Packet add = new Packet(0, 1.0f, 2.0f);
            transport.ClientEnqueue(add, "test", 0);
            server.SingleStep();

            FakeData soluzione = transport.ClientDequeue();
            float sol = BitConverter.ToSingle(soluzione.data, 1);
            Assert.That(sol, Is.EqualTo(3));

        }

        [Test]
        public void checkSub()
        {
            Packet sub = new Packet(1, 7.0f, 2.0f);
            transport.ClientEnqueue(sub, "test", 0);
            server.SingleStep();

            FakeData soluzione = transport.ClientDequeue();

            Assert.That(soluzione.data[0], Is.EqualTo(4));

        }

        [Test]
        public void checkSubResult()
        {
            Packet sub = new Packet(1, 7.0f, 2.0f);
            transport.ClientEnqueue(sub, "test", 0);
            server.SingleStep();

            FakeData soluzione = transport.ClientDequeue();
            float sol = BitConverter.ToSingle(soluzione.data, 1);
            Assert.That(sol, Is.EqualTo(5));

        }


        [Test]
        public void checkMoltiplier()
        {
            Packet moltiplier = new Packet(2, 5.0f, 2.0f);
            transport.ClientEnqueue(moltiplier, "test", 0);
            server.SingleStep();

            FakeData soluzione = transport.ClientDequeue();

            Assert.That(soluzione.data[0], Is.EqualTo(4));

        }

        [Test]
        public void checkMoltiplierResult()
        {
            Packet moltiplier = new Packet(2, 5.0f, 2.0f);
            transport.ClientEnqueue(moltiplier, "test", 0);
            server.SingleStep();

            FakeData soluzione = transport.ClientDequeue();
            float sol = BitConverter.ToSingle(soluzione.data, 1);
            Assert.That(sol, Is.EqualTo(10));

        }


        [Test]
        public void checkDivider()
        {
            Packet divider = new Packet(3, 8.0f, 2.0f);
            transport.ClientEnqueue(divider, "test", 0);
            server.SingleStep();

            FakeData soluzione = transport.ClientDequeue();

            Assert.That(soluzione.data[0], Is.EqualTo(4));

        }

        [Test]
        public void checkDividerResult()
        {
            Packet moltiplier = new Packet(3, 8.0f, 2.0f);
            transport.ClientEnqueue(moltiplier, "test", 0);
            server.SingleStep();

            FakeData soluzione = transport.ClientDequeue();
            float sol = BitConverter.ToSingle(soluzione.data, 1);
            Assert.That(sol, Is.EqualTo(4));

        }
    }
}
