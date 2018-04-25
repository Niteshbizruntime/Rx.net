using org.apache.rocketmq.client.producer;
using org.apache.rocketmq.common.message;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Producer
{
    class BroadCastProducer
    {
        static void Main(string[] args)
        {
            DefaultMQProducer producer = new DefaultMQProducer("ProducerGroupName");
            producer.start();
            String[] tags = new String[] { "TagA", "TagB" };
            for (int i = 0; i < 50; i++)
            {
                Message msg = new Message("BroadTopicTest",
                    tags[i % 2],
                    "OrderID188",
                    Encoding.Default.GetBytes("Hello world"+i));
                SendResult sendResult = producer.send(msg);
               Console.WriteLine(sendResult);
            }
            producer.shutdown();
        }
    }
}
