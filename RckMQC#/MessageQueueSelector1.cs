using java.util;
using org.apache.rocketmq.client.producer;
using org.apache.rocketmq.common.message;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Producer
{
    class MessageQueueSelector1: MessageQueueSelector
    {
        


                public MessageQueue select(List mqs, Message msg, Object arg)
        {
            int id = (int)arg;
            int index = id % mqs.size();
            return (MessageQueue)mqs.get(index);
        }
    
}
}
