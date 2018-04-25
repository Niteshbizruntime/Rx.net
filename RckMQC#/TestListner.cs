using java.util;
using org.apache.rocketmq.client.consumer.listener;
using org.apache.rocketmq.common.message;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Consumer
{
    internal class TestListner : MessageListenerConcurrently
    {

        public ConsumeConcurrentlyStatus consumeMessage(List list, ConsumeConcurrentlyContext ccc)
        {
            for (int i = 0; i < list.size(); i++)
            {
                var msg = list.get(i) as Message;

                Console.WriteLine(" Receive message " + msg.getBuyerId() + msg.getDelayTimeLevel() + "ms later");

            }
           
            return ConsumeConcurrentlyStatus.CONSUME_SUCCESS;
        }
    }
}
