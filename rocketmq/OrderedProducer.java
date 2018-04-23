package com.bizruntime.producer;

import java.util.List;

import org.apache.rocketmq.client.producer.DefaultMQProducer;
import org.apache.rocketmq.client.producer.MQProducer;
import org.apache.rocketmq.client.producer.MessageQueueSelector;
import org.apache.rocketmq.client.producer.SendResult;
import org.apache.rocketmq.common.message.Message;
import org.apache.rocketmq.common.message.MessageQueue;
import org.apache.rocketmq.remoting.common.RemotingHelper;

public class OrderedProducer {
	 public static void main(String[] args) throws Exception {
	       
	        MQProducer producer = new DefaultMQProducer("GroupA");
	       
	        producer.start();
	        String[] tags = new String[] {"TagA", "TagB", "TagC", "TagD", "TagE"};
	        for (int i = 0; i < 100; i++) {
	            int orderId = i % 10;
	           
	            Message msg = new Message("TopicTestjjj", tags[i % tags.length], "KEY" + i,
	                    ("Hello Consumer " + i).getBytes(RemotingHelper.DEFAULT_CHARSET));
	            SendResult sendResult = producer.send(msg, new MessageQueueSelector() {
	            
	            public MessageQueue select(List<MessageQueue> mqs, Message msg, Object arg) {
	                Integer id = (Integer) arg;
	                int index = id % mqs.size();
	                return mqs.get(index);
	            }
	            }, orderId);

	            System.out.printf("%s%n", sendResult);
	        }
	       
	        producer.shutdown();
	    }
}
