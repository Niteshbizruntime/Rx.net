using org.apache.rocketmq.client.consumer;
using org.apache.rocketmq.client.consumer.listener;
using org.apache.rocketmq.common.consumer;
using org.apache.rocketmq.common.protocol.heartbeat;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Consumer
{
    class BroadcastConsumer
    {
        static void Main(string[] args)
        {
            DefaultMQPushConsumer consumer = new DefaultMQPushConsumer("BroadcastConsumerGroup");

            consumer.setConsumeFromWhere(ConsumeFromWhere.CONSUME_FROM_FIRST_OFFSET);

            
            consumer.setMessageModel(MessageModel.BROADCASTING);

            consumer.subscribe("BroadTopicTest", "TagA");

            consumer.registerMessageListener(new TestListener());

        consumer.start();
        Console.WriteLine("Broadcast Consumer Started.");
}
    }
}
