using java.util;
using java.util.concurrent.atomic;
using org.apache.rocketmq.client.consumer.listener;
using org.apache.rocketmq.common.message;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Consumer
{
    internal class TestListener1 : MessageListenerOrderly
    {

        AtomicLong consumeTimes = new AtomicLong(0);
        public ConsumeOrderlyStatus consumeMessage(List list, ConsumeOrderlyContext context)
        {
            
            context.setAutoCommit(false);
            for (int i = 0; i < list.size(); i++)
            {
                var msg = list.get(i) as Message;
                byte[] body = msg.getBody();
                var str = Encoding.Default.GetString(body);
                Console.WriteLine(str);

            }
            this.consumeTimes.incrementAndGet();
            if ((this.consumeTimes.get() % 2) == 0)
            {
                return ConsumeOrderlyStatus.SUCCESS;
            }
            else if ((this.consumeTimes.get() % 3) == 0)
            {
                return ConsumeOrderlyStatus.ROLLBACK;
            }
            else if ((this.consumeTimes.get() % 4) == 0)
            {
                return ConsumeOrderlyStatus.COMMIT;
            }
            else if ((this.consumeTimes.get() % 5) == 0)
            {
                context.setSuspendCurrentQueueTimeMillis(3000);
                return ConsumeOrderlyStatus.SUSPEND_CURRENT_QUEUE_A_MOMENT;
            }
            return ConsumeOrderlyStatus.SUCCESS;
            
        }
    }
}
